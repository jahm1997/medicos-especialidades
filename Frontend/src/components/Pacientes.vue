<template>
  <div class="pacientes-container">
    <div class="header">
      <h2>Gestión de Pacientes</h2>
      <button @click="showCreateModal = true" class="btn btn-primary">
        <i class="fas fa-plus"></i> Nuevo Paciente
      </button>
    </div>

    <!-- Filtros y búsqueda -->
    <div class="filters">
      <div class="search-box">
        <input
          v-model="searchTerm"
          @input="searchPacientes"
          type="text"
          placeholder="Buscar por nombre, cédula o email..."
          class="form-control"
        />
        <i class="fas fa-search"></i>
      </div>
    </div>

    <!-- Tabla de pacientes -->
    <div class="table-container">
      <table class="table">
        <thead>
          <tr>
            <th>Cédula</th>
            <th>Nombre</th>
            <th>Email</th>
            <th>Teléfono</th>
            <th>Fecha Nacimiento</th>
            <th>Acciones</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="paciente in pacientes" :key="paciente.id">
            <td>{{ paciente.cedula }}</td>
            <td>{{ paciente.nombre }} {{ paciente.apellido }}</td>
            <td>{{ paciente.email }}</td>
            <td>{{ paciente.telefono }}</td>
            <td>{{ formatDate(paciente.fechaNacimiento) }}</td>
            <td>
              <button @click="viewPaciente(paciente)" class="btn btn-info btn-sm">
                <i class="fas fa-eye"></i>
              </button>
              <button @click="editPaciente(paciente)" class="btn btn-warning btn-sm">
                <i class="fas fa-edit"></i>
              </button>
              <button @click="deletePaciente(paciente)" class="btn btn-danger btn-sm">
                <i class="fas fa-trash"></i>
              </button>
            </td>
          </tr>
        </tbody>
      </table>
    </div>

    <!-- Modal para crear/editar paciente -->
    <div v-if="showCreateModal || showEditModal" class="modal-overlay" @click="closeModals">
      <div class="modal-content" @click.stop>
        <div class="modal-header">
          <h3>{{ showCreateModal ? 'Nuevo Paciente' : 'Editar Paciente' }}</h3>
          <button @click="closeModals" class="close-btn">&times;</button>
        </div>
        <form @submit.prevent="savePaciente" class="modal-body">
          <div class="form-row">
            <div class="form-group">
              <label>Cédula *</label>
              <input v-model="currentPaciente.cedula" type="text" class="form-control" required />
            </div>
            <div class="form-group">
              <label>Nombre *</label>
              <input v-model="currentPaciente.nombre" type="text" class="form-control" required />
            </div>
          </div>
          <div class="form-row">
            <div class="form-group">
              <label>Apellido *</label>
              <input v-model="currentPaciente.apellido" type="text" class="form-control" required />
            </div>
            <div class="form-group">
              <label>Email *</label>
              <input v-model="currentPaciente.email" type="email" class="form-control" required />
            </div>
          </div>
          <div class="form-row">
            <div class="form-group">
              <label>Teléfono</label>
              <input v-model="currentPaciente.telefono" type="text" class="form-control" />
            </div>
            <div class="form-group">
              <label>Fecha de Nacimiento *</label>
              <input v-model="currentPaciente.fechaNacimiento" type="date" class="form-control" required />
            </div>
          </div>
          <div class="form-group">
            <label>Dirección</label>
            <textarea v-model="currentPaciente.direccion" class="form-control" rows="3"></textarea>
          </div>
          <div class="modal-footer">
            <button type="button" @click="closeModals" class="btn btn-secondary">Cancelar</button>
            <button type="submit" class="btn btn-primary">Guardar</button>
          </div>
        </form>
      </div>
    </div>

    <!-- Modal para ver detalles del paciente -->
    <div v-if="showViewModal" class="modal-overlay" @click="closeModals">
      <div class="modal-content" @click.stop>
        <div class="modal-header">
          <h3>Detalles del Paciente</h3>
          <button @click="closeModals" class="close-btn">&times;</button>
        </div>
        <div class="modal-body">
          <div class="patient-details">
            <div class="detail-row">
              <strong>Cédula:</strong> {{ viewingPaciente.cedula }}
            </div>
            <div class="detail-row">
              <strong>Nombre:</strong> {{ viewingPaciente.nombre }} {{ viewingPaciente.apellido }}
            </div>
            <div class="detail-row">
              <strong>Email:</strong> {{ viewingPaciente.email }}
            </div>
            <div class="detail-row">
              <strong>Teléfono:</strong> {{ viewingPaciente.telefono }}
            </div>
            <div class="detail-row">
              <strong>Fecha de Nacimiento:</strong> {{ formatDate(viewingPaciente.fechaNacimiento) }}
            </div>
            <div class="detail-row">
              <strong>Dirección:</strong> {{ viewingPaciente.direccion }}
            </div>
          </div>
          
          <!-- Citas del paciente -->
          <div v-if="viewingPaciente.citas && viewingPaciente.citas.length > 0" class="appointments-section">
            <h4>Citas</h4>
            <div class="appointments-list">
              <div v-for="cita in viewingPaciente.citas" :key="cita.id" class="appointment-item">
                <div class="appointment-info">
                  <strong>{{ formatDateTime(cita.fechaHora) }}</strong>
                  <span class="specialty">{{ cita.especialidad?.nombre }}</span>
                  <span class="doctor">Dr. {{ cita.medico?.nombre }} {{ cita.medico?.apellido }}</span>
                  <span :class="['status', cita.estado.toLowerCase()]">{{ cita.estado }}</span>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { pacienteService } from '../services/api.js'
import { notify, showNoDataState } from '../utils/notifications.js'


export default {
  name: 'Pacientes',
  data() {
    return {
      pacientes: [],
      searchTerm: '',
      showCreateModal: false,
      showEditModal: false,
      showViewModal: false,
      currentPaciente: {
        cedula: '',
        nombre: '',
        apellido: '',
        email: '',
        telefono: '',
        fechaNacimiento: '',
        direccion: ''
      },
      viewingPaciente: {}
    }
  },
  async mounted() {
    await this.loadPacientes()
  },
  methods: {
    async loadPacientes() {
      try {
        const response = await pacienteService.getAll()
        this.pacientes = response.data
      } catch (error) {
        console.error('Error cargando pacientes:', error)
        notify.error('Error al cargar los pacientes')
      }
    },
    async searchPacientes() {
      if (this.searchTerm.trim() === '') {
        await this.loadPacientes()
      } else {
        try {
          const response = await pacienteService.search(this.searchTerm)
          this.pacientes = response.data
        } catch (error) {
          console.error('Error en la búsqueda:', error)
        }
      }
    },
    async viewPaciente(paciente) {
      try {
        const response = await pacienteService.getById(paciente.id)
        this.viewingPaciente = response.data
        this.showViewModal = true
      } catch (error) {
        console.error('Error al cargar detalles del paciente:', error)
        notify.error(error.userMessage || 'Error al cargar los detalles del paciente')
      }
    },
    editPaciente(paciente) {
      this.currentPaciente = { ...paciente }
      this.showEditModal = true
    },
    async deletePaciente(paciente) {
      if (confirm(`¿Está seguro de eliminar al paciente ${paciente.nombre} ${paciente.apellido}?`)) {
        try {
          await pacienteService.delete(paciente.id)
          await this.loadPacientes()
          notify.success('Paciente eliminado correctamente')
        } catch (error) {
          console.error('Error al eliminar paciente:', error)
          notify.error(error.userMessage || 'Error al eliminar el paciente')
        }
      }
    },
    async savePaciente() {
      try {
        if (this.showCreateModal) {
          await pacienteService.create(this.currentPaciente)
          notify.success('Paciente creado correctamente')
        } else {
          await pacienteService.update(this.currentPaciente.id, this.currentPaciente)
          notify.success('Paciente actualizado correctamente')
        }
        await this.loadPacientes()
        this.closeModals()
      } catch (error) {
        console.error('Error al guardar paciente:', error)
        notify.error(error.userMessage || 'Error al guardar el paciente')
      }
    },
    closeModals() {
      this.showCreateModal = false
      this.showEditModal = false
      this.showViewModal = false
      this.currentPaciente = {
        cedula: '',
        nombre: '',
        apellido: '',
        email: '',
        telefono: '',
        fechaNacimiento: '',
        direccion: ''
      }
      this.viewingPaciente = {}
    },
    formatDate(dateString) {
      if (!dateString) return ''
      return new Date(dateString).toLocaleDateString('es-ES')
    },
    formatDateTime(dateTimeString) {
      if (!dateTimeString) return ''
      return new Date(dateTimeString).toLocaleString('es-ES')
    }
  }
}
</script>

<style scoped>
.pacientes-container {
  padding: 20px;
}

.header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 20px;
}

.filters {
  margin-bottom: 20px;
}

.search-box {
  position: relative;
  max-width: 400px;
}

.search-box input {
  padding-right: 40px;
}

.search-box i {
  position: absolute;
  right: 12px;
  top: 50%;
  transform: translateY(-50%);
  color: #666;
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

.patient-details {
  margin-bottom: 20px;
}

.detail-row {
  padding: 8px 0;
  border-bottom: 1px solid #f0f0f0;
}

.appointments-section {
  margin-top: 20px;
}

.appointments-list {
  margin-top: 10px;
}

.appointment-item {
  padding: 10px;
  border: 1px solid #eee;
  border-radius: 4px;
  margin-bottom: 8px;
}

.appointment-info {
  display: flex;
  gap: 15px;
  align-items: center;
}

.specialty {
  background-color: #e3f2fd;
  color: #1976d2;
  padding: 2px 8px;
  border-radius: 12px;
  font-size: 12px;
}

.doctor {
  color: #666;
  font-size: 14px;
}

.status {
  padding: 2px 8px;
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
</style>