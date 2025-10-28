<template>
  <div class="horarios-container">
    <div class="header">
      <h2>Gestión de Horarios Disponibles</h2>
      <div class="header-actions">
        <button @click="showCreateModal = true" class="btn btn-primary">
          <i class="fas fa-plus"></i> Nuevo Horario
        </button>
        <button @click="showRecurrentModal = true" class="btn btn-success">
          <i class="fas fa-calendar-alt"></i> Horarios Recurrentes
        </button>
      </div>
    </div>

    <!-- Filtros -->
    <div class="filters">
      <div class="filter-group">
        <label>Médico:</label>
        <select v-model="selectedMedico" @change="loadHorarios" class="form-control">
          <option value="">Todos los médicos</option>
          <option v-for="medico in medicos" :key="medico.id" :value="medico.id">
            Dr. {{ medico.nombre }} {{ medico.apellido }}
          </option>
        </select>
      </div>
      <div class="filter-group">
        <label>Fecha:</label>
        <input v-model="selectedDate" @change="loadHorarios" type="date" class="form-control" />
      </div>
    </div>

    <!-- Tabla de horarios -->
    <div class="table-container">
      <table class="table">
        <thead>
          <tr>
            <th>Médico</th>
            <th>Fecha</th>
            <th>Hora Inicio</th>
            <th>Hora Fin</th>
            <th>Estado</th>
            <th>Acciones</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="horario in horarios" :key="horario.id">
            <td>Dr. {{ horario.medico?.nombre }} {{ horario.medico?.apellido }}</td>
            <td>{{ formatDate(horario.fecha) }}</td>
            <td>{{ formatTime(horario.horaInicio) }}</td>
            <td>{{ formatTime(horario.horaFin) }}</td>
            <td>
              <span :class="['status', horario.disponible ? 'disponible' : 'no-disponible']">
                {{ horario.disponible ? 'Disponible' : 'No Disponible' }}
              </span>
            </td>
            <td>
              <button @click="editHorario(horario)" class="btn btn-warning btn-sm">
                <i class="fas fa-edit"></i>
              </button>
              <button 
                v-if="horario.disponible" 
                @click="marcarNoDisponible(horario)" 
                class="btn btn-secondary btn-sm"
              >
                <i class="fas fa-ban"></i>
              </button>
              <button @click="deleteHorario(horario)" class="btn btn-danger btn-sm">
                <i class="fas fa-trash"></i>
              </button>
            </td>
          </tr>
        </tbody>
      </table>
    </div>

    <!-- Modal para crear/editar horario -->
    <div v-if="showCreateModal || showEditModal" class="modal-overlay" @click="closeModals">
      <div class="modal-content" @click.stop>
        <div class="modal-header">
          <h3>{{ showCreateModal ? 'Nuevo Horario' : 'Editar Horario' }}</h3>
          <button @click="closeModals" class="close-btn">&times;</button>
        </div>
        <form @submit.prevent="saveHorario" class="modal-body">
          <div class="form-group">
            <label>Médico *</label>
            <select v-model="currentHorario.medicoId" class="form-control" required>
              <option value="">Seleccione un médico</option>
              <option v-for="medico in medicos" :key="medico.id" :value="medico.id">
                Dr. {{ medico.nombre }} {{ medico.apellido }}
              </option>
            </select>
          </div>
          <div class="form-row">
            <div class="form-group">
              <label>Fecha *</label>
              <input v-model="currentHorario.fecha" type="date" class="form-control" required />
            </div>
            <div class="form-group">
              <label>Disponible</label>
              <input v-model="currentHorario.disponible" type="checkbox" class="form-check" />
            </div>
          </div>
          <div class="form-row">
            <div class="form-group">
              <label>Hora Inicio *</label>
              <input v-model="currentHorario.horaInicio" type="time" class="form-control" required />
            </div>
            <div class="form-group">
              <label>Hora Fin *</label>
              <input v-model="currentHorario.horaFin" type="time" class="form-control" required />
            </div>
          </div>
          <div class="modal-footer">
            <button type="button" @click="closeModals" class="btn btn-secondary">Cancelar</button>
            <button type="submit" class="btn btn-primary">Guardar</button>
          </div>
        </form>
      </div>
    </div>

    <!-- Modal para horarios recurrentes -->
    <div v-if="showRecurrentModal" class="modal-overlay" @click="closeModals">
      <div class="modal-content" @click.stop>
        <div class="modal-header">
          <h3>Crear Horarios Recurrentes</h3>
          <button @click="closeModals" class="close-btn">&times;</button>
        </div>
        <form @submit.prevent="saveHorariosRecurrentes" class="modal-body">
          <div class="form-group">
            <label>Médico *</label>
            <select v-model="recurrentHorario.medicoId" class="form-control" required>
              <option value="">Seleccione un médico</option>
              <option v-for="medico in medicos" :key="medico.id" :value="medico.id">
                Dr. {{ medico.nombre }} {{ medico.apellido }}
              </option>
            </select>
          </div>
          <div class="form-row">
            <div class="form-group">
              <label>Fecha Inicio *</label>
              <input v-model="recurrentHorario.fechaInicio" type="date" class="form-control" required />
            </div>
            <div class="form-group">
              <label>Fecha Fin *</label>
              <input v-model="recurrentHorario.fechaFin" type="date" class="form-control" required />
            </div>
          </div>
          <div class="form-row">
            <div class="form-group">
              <label>Hora Inicio *</label>
              <input v-model="recurrentHorario.horaInicio" type="time" class="form-control" required />
            </div>
            <div class="form-group">
              <label>Hora Fin *</label>
              <input v-model="recurrentHorario.horaFin" type="time" class="form-control" required />
            </div>
          </div>
          <div class="form-group">
            <label>Días de la Semana:</label>
            <div class="days-selector">
              <label v-for="(day, index) in diasSemana" :key="index" class="day-checkbox">
                <input 
                  type="checkbox" 
                  :value="index" 
                  v-model="recurrentHorario.diasSemana"
                />
                {{ day }}
              </label>
            </div>
          </div>
          <div class="modal-footer">
            <button type="button" @click="closeModals" class="btn btn-secondary">Cancelar</button>
            <button type="submit" class="btn btn-success">Crear Horarios</button>
          </div>
        </form>
      </div>
    </div>
  </div>
</template>

<script>
import { horarioService, medicoService } from '../services/api.js'
import { notify, showNoDataState } from '../utils/notifications.js'


export default {
  name: 'HorariosDisponibles',
  data() {
    return {
      horarios: [],
      medicos: [],
      selectedMedico: '',
      selectedDate: '',
      showCreateModal: false,
      showEditModal: false,
      showRecurrentModal: false,
      currentHorario: {
        medicoId: '',
        fecha: '',
        horaInicio: '',
        horaFin: '',
        disponible: true
      },
      recurrentHorario: {
        medicoId: '',
        fechaInicio: '',
        fechaFin: '',
        horaInicio: '',
        horaFin: '',
        diasSemana: []
      },
      diasSemana: ['Domingo', 'Lunes', 'Martes', 'Miércoles', 'Jueves', 'Viernes', 'Sábado']
    }
  },
  async mounted() {
    await this.loadMedicos()
    await this.loadHorarios()
  },
  methods: {
    async loadMedicos() {
      try {
        const response = await medicoService.getAll()
        this.medicos = response.data
      } catch (error) {
        console.error('Error cargando médicos:', error)
        notify.error('Error al cargar médicos')
      }
    },
    async loadHorarios() {
      try {
        let response;
        if (this.selectedMedico) {
          response = await horarioService.getByMedico(this.selectedMedico)
          this.horarios = response.data
        } else {
          response = await horarioService.getAll()
          this.horarios = response.data
        }
      } catch (error) {
        console.error('Error cargando horarios:', error)
        notify.error('Error al cargar horarios')
      }
    },
    editHorario(horario) {
      this.currentHorario = {
        ...horario,
        fecha: this.formatDateForInput(horario.fecha),
        horaInicio: this.formatTimeForInput(horario.horaInicio),
        horaFin: this.formatTimeForInput(horario.horaFin)
      }
      this.showEditModal = true
    },
    async marcarNoDisponible(horario) {
       if (confirm('¿Está seguro de marcar este horario como no disponible?')) {
         try {
           await horarioService.marcarNoDisponible(horario.id)
           await this.loadHorarios()
           notify.success('Horario marcado como no disponible')
         } catch (error) {
           console.error('Error al marcar horario:', error)
           notify.error(error.userMessage || 'Error al marcar el horario como no disponible')
         }
       }
     },
    async deleteHorario(horario) {
      if (confirm(`¿Está seguro de eliminar el horario del ${horario.fechaHora}?`)) {
        try {
          await horarioService.delete(horario.id)
          await this.loadHorarios()
          notify.success('Horario eliminado correctamente')
        } catch (error) {
          console.error('Error al eliminar horario:', error)
          notify.error(error.userMessage || 'Error al eliminar el horario')
        }
      }
    },
    async saveHorario() {
      try {
        if (this.showCreateModal) {
          await horarioService.create(this.currentHorario)
          notify.success('Horario creado correctamente')
        } else {
          await horarioService.update(this.currentHorario.id, this.currentHorario)
          notify.success('Horario actualizado correctamente')
        }
        await this.loadHorarios()
        this.closeModals()
      } catch (error) {
        console.error('Error al guardar horario:', error)
        notify.error(error.userMessage || 'Error al guardar el horario')
      }
    },
    async saveHorariosRecurrentes() {
       try {
         await horarioService.createRecurrentes(this.recurrentHorario)
         notify.success('Horarios recurrentes creados correctamente')
         await this.loadHorarios()
         this.closeModals()
       } catch (error) {
         console.error('Error al crear horarios recurrentes:', error)
         notify.error(error.userMessage || 'Error al crear los horarios recurrentes')
       }
     },
    closeModals() {
      this.showCreateModal = false
      this.showEditModal = false
      this.showRecurrentModal = false
      this.currentHorario = {
        medicoId: '',
        fecha: '',
        horaInicio: '',
        horaFin: '',
        disponible: true
      }
      this.recurrentHorario = {
        medicoId: '',
        fechaInicio: '',
        fechaFin: '',
        horaInicio: '',
        horaFin: '',
        diasSemana: []
      }
    },
    formatDate(dateString) {
      if (!dateString) return ''
      return new Date(dateString).toLocaleDateString('es-ES')
    },
    formatTime(timeString) {
      if (!timeString) return ''
      return timeString.substring(0, 5) // HH:MM
    },
    formatDateForInput(dateString) {
      if (!dateString) return ''
      return new Date(dateString).toISOString().split('T')[0]
    },
    formatTimeForInput(timeString) {
      if (!timeString) return ''
      return timeString.substring(0, 5) // HH:MM
    }
  }
}
</script>

<style scoped>
.horarios-container {
  padding: 20px;
}

.header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 20px;
}

.header-actions {
  display: flex;
  gap: 10px;
}

.filters {
  display: flex;
  gap: 20px;
  margin-bottom: 20px;
  padding: 15px;
  background: white;
  border-radius: 8px;
  box-shadow: 0 2px 4px rgba(0,0,0,0.1);
}

.filter-group {
  display: flex;
  flex-direction: column;
  gap: 5px;
}

.filter-group label {
  font-weight: 500;
  font-size: 14px;
}

.table-container {
  background: white;
  border-radius: 8px;
  overflow: hidden;
  box-shadow: 0 2px 4px rgba(0,0,0,0.1);
}

.table {
  width: 100%;
  border-collapse: collapse;
}

.table th,
.table td {
  padding: 12px;
  text-align: left;
  border-bottom: 1px solid #eee;
}

.table th {
  background-color: #f8f9fa;
  font-weight: 600;
}

.status {
  padding: 4px 8px;
  border-radius: 12px;
  font-size: 12px;
  font-weight: 500;
}

.status.disponible {
  background-color: #d4edda;
  color: #155724;
}

.status.no-disponible {
  background-color: #f8d7da;
  color: #721c24;
}

.btn {
  padding: 8px 16px;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  font-size: 14px;
  margin-right: 5px;
}

.btn-primary {
  background-color: #007bff;
  color: white;
}

.btn-success {
  background-color: #28a745;
  color: white;
}

.btn-warning {
  background-color: #ffc107;
  color: #212529;
}

.btn-danger {
  background-color: #dc3545;
  color: white;
}

.btn-secondary {
  background-color: #6c757d;
  color: white;
}

.btn-sm {
  padding: 4px 8px;
  font-size: 12px;
}

.modal-overlay {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background-color: rgba(0, 0, 0, 0.5);
  display: flex;
  justify-content: center;
  align-items: center;
  z-index: 1000;
}

.modal-content {
  background: white;
  border-radius: 8px;
  width: 90%;
  max-width: 600px;
  max-height: 90vh;
  overflow-y: auto;
}

.modal-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 20px;
  border-bottom: 1px solid #eee;
}

.close-btn {
  background: none;
  border: none;
  font-size: 24px;
  cursor: pointer;
}

.modal-body {
  padding: 20px;
}

.form-row {
  display: flex;
  gap: 15px;
  margin-bottom: 15px;
}

.form-group {
  flex: 1;
  margin-bottom: 15px;
}

.form-group label {
  display: block;
  margin-bottom: 5px;
  font-weight: 500;
}

.form-control {
  width: 100%;
  padding: 8px 12px;
  border: 1px solid #ddd;
  border-radius: 4px;
  font-size: 14px;
}

.form-check {
  width: auto;
  margin-top: 8px;
}

.modal-footer {
  display: flex;
  justify-content: flex-end;
  gap: 10px;
  margin-top: 20px;
}

.days-selector {
  display: flex;
  flex-wrap: wrap;
  gap: 10px;
  margin-top: 5px;
}

.day-checkbox {
  display: flex;
  align-items: center;
  gap: 5px;
  font-size: 14px;
  cursor: pointer;
}

.day-checkbox input {
  width: auto;
}
</style>