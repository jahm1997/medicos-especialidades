<template>
  <div class="especialidades-container">
    <div class="header">
      <h2>Gestión de Especialidades</h2>
      <button @click="showCreateModal = true" class="btn btn-primary">
        <i class="fas fa-plus"></i> Nueva Especialidad
      </button>
    </div>

    <!-- Tabla de especialidades -->
    <div class="table-container">
      <table class="table">
        <thead>
          <tr>
            <th>Nombre</th>
            <th>Descripción</th>
            <th>Médicos Asociados</th>
            <th>Acciones</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="especialidad in especialidades" :key="especialidad.id">
            <td>{{ especialidad.nombre }}</td>
            <td>{{ especialidad.descripcion }}</td>
            <td>
              <span class="badge">{{ especialidad.medicos?.length || 0 }} médicos</span>
            </td>
            <td>
              <button @click="viewEspecialidad(especialidad)" class="btn btn-info btn-sm">
                <i class="fas fa-eye"></i>
              </button>
              <button @click="editEspecialidad(especialidad)" class="btn btn-warning btn-sm">
                <i class="fas fa-edit"></i>
              </button>
              <button @click="deleteEspecialidad(especialidad)" class="btn btn-danger btn-sm">
                <i class="fas fa-trash"></i>
              </button>
            </td>
          </tr>
        </tbody>
      </table>
    </div>

    <!-- Modal para crear/editar especialidad -->
    <div v-if="showCreateModal || showEditModal" class="modal-overlay" @click="closeModals">
      <div class="modal-content" @click.stop>
        <div class="modal-header">
          <h3>{{ showCreateModal ? 'Nueva Especialidad' : 'Editar Especialidad' }}</h3>
          <button @click="closeModals" class="close-btn">&times;</button>
        </div>
        <form @submit.prevent="saveEspecialidad" class="modal-body">
          <div class="form-group">
            <label>Nombre *</label>
            <input v-model="currentEspecialidad.nombre" type="text" class="form-control" required />
          </div>
          <div class="form-group">
            <label>Descripción</label>
            <textarea v-model="currentEspecialidad.descripcion" class="form-control" rows="4"></textarea>
          </div>
          <div class="modal-footer">
            <button type="button" @click="closeModals" class="btn btn-secondary">Cancelar</button>
            <button type="submit" class="btn btn-primary">Guardar</button>
          </div>
        </form>
      </div>
    </div>

    <!-- Modal para ver detalles de la especialidad -->
    <div v-if="showViewModal" class="modal-overlay" @click="closeModals">
      <div class="modal-content" @click.stop>
        <div class="modal-header">
          <h3>Detalles de la Especialidad</h3>
          <button @click="closeModals" class="close-btn">&times;</button>
        </div>
        <div class="modal-body">
          <div class="specialty-details">
            <div class="detail-row">
              <strong>Nombre:</strong> {{ viewingEspecialidad.nombre }}
            </div>
            <div class="detail-row">
              <strong>Descripción:</strong> {{ viewingEspecialidad.descripcion }}
            </div>
          </div>
          
          <!-- Médicos asociados -->
          <div v-if="viewingEspecialidad.medicos && viewingEspecialidad.medicos.length > 0" class="doctors-section">
            <h4>Médicos Asociados</h4>
            <div class="doctors-list">
              <div v-for="medico in viewingEspecialidad.medicos" :key="medico.id" class="doctor-item">
                <div class="doctor-info">
                  <strong>Dr. {{ medico.nombre }} {{ medico.apellido }}</strong>
                  <span class="license">Licencia: {{ medico.numeroLicencia }}</span>
                  <span class="email">{{ medico.email }}</span>
                  <span class="phone">{{ medico.telefono }}</span>
                </div>
              </div>
            </div>
          </div>
          <div v-else class="no-doctors">
            <p>No hay médicos asociados a esta especialidad.</p>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { especialidadService } from '../services/api.js'
import { notify, showNoDataState } from '../utils/notifications.js'


export default {
  name: 'Especialidades',
  data() {
    return {
      especialidades: [],
      showCreateModal: false,
      showEditModal: false,
      showViewModal: false,
      currentEspecialidad: {
        nombre: '',
        descripcion: ''
      },
      viewingEspecialidad: {}
    }
  },
  async mounted() {
    await this.loadEspecialidades()
  },
  methods: {
    async loadEspecialidades() {
      try {
        const response = await especialidadService.getAll()
        this.especialidades = response.data
      } catch (error) {
        console.error('Error cargando especialidades:', error)
        notify.error('Error al cargar las especialidades')
      }
    },
    async viewEspecialidad(especialidad) {
      try {
        const response = await especialidadService.getById(especialidad.id)
        this.viewingEspecialidad = response.data
        this.showViewModal = true
      } catch (error) {
        console.error('Error al cargar detalles de la especialidad:', error)
        notify.error(error.userMessage || 'Error al cargar los detalles de la especialidad')
      }
    },
    editEspecialidad(especialidad) {
      this.currentEspecialidad = { ...especialidad }
      this.showEditModal = true
    },
    async deleteEspecialidad(especialidad) {
      if (confirm(`¿Está seguro de eliminar la especialidad ${especialidad.nombre}?`)) {
        try {
          await especialidadService.delete(especialidad.id)
          await this.loadEspecialidades()
          notify.success('Especialidad eliminada correctamente')
        } catch (error) {
          console.error('Error al eliminar especialidad:', error)
          notify.error(error.userMessage || 'Error al eliminar la especialidad. Puede que tenga médicos asociados.')
        }
      }
    },
    async saveEspecialidad() {
      try {
        if (this.showCreateModal) {
          await especialidadService.create(this.currentEspecialidad)
          notify.success('Especialidad creada correctamente')
        } else {
          await especialidadService.update(this.currentEspecialidad.id, this.currentEspecialidad)
          notify.success('Especialidad actualizada correctamente')
        }
        await this.loadEspecialidades()
        this.closeModals()
      } catch (error) {
        console.error('Error al guardar especialidad:', error)
        notify.error(error.userMessage || 'Error al guardar la especialidad')
      }
    },
    closeModals() {
      this.showCreateModal = false
      this.showEditModal = false
      this.showViewModal = false
      this.currentEspecialidad = {
        nombre: '',
        descripcion: ''
      }
      this.viewingEspecialidad = {}
    }
  }
}
</script>

<style scoped>
.especialidades-container {
  padding: 20px;
}

.header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 20px;
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

.badge {
  background-color: #007bff;
  color: white;
  padding: 4px 8px;
  border-radius: 12px;
  font-size: 12px;
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

.form-group {
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

.specialty-details {
  margin-bottom: 20px;
}

.detail-row {
  padding: 8px 0;
  border-bottom: 1px solid #f0f0f0;
}

.doctors-section {
  margin-top: 20px;
}

.doctors-list {
  margin-top: 10px;
}

.doctor-item {
  padding: 15px;
  border: 1px solid #eee;
  border-radius: 8px;
  margin-bottom: 10px;
  background-color: #f8f9fa;
}

.doctor-info {
  display: flex;
  flex-direction: column;
  gap: 5px;
}

.license {
  color: #666;
  font-size: 14px;
}

.email {
  color: #007bff;
  font-size: 14px;
}

.phone {
  color: #666;
  font-size: 14px;
}

.no-doctors {
  text-align: center;
  color: #666;
  font-style: italic;
  margin-top: 20px;
}
</style>