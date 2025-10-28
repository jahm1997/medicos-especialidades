<template>
  <div class="citas-container">
    <div class="header">
      <h2>Gestión de Citas Médicas</h2>
      <button @click="showCreateModal = true" class="btn btn-primary">
        <i class="fas fa-plus"></i> Nueva Cita
      </button>
    </div>

    <!-- Filtros -->
    <div class="filters">
      <div class="filter-group">
        <label>Estado:</label>
        <select v-model="selectedEstado" @change="loadCitas" class="form-control">
          <option value="">Todos</option>
          <option value="Pendiente">Pendiente</option>
          <option value="Completada">Completada</option>
          <option value="Cancelada">Cancelada</option>
        </select>
      </div>
      <div class="filter-group">
        <label>Fecha:</label>
        <input v-model="selectedDate" @change="loadCitas" type="date" class="form-control" />
      </div>
      <div class="filter-group">
        <label>Médico:</label>
        <select v-model="selectedMedico" @change="loadCitas" class="form-control">
          <option value="">Todos los médicos</option>
          <option v-for="medico in medicos" :key="medico.id" :value="medico.id">
            Dr. {{ medico.nombre }} {{ medico.apellido }}
          </option>
        </select>
      </div>
    </div>

    <!-- Tabla de citas -->
    <div class="table-container">
      <table class="table">
        <thead>
          <tr>
            <th>Fecha/Hora</th>
            <th>Paciente</th>
            <th>Médico</th>
            <th>Especialidad</th>
            <th>Estado</th>
            <th>Acciones</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="cita in citas" :key="cita.id">
            <td>{{ formatDateTime(cita.fechaHora) }}</td>
            <td>{{ cita.paciente?.nombre }} {{ cita.paciente?.apellido }}</td>
            <td>Dr. {{ cita.medico?.nombre }} {{ cita.medico?.apellido }}</td>
            <td>{{ cita.especialidad?.nombre }}</td>
            <td>
              <span :class="['status', cita.estado.toLowerCase()]">
                {{ cita.estado }}
              </span>
            </td>
            <td>
              <button @click="viewCita(cita)" class="btn btn-info btn-sm">
                <i class="fas fa-eye"></i>
              </button>
              <button v-if="cita.estado === 'Pendiente'" @click="editCita(cita)" class="btn btn-warning btn-sm">
                <i class="fas fa-edit"></i>
              </button>
              <button v-if="cita.estado === 'Pendiente'" @click="completarCita(cita)" class="btn btn-success btn-sm">
                <i class="fas fa-check"></i>
              </button>
              <button v-if="cita.estado === 'Pendiente'" @click="cancelarCita(cita)" class="btn btn-danger btn-sm">
                <i class="fas fa-times"></i>
              </button>
            </td>
          </tr>
        </tbody>
      </table>
    </div>

    <!-- Modal para crear/editar cita -->
    <div v-if="showCreateModal || showEditModal" class="modal-overlay" @click="closeModals">
      <div class="modal-content large-modal" @click.stop>
        <div class="modal-header">
          <h3>{{ showCreateModal ? 'Nueva Cita' : 'Editar Cita' }}</h3>
          <button @click="closeModals" class="close-btn">&times;</button>
        </div>
        <form @submit.prevent="saveCita" class="modal-body">
          <div class="form-row">
            <div class="form-group">
              <label>Paciente *</label>
              <select v-model="currentCita.pacienteId" class="form-control" required>
                <option value="">Seleccione un paciente</option>
                <option v-for="paciente in pacientes" :key="paciente.id" :value="paciente.id">
                  {{ paciente.nombre }} {{ paciente.apellido }} - {{ paciente.cedula }}
                </option>
              </select>
            </div>
            <div class="form-group">
              <label>Especialidad *</label>
              <select v-model="currentCita.especialidadId" @change="loadMedicosByEspecialidad" class="form-control" required>
                <option value="">Seleccione una especialidad</option>
                <option v-for="especialidad in especialidades" :key="especialidad.id" :value="especialidad.id">
                  {{ especialidad.nombre }}
                </option>
              </select>
            </div>
          </div>
          <div class="form-row">
            <div class="form-group">
              <label>Médico *</label>
              <select v-model="currentCita.medicoId" @change="loadHorariosByMedico" class="form-control" required>
                <option value="">Seleccione un médico</option>
                <option v-for="medico in medicosDisponibles" :key="medico.id" :value="medico.id">
                  Dr. {{ medico.nombre }} {{ medico.apellido }}
                </option>
              </select>
            </div>
            <div class="form-group">
              <label>Fecha *</label>
              <input v-model="currentCita.fecha" @change="loadHorariosByMedico" type="date" class="form-control" required />
            </div>
          </div>
          <div class="form-row">
            <div class="form-group">
              <label>Horario Disponible *</label>
              <select v-model="currentCita.horarioDisponibleId" class="form-control" required>
                <option value="">Seleccione un horario</option>
                <option v-for="horario in horariosDisponibles" :key="horario.id" :value="horario.id">
                  {{ formatTime(horario.horaInicio) }} - {{ formatTime(horario.horaFin) }}
                </option>
              </select>
            </div>
            <div class="form-group">
              <label>Estado</label>
              <select v-model="currentCita.estado" class="form-control">
                <option value="Pendiente">Pendiente</option>
                <option value="Completada">Completada</option>
                <option value="Cancelada">Cancelada</option>
              </select>
            </div>
          </div>
          <div class="form-group">
            <label>Motivo de la Consulta</label>
            <textarea v-model="currentCita.motivo" class="form-control" rows="3"></textarea>
          </div>
          <div class="form-group">
            <label>Observaciones</label>
            <textarea v-model="currentCita.observaciones" class="form-control" rows="3"></textarea>
          </div>
          <div class="modal-footer">
            <button type="button" @click="closeModals" class="btn btn-secondary">Cancelar</button>
            <button type="submit" class="btn btn-primary">Guardar</button>
          </div>
        </form>
      </div>
    </div>

    <!-- Modal para ver detalles de la cita -->
    <div v-if="showViewModal" class="modal-overlay" @click="closeModals">
      <div class="modal-content" @click.stop>
        <div class="modal-header">
          <h3>Detalles de la Cita</h3>
          <button @click="closeModals" class="close-btn">&times;</button>
        </div>
        <div class="modal-body">
          <div class="appointment-details">
            <div class="detail-section">
              <h4>Información General</h4>
              <div class="detail-row">
                <strong>Fecha y Hora:</strong> {{ formatDateTime(viewingCita.fechaHora) }}
              </div>
              <div class="detail-row">
                <strong>Estado:</strong> 
                <span :class="['status', viewingCita.estado?.toLowerCase()]">
                  {{ viewingCita.estado }}
                </span>
              </div>
            </div>
            
            <div class="detail-section">
              <h4>Paciente</h4>
              <div class="detail-row">
                <strong>Nombre:</strong> {{ viewingCita.paciente?.nombre }} {{ viewingCita.paciente?.apellido }}
              </div>
              <div class="detail-row">
                <strong>Cédula:</strong> {{ viewingCita.paciente?.cedula }}
              </div>
              <div class="detail-row">
                <strong>Email:</strong> {{ viewingCita.paciente?.email }}
              </div>
            </div>
            
            <div class="detail-section">
              <h4>Médico y Especialidad</h4>
              <div class="detail-row">
                <strong>Médico:</strong> Dr. {{ viewingCita.medico?.nombre }} {{ viewingCita.medico?.apellido }}
              </div>
              <div class="detail-row">
                <strong>Especialidad:</strong> {{ viewingCita.especialidad?.nombre }}
              </div>
            </div>
            
            <div class="detail-section">
              <h4>Detalles de la Consulta</h4>
              <div class="detail-row">
                <strong>Motivo:</strong> {{ viewingCita.motivo || 'No especificado' }}
              </div>
              <div class="detail-row">
                <strong>Observaciones:</strong> {{ viewingCita.observaciones || 'Sin observaciones' }}
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>

    <!-- Modal para completar cita -->
    <div v-if="showCompleteModal" class="modal-overlay" @click="closeModals">
      <div class="modal-content" @click.stop>
        <div class="modal-header">
          <h3>Completar Cita</h3>
          <button @click="closeModals" class="close-btn">&times;</button>
        </div>
        <form @submit.prevent="confirmarCompletarCita" class="modal-body">
          <div class="form-group">
            <label>Observaciones de la Consulta</label>
            <textarea v-model="observacionesCompletar" class="form-control" rows="4" placeholder="Ingrese las observaciones de la consulta..."></textarea>
          </div>
          <div class="modal-footer">
            <button type="button" @click="closeModals" class="btn btn-secondary">Cancelar</button>
            <button type="submit" class="btn btn-success">Completar Cita</button>
          </div>
        </form>
      </div>
    </div>

    <!-- Modal para cancelar cita -->
    <div v-if="showCancelModal" class="modal-overlay" @click="closeModals">
      <div class="modal-content" @click.stop>
        <div class="modal-header">
          <h3>Cancelar Cita</h3>
          <button @click="closeModals" class="close-btn">&times;</button>
        </div>
        <form @submit.prevent="confirmarCancelarCita" class="modal-body">
          <div class="form-group">
            <label>Motivo de Cancelación</label>
            <textarea v-model="motivoCancelacion" class="form-control" rows="3" placeholder="Ingrese el motivo de la cancelación..." required></textarea>
          </div>
          <div class="modal-footer">
            <button type="button" @click="closeModals" class="btn btn-secondary">Cancelar</button>
            <button type="submit" class="btn btn-danger">Cancelar Cita</button>
          </div>
        </form>
      </div>
    </div>
  </div>
</template>

<script>
import { citaService, pacienteService, medicoService, especialidadService, horarioService } from '../services/api.js'

export default {
  name: 'Citas',
  data() {
    return {
      citas: [],
      pacientes: [],
      medicos: [],
      especialidades: [],
      medicosDisponibles: [],
      horariosDisponibles: [],
      selectedEstado: '',
      selectedDate: '',
      selectedMedico: '',
      showCreateModal: false,
      showEditModal: false,
      showViewModal: false,
      showCompleteModal: false,
      showCancelModal: false,
      currentCita: {
        pacienteId: '',
        medicoId: '',
        especialidadId: '',
        horarioDisponibleId: '',
        fecha: '',
        motivo: '',
        observaciones: '',
        estado: 'Pendiente'
      },
      viewingCita: {},
      citaToComplete: null,
      citaToCancel: null,
      observacionesCompletar: '',
      motivoCancelacion: ''
    }
  },
  async mounted() {
    await this.loadInitialData()
    await this.loadCitas()
  },
  methods: {
    async loadInitialData() {
      try {
        const [pacientesRes, medicosRes, especialidadesRes] = await Promise.all([
          pacienteService.getAll(),
          medicoService.getAll(),
          especialidadService.getAll()
        ])
        
        this.pacientes = pacientesRes.data
        this.medicos = medicosRes.data
        this.especialidades = especialidadesRes.data
      } catch (error) {
        console.error('Error cargando datos iniciales:', error)
        this.$toast.error('Error al cargar los datos iniciales')
      }
    },
    async loadCitas() {
      try {
        let response
        
        if (this.selectedEstado === 'Pendiente') {
          response = await citaService.getPendientes()
        } else if (this.selectedDate) {
          response = await citaService.getByFecha(this.selectedDate)
        } else if (this.selectedMedico) {
          response = await citaService.getByMedico(this.selectedMedico)
        } else {
          response = await citaService.getAll()
        }
        
        this.citas = response.data
        
        // Filtrar por estado si no es "Pendiente" específicamente
        if (this.selectedEstado && this.selectedEstado !== 'Pendiente') {
          this.citas = this.citas.filter(cita => cita.estado === this.selectedEstado)
        }
      } catch (error) {
        console.error('Error cargando citas:', error)
        this.$toast.error('Error al cargar las citas')
      }
    },
    async loadMedicosByEspecialidad() {
      if (this.currentCita.especialidadId) {
        try {
          const response = await medicoService.getByEspecialidad(this.currentCita.especialidadId)
          this.medicosDisponibles = response.data
        } catch (error) {
          console.error('Error al cargar médicos por especialidad:', error)
          this.medicosDisponibles = []
        }
      } else {
        this.medicosDisponibles = []
      }
      this.horariosDisponibles = []
      this.currentCita.medicoId = ''
      this.currentCita.horarioDisponibleId = ''
    },
    async loadHorariosByMedico() {
      if (this.currentCita.medicoId && this.currentCita.fecha) {
        try {
          const response = await horarioService.getDisponibles(this.currentCita.medicoId, this.currentCita.fecha)
          this.horariosDisponibles = response.data.filter(h => h.disponible)
        } catch (error) {
          console.error('Error al cargar horarios:', error)
          this.horariosDisponibles = []
        }
      } else {
        this.horariosDisponibles = []
      }
      this.currentCita.horarioDisponibleId = ''
    },
    async viewCita(cita) {
      try {
        const response = await citaService.getById(cita.id)
        this.viewingCita = response.data
        this.showViewModal = true
      } catch (error) {
        console.error('Error al cargar detalles de la cita:', error)
        alert('Error al cargar los detalles de la cita')
      }
    },
    editCita(cita) {
      this.currentCita = {
        ...cita,
        fecha: this.formatDateForInput(cita.fechaHora),
        pacienteId: cita.paciente?.id,
        medicoId: cita.medico?.id,
        especialidadId: cita.especialidad?.id,
        horarioDisponibleId: cita.horarioDisponible?.id
      }
      this.loadMedicosByEspecialidad()
      this.showEditModal = true
    },
    completarCita(cita) {
      this.citaToComplete = cita
      this.observacionesCompletar = ''
      this.showCompleteModal = true
    },
    cancelarCita(cita) {
      this.citaToCancel = cita
      this.motivoCancelacion = ''
      this.showCancelModal = true
    },
    async confirmarCompletarCita() {
      try {
        await citaService.completar(this.citaToComplete.id, this.observacionesCompletar)
        alert('Cita completada correctamente')
        await this.loadCitas()
        this.closeModals()
      } catch (error) {
        console.error('Error al completar cita:', error)
        alert('Error al completar la cita')
      }
    },
    async confirmarCancelarCita() {
      try {
        await citaService.cancelar(this.citaToCancel.id, this.motivoCancelacion)
        alert('Cita cancelada correctamente')
        await this.loadCitas()
        this.closeModals()
      } catch (error) {
        console.error('Error al cancelar cita:', error)
        alert('Error al cancelar la cita')
      }
    },
    async saveCita() {
      try {
        // Construir el objeto de cita con la fecha y hora
        const citaData = {
          ...this.currentCita,
          fechaHora: this.currentCita.fecha + 'T00:00:00' // Ajustar según el horario seleccionado
        }
        
        if (this.showCreateModal) {
          await citaService.create(citaData)
          alert('Cita creada correctamente')
        } else {
          await citaService.update(this.currentCita.id, citaData)
          alert('Cita actualizada correctamente')
        }
        await this.loadCitas()
        this.closeModals()
      } catch (error) {
        console.error('Error al guardar cita:', error)
        alert('Error al guardar la cita')
      }
    },
    closeModals() {
      this.showCreateModal = false
      this.showEditModal = false
      this.showViewModal = false
      this.showCompleteModal = false
      this.showCancelModal = false
      this.currentCita = {
        pacienteId: '',
        medicoId: '',
        especialidadId: '',
        horarioDisponibleId: '',
        fecha: '',
        motivo: '',
        observaciones: '',
        estado: 'Pendiente'
      }
      this.viewingCita = {}
      this.citaToComplete = null
      this.citaToCancel = null
      this.observacionesCompletar = ''
      this.motivoCancelacion = ''
      this.medicosDisponibles = []
      this.horariosDisponibles = []
    },
    formatDateTime(dateTimeString) {
      if (!dateTimeString) return ''
      return new Date(dateTimeString).toLocaleString('es-ES')
    },
    formatTime(timeString) {
      if (!timeString) return ''
      return timeString.substring(0, 5) // HH:MM
    },
    formatDateForInput(dateTimeString) {
      if (!dateTimeString) return ''
      return new Date(dateTimeString).toISOString().split('T')[0]
    }
  }
}
</script>

<style scoped>
.citas-container {
  padding: 20px;
}

.header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 20px;
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

.status.pendiente {
  background-color: #fff3cd;
  color: #856404;
}

.status.completada {
  background-color: #d4edda;
  color: #155724;
}

.status.cancelada {
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

.btn-info {
  background-color: #17a2b8;
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

.large-modal {
  max-width: 800px;
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

.modal-footer {
  display: flex;
  justify-content: flex-end;
  gap: 10px;
  margin-top: 20px;
}

.appointment-details {
  display: flex;
  flex-direction: column;
  gap: 20px;
}

.detail-section {
  border-bottom: 1px solid #eee;
  padding-bottom: 15px;
}

.detail-section:last-child {
  border-bottom: none;
}

.detail-section h4 {
  margin-bottom: 10px;
  color: #333;
  font-size: 16px;
}

.detail-row {
  padding: 5px 0;
  display: flex;
  align-items: center;
  gap: 10px;
}
</style>