<template>
  <div class="reservar-cita-container">
    <div class="header">
      <h2>Reservar Cita Médica</h2>
      <p class="subtitle">Seleccione la especialidad, médico y horario de su preferencia</p>
    </div>

    <div class="reservation-form">
      <!-- Paso 1: Seleccionar Especialidad -->
      <div class="step" :class="{ active: currentStep >= 1, completed: currentStep > 1 }">
        <div class="step-header">
          <div class="step-number">1</div>
          <h3>Seleccionar Especialidad</h3>
          <div v-if="loadingEspecialidades" class="loading-spinner">
            <i class="fas fa-spinner fa-spin"></i>
          </div>
        </div>
        <div v-if="currentStep >= 1" class="step-content">
          <div v-if="especialidades.length === 0 && !loadingEspecialidades" class="no-data">
            <i class="fas fa-exclamation-triangle"></i>
            <p>No hay especialidades disponibles en este momento</p>
            <button @click="loadEspecialidades" class="btn btn-outline-primary">
              <i class="fas fa-refresh"></i> Reintentar
            </button>
          </div>
          <div v-else class="specialties-grid">
            <div 
              v-for="especialidad in especialidades" 
              :key="especialidad.id"
              @click="selectEspecialidad(especialidad)"
              :class="['specialty-card', { 
                selected: selectedEspecialidad?.id === especialidad.id,
                loading: loadingMedicos && selectedEspecialidad?.id === especialidad.id
              }]"
            >
              <div class="specialty-icon">
                <i v-if="loadingMedicos && selectedEspecialidad?.id === especialidad.id" 
                   class="fas fa-spinner fa-spin"></i>
                <i v-else class="fas fa-stethoscope"></i>
              </div>
              <h4>{{ especialidad.nombre }}</h4>
              <p>{{ especialidad.descripcion }}</p>
              <div v-if="especialidad.medicos" class="medicos-count">
                {{ especialidad.medicos.length }} médico(s) disponible(s)
              </div>
            </div>
          </div>
        </div>
      </div>

      <!-- Paso 2: Seleccionar Médico -->
      <div class="step" :class="{ active: currentStep >= 2, completed: currentStep > 2 }">
        <div class="step-header">
          <div class="step-number">2</div>
          <h3>Seleccionar Médico</h3>
          <div v-if="loadingMedicos" class="loading-spinner">
            <i class="fas fa-spinner fa-spin"></i>
          </div>
        </div>
        <div v-if="currentStep >= 2" class="step-content">
          <div v-if="medicosDisponibles.length === 0 && !loadingMedicos" class="no-data">
            <i class="fas fa-user-md"></i>
            <p>No hay médicos disponibles para esta especialidad</p>
          </div>
          <div v-else class="doctors-grid">
            <div 
              v-for="medico in medicosDisponibles" 
              :key="medico.id"
              @click="selectMedico(medico)"
              :class="['doctor-card', { selected: selectedMedico?.id === medico.id }]"
            >
              <div class="doctor-avatar">
                <i class="fas fa-user-md"></i>
              </div>
              <div class="doctor-info">
                <h4>Dr. {{ medico.nombre }} {{ medico.apellido }}</h4>
                <p class="license">Licencia: {{ medico.numeroLicencia }}</p>
                <p class="email">{{ medico.email }}</p>
                <div class="rating">
                  <i v-for="star in 5" :key="star" class="fas fa-star"></i>
                  <span>Excelente</span>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>

      <!-- Paso 3: Seleccionar Fecha -->
      <div class="step" :class="{ active: currentStep >= 3, completed: currentStep > 3 }">
        <div class="step-header">
          <div class="step-number">3</div>
          <h3>Seleccionar Fecha</h3>
        </div>
        <div v-if="currentStep >= 3" class="step-content">
          <div class="date-selector">
            <input 
              v-model="selectedDate" 
              @change="loadHorarios"
              :min="minDate"
              type="date" 
              class="form-control date-input"
              placeholder="Seleccione una fecha"
            />
          </div>
          <div v-if="selectedDate && selectedMedico" class="date-info">
            <p><i class="fas fa-calendar"></i> Fecha seleccionada: {{ formatDate(selectedDate) }}</p>
          </div>
        </div>
      </div>

      <!-- Paso 4: Seleccionar Horario -->
      <div class="step" :class="{ active: currentStep >= 4, completed: currentStep > 4 }">
        <div class="step-header">
          <div class="step-number">4</div>
          <h3>Seleccionar Horario</h3>
          <div v-if="loadingHorarios" class="loading-spinner">
            <i class="fas fa-spinner fa-spin"></i>
          </div>
        </div>
        <div v-if="currentStep >= 4" class="step-content">
          <div v-if="horariosDisponibles.length === 0 && !loadingHorarios && selectedDate" class="no-data">
            <i class="fas fa-clock"></i>
            <p>No hay horarios disponibles para esta fecha</p>
            <button @click="loadHorarios" class="btn btn-outline-primary">
              <i class="fas fa-refresh"></i> Buscar otros horarios
            </button>
          </div>
          <div v-else-if="!selectedDate" class="no-data">
            <i class="fas fa-calendar-alt"></i>
            <p>Primero seleccione una fecha</p>
          </div>
          <div v-else class="schedules-grid">
            <div 
              v-for="horario in horariosDisponibles" 
              :key="horario.id"
              @click="selectHorario(horario)"
              :class="['schedule-card', { selected: selectedHorario?.id === horario.id }]"
            >
              <div class="schedule-time">
                <i class="fas fa-clock"></i>
                <span>{{ formatTime(horario.horaInicio) }} - {{ formatTime(horario.horaFin) }}</span>
              </div>
              <div class="schedule-status">
                <i class="fas fa-check-circle"></i>
                <span>Disponible</span>
              </div>
            </div>
          </div>
        </div>
      </div>

      <!-- Paso 5: Datos del Paciente -->
      <div class="step" :class="{ active: currentStep >= 5 }">
        <div class="step-header">
          <div class="step-number">5</div>
          <h3>Datos del Paciente</h3>
        </div>
        <div v-if="currentStep >= 5" class="step-content">
          <div class="patient-form">
            <div class="form-row">
              <div class="form-group">
                <label>Cédula *</label>
                <input 
                  v-model="pacienteData.cedula" 
                  @blur="buscarPacientePorCedula"
                  @input="validateCedula"
                  type="text" 
                  class="form-control" 
                  :class="{ 'is-invalid': errors.cedula, 'is-valid': pacienteData.cedula && !errors.cedula }"
                  required 
                  placeholder="Ingrese su número de cédula"
                />
                <div v-if="errors.cedula" class="invalid-feedback">{{ errors.cedula }}</div>
                <div v-if="pacienteExistente" class="valid-feedback">
                  <i class="fas fa-check"></i> Paciente encontrado
                </div>
              </div>
              <div class="form-group">
                <label>Nombre *</label>
                <input 
                  v-model="pacienteData.nombre" 
                  @input="validateNombre"
                  type="text" 
                  class="form-control" 
                  :class="{ 'is-invalid': errors.nombre, 'is-valid': pacienteData.nombre && !errors.nombre }"
                  required 
                  placeholder="Ingrese su nombre"
                />
                <div v-if="errors.nombre" class="invalid-feedback">{{ errors.nombre }}</div>
              </div>
            </div>
            <div class="form-row">
              <div class="form-group">
                <label>Apellido *</label>
                <input 
                  v-model="pacienteData.apellido" 
                  @input="validateApellido"
                  type="text" 
                  class="form-control" 
                  :class="{ 'is-invalid': errors.apellido, 'is-valid': pacienteData.apellido && !errors.apellido }"
                  required 
                  placeholder="Ingrese su apellido"
                />
                <div v-if="errors.apellido" class="invalid-feedback">{{ errors.apellido }}</div>
              </div>
              <div class="form-group">
                <label>Email *</label>
                <input 
                  v-model="pacienteData.email" 
                  @input="validateEmail"
                  type="email" 
                  class="form-control" 
                  :class="{ 'is-invalid': errors.email, 'is-valid': pacienteData.email && !errors.email }"
                  required 
                  placeholder="ejemplo@correo.com"
                />
                <div v-if="errors.email" class="invalid-feedback">{{ errors.email }}</div>
              </div>
            </div>
            <div class="form-row">
              <div class="form-group">
                <label>Teléfono</label>
                <input 
                  v-model="pacienteData.telefono" 
                  @input="validateTelefono"
                  type="text" 
                  class="form-control" 
                  :class="{ 'is-invalid': errors.telefono, 'is-valid': pacienteData.telefono && !errors.telefono }"
                  placeholder="Ej: +507 6000-0000"
                />
                <div v-if="errors.telefono" class="invalid-feedback">{{ errors.telefono }}</div>
              </div>
              <div class="form-group">
                <label>Fecha de Nacimiento *</label>
                <input 
                  v-model="pacienteData.fechaNacimiento" 
                  @input="validateFechaNacimiento"
                  type="date" 
                  class="form-control" 
                  :class="{ 'is-invalid': errors.fechaNacimiento, 'is-valid': pacienteData.fechaNacimiento && !errors.fechaNacimiento }"
                  :max="maxBirthDate"
                  required 
                />
                <div v-if="errors.fechaNacimiento" class="invalid-feedback">{{ errors.fechaNacimiento }}</div>
              </div>
            </div>
            <div class="form-group">
              <label>Motivo de la Consulta</label>
              <textarea 
                v-model="motivoConsulta" 
                class="form-control" 
                rows="3" 
                placeholder="Describa brevemente el motivo de su consulta..."
                maxlength="500"
              ></textarea>
              <small class="form-text text-muted">{{ motivoConsulta.length }}/500 caracteres</small>
            </div>
          </div>
        </div>
      </div>

      <!-- Resumen de la Reserva -->
      <div v-if="currentStep >= 5" class="reservation-summary">
        <h3><i class="fas fa-clipboard-list"></i> Resumen de la Reserva</h3>
        <div class="summary-content">
          <div class="summary-item">
            <i class="fas fa-stethoscope"></i>
            <strong>Especialidad:</strong> {{ selectedEspecialidad?.nombre }}
          </div>
          <div class="summary-item">
            <i class="fas fa-user-md"></i>
            <strong>Médico:</strong> Dr. {{ selectedMedico?.nombre }} {{ selectedMedico?.apellido }}
          </div>
          <div class="summary-item">
            <i class="fas fa-calendar"></i>
            <strong>Fecha:</strong> {{ formatDate(selectedDate) }}
          </div>
          <div class="summary-item">
            <i class="fas fa-clock"></i>
            <strong>Horario:</strong> {{ formatTime(selectedHorario?.horaInicio) }} - {{ formatTime(selectedHorario?.horaFin) }}
          </div>
          <div class="summary-item">
            <i class="fas fa-user"></i>
            <strong>Paciente:</strong> {{ pacienteData.nombre }} {{ pacienteData.apellido }}
          </div>
          <div v-if="motivoConsulta" class="summary-item">
            <i class="fas fa-notes-medical"></i>
            <strong>Motivo:</strong> {{ motivoConsulta }}
          </div>
        </div>
      </div>

      <!-- Botones de Navegación -->
      <div class="navigation-buttons">
        <button 
          v-if="currentStep > 1" 
          @click="previousStep" 
          class="btn btn-secondary"
          :disabled="processingReservation"
        >
          <i class="fas fa-arrow-left"></i> Anterior
        </button>
        
        <button 
          v-if="currentStep < 5" 
          @click="nextStep" 
          :disabled="!canProceedToNextStep"
          class="btn btn-primary"
        >
          Siguiente <i class="fas fa-arrow-right"></i>
        </button>
        
        <button 
          v-if="currentStep === 5" 
          @click="confirmarReserva" 
          :disabled="!canConfirmReservation || processingReservation"
          class="btn btn-success"
        >
          <i v-if="processingReservation" class="fas fa-spinner fa-spin"></i>
          <i v-else class="fas fa-check"></i> 
          {{ processingReservation ? 'Procesando...' : 'Confirmar Reserva' }}
        </button>
      </div>
    </div>

    <!-- Success Modal -->
    <div v-if="showSuccessModal" class="modal-overlay" @click="closeSuccessModal">
      <div class="modal success-modal" @click.stop>
        <div class="modal-header success">
          <i class="fas fa-check-circle"></i>
          <h3>¡Cita Reservada Exitosamente!</h3>
        </div>
        <div class="modal-body">
          <p>Su cita ha sido programada correctamente.</p>
          <div class="appointment-details">
            <p><strong>Fecha:</strong> {{ formatDate(selectedDate) }}</p>
            <p><strong>Hora:</strong> {{ formatTime(selectedHorario?.horaInicio) }}</p>
            <p><strong>Médico:</strong> Dr. {{ selectedMedico?.nombre }} {{ selectedMedico?.apellido }}</p>
          </div>
          <p class="reminder">Recuerde llegar 15 minutos antes de su cita.</p>
        </div>
        <div class="modal-footer">
          <button @click="closeSuccessModal" class="btn btn-primary">Entendido</button>
          <button @click="newReservation" class="btn btn-outline-primary">Nueva Cita</button>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { especialidadService, medicoService, horarioService, pacienteService, citaService } from '../services/api.js'
import { useErrorHandler } from '../composables/useErrorHandler.js'

export default {
  name: 'ReservarCita',
  setup() {
    const { addError, addSuccess } = useErrorHandler()
    return { addError, addSuccess }
  },
  data() {
    return {
      currentStep: 1,
      especialidades: [],
      medicosDisponibles: [],
      horariosDisponibles: [],
      selectedEspecialidad: null,
      selectedMedico: null,
      selectedDate: '',
      selectedHorario: null,
      pacienteData: {
        cedula: '',
        nombre: '',
        apellido: '',
        email: '',
        telefono: '',
        fechaNacimiento: ''
      },
      motivoConsulta: '',
      pacienteExistente: null,
      loadingEspecialidades: false,
      loadingMedicos: false,
      loadingHorarios: false,
      processingReservation: false,
      showSuccessModal: false,
      errors: {}
    }
  },
  computed: {
    minDate() {
      const today = new Date()
      return today.toISOString().split('T')[0]
    },
    maxBirthDate() {
      const today = new Date()
      return today.toISOString().split('T')[0]
    },
    canProceedToNextStep() {
      switch (this.currentStep) {
        case 1:
          return this.selectedEspecialidad !== null && !this.loadingMedicos
        case 2:
          return this.selectedMedico !== null
        case 3:
          return this.selectedDate !== ''
        case 4:
          return this.selectedHorario !== null
        default:
          return false
      }
    },
    canConfirmReservation() {
      return this.selectedEspecialidad && 
             this.selectedMedico && 
             this.selectedDate && 
             this.selectedHorario &&
             this.pacienteData.cedula &&
             this.pacienteData.nombre &&
             this.pacienteData.apellido &&
             this.pacienteData.email &&
             this.pacienteData.fechaNacimiento &&
             Object.keys(this.errors).length === 0
    }
  },
  async mounted() {
    await this.loadEspecialidades()
  },
  methods: {
    async loadEspecialidades() {
      this.loadingEspecialidades = true
      try {
        const response = await especialidadService.getConMedicos()
        this.especialidades = response.data.filter(e => e.medicos && e.medicos.length > 0)
        if (this.especialidades.length === 0) {
          this.addError('No hay especialidades disponibles en este momento')
        }
      } catch (error) {
        console.error('Error cargando especialidades:', error)
        this.addError('Error al cargar las especialidades. Por favor, intente nuevamente.')
      } finally {
        this.loadingEspecialidades = false
      }
    },
    async selectEspecialidad(especialidad) {
      if (this.loadingMedicos) return
      
      this.selectedEspecialidad = especialidad
      this.loadingMedicos = true
      
      try {
        const response = await medicoService.getByEspecialidad(especialidad.id)
        this.medicosDisponibles = response.data
        
        if (this.medicosDisponibles.length === 0) {
          this.addError('No hay médicos disponibles para esta especialidad')
        } else {
          // Auto-advance to next step after successful load
          setTimeout(() => {
            if (this.currentStep === 1) {
              this.nextStep()
            }
          }, 500)
        }
      } catch (error) {
        console.error('Error cargando médicos:', error)
        this.addError('Error al cargar los médicos. Por favor, intente nuevamente.')
        this.medicosDisponibles = []
      } finally {
        this.loadingMedicos = false
      }
      
      // Reset subsequent selections
      this.selectedMedico = null
      this.selectedDate = ''
      this.selectedHorario = null
      this.horariosDisponibles = []
    },
    selectMedico(medico) {
      this.selectedMedico = medico
      this.selectedDate = ''
      this.selectedHorario = null
      this.horariosDisponibles = []
      
      // Auto-advance to next step
      setTimeout(() => {
        if (this.currentStep === 2) {
          this.nextStep()
        }
      }, 300)
    },
    async loadHorarios() {
      if (this.selectedMedico && this.selectedDate) {
        this.loadingHorarios = true
        try {
          const response = await horarioService.getDisponibles(this.selectedMedico.id, this.selectedDate)
          this.horariosDisponibles = response.data.filter(h => h.disponible)
          
          if (this.horariosDisponibles.length === 0) {
            this.addError('No hay horarios disponibles para esta fecha. Intente con otra fecha.')
          } else {
            // Auto-advance to next step
            setTimeout(() => {
              if (this.currentStep === 3) {
                this.nextStep()
              }
            }, 300)
          }
        } catch (error) {
          console.error('Error cargando horarios:', error)
          this.addError('Error al cargar los horarios disponibles')
          this.horariosDisponibles = []
        } finally {
          this.loadingHorarios = false
        }
      }
      this.selectedHorario = null
    },
    selectHorario(horario) {
      this.selectedHorario = horario
      
      // Auto-advance to next step
      setTimeout(() => {
        if (this.currentStep === 4) {
          this.nextStep()
        }
      }, 300)
    },
    async buscarPacientePorCedula() {
      if (this.pacienteData.cedula && this.pacienteData.cedula.length >= 8) {
        try {
          const response = await pacienteService.getByCedula(this.pacienteData.cedula)
          if (response.data) {
            this.pacienteExistente = response.data
            this.pacienteData = {
              ...response.data,
              fechaNacimiento: this.formatDateForInput(response.data.fechaNacimiento)
            }
            this.addSuccess('Paciente encontrado en el sistema')
          }
        } catch (error) {
          // Paciente no encontrado, mantener formulario vacío
          this.pacienteExistente = null
        }
      }
    },
    nextStep() {
      if (this.canProceedToNextStep && this.currentStep < 5) {
        this.currentStep++
      }
    },
    previousStep() {
      if (this.currentStep > 1) {
        this.currentStep--
      }
    },
    async confirmarReserva() {
      if (!this.canConfirmReservation) return
      
      this.processingReservation = true
      try {
        // Crear o actualizar paciente si es necesario
        let pacienteId = this.pacienteExistente?.id
        
        if (!pacienteId) {
          const pacienteResponse = await pacienteService.create(this.pacienteData)
          pacienteId = pacienteResponse.data.id
        }
        
        // Crear la cita
        const citaData = {
          pacienteId: pacienteId,
          medicoId: this.selectedMedico.id,
          especialidadId: this.selectedEspecialidad.id,
          horarioDisponibleId: this.selectedHorario.id,
          fechaHora: `${this.selectedDate}T${this.selectedHorario.horaInicio}`,
          motivo: this.motivoConsulta,
          estado: 'Pendiente'
        }
        
        await citaService.create(citaData)
        
        this.showSuccessModal = true
        this.addSuccess('¡Cita reservada exitosamente!')
        
      } catch (error) {
        console.error('Error al confirmar reserva:', error)
        this.addError('Error al confirmar la reserva. Por favor, verifique los datos e intente nuevamente.')
      } finally {
        this.processingReservation = false
      }
    },
    closeSuccessModal() {
      this.showSuccessModal = false
      this.resetForm()
    },
    newReservation() {
      this.showSuccessModal = false
      this.resetForm()
    },
    resetForm() {
      this.currentStep = 1
      this.selectedEspecialidad = null
      this.selectedMedico = null
      this.selectedDate = ''
      this.selectedHorario = null
      this.medicosDisponibles = []
      this.horariosDisponibles = []
      this.pacienteData = {
        cedula: '',
        nombre: '',
        apellido: '',
        email: '',
        telefono: '',
        fechaNacimiento: ''
      }
      this.motivoConsulta = ''
      this.pacienteExistente = null
      this.errors = {}
    },
    // Validation methods
    validateCedula() {
      if (!this.pacienteData.cedula) {
        this.$set(this.errors, 'cedula', 'La cédula es requerida')
      } else if (this.pacienteData.cedula.length < 8) {
        this.$set(this.errors, 'cedula', 'La cédula debe tener al menos 8 caracteres')
      } else {
        this.$delete(this.errors, 'cedula')
      }
    },
    validateNombre() {
      if (!this.pacienteData.nombre) {
        this.$set(this.errors, 'nombre', 'El nombre es requerido')
      } else if (this.pacienteData.nombre.length < 2) {
        this.$set(this.errors, 'nombre', 'El nombre debe tener al menos 2 caracteres')
      } else {
        this.$delete(this.errors, 'nombre')
      }
    },
    validateApellido() {
      if (!this.pacienteData.apellido) {
        this.$set(this.errors, 'apellido', 'El apellido es requerido')
      } else if (this.pacienteData.apellido.length < 2) {
        this.$set(this.errors, 'apellido', 'El apellido debe tener al menos 2 caracteres')
      } else {
        this.$delete(this.errors, 'apellido')
      }
    },
    validateEmail() {
      const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/
      if (!this.pacienteData.email) {
        this.$set(this.errors, 'email', 'El email es requerido')
      } else if (!emailRegex.test(this.pacienteData.email)) {
        this.$set(this.errors, 'email', 'Ingrese un email válido')
      } else {
        this.$delete(this.errors, 'email')
      }
    },
    validateTelefono() {
      if (this.pacienteData.telefono && this.pacienteData.telefono.length < 8) {
        this.$set(this.errors, 'telefono', 'El teléfono debe tener al menos 8 dígitos')
      } else {
        this.$delete(this.errors, 'telefono')
      }
    },
    validateFechaNacimiento() {
      if (!this.pacienteData.fechaNacimiento) {
        this.$set(this.errors, 'fechaNacimiento', 'La fecha de nacimiento es requerida')
      } else {
        const birthDate = new Date(this.pacienteData.fechaNacimiento)
        const today = new Date()
        const age = today.getFullYear() - birthDate.getFullYear()
        
        if (age < 0 || age > 120) {
          this.$set(this.errors, 'fechaNacimiento', 'Ingrese una fecha de nacimiento válida')
        } else {
          this.$delete(this.errors, 'fechaNacimiento')
        }
      }
    },
    formatDate(dateString) {
      if (!dateString) return ''
      return new Date(dateString).toLocaleDateString('es-ES', {
        weekday: 'long',
        year: 'numeric',
        month: 'long',
        day: 'numeric'
      })
    },
    formatTime(timeString) {
      if (!timeString) return ''
      return timeString.substring(0, 5) // HH:MM
    },
    formatDateForInput(dateString) {
      if (!dateString) return ''
      return new Date(dateString).toISOString().split('T')[0]
    }
  }
}
</script>

<style scoped>
.reservar-cita-container {
  padding: 20px;
  max-width: 1000px;
  margin: 0 auto;
}

.header {
  text-align: center;
  margin-bottom: 30px;
}

.header h2 {
  color: #333;
  margin-bottom: 10px;
}

.subtitle {
  color: #666;
  font-size: 16px;
}

/* Progress Bar */
.progress-container {
  margin-bottom: 30px;
}

.progress-bar {
  width: 100%;
  height: 8px;
  background-color: #e9ecef;
  border-radius: 4px;
  overflow: hidden;
  margin-bottom: 15px;
}

.progress-fill {
  height: 100%;
  background: linear-gradient(90deg, #007bff, #28a745);
  transition: width 0.5s ease;
}

.progress-steps {
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.progress-step {
  width: 30px;
  height: 30px;
  border-radius: 50%;
  background-color: #e9ecef;
  color: #6c757d;
  display: flex;
  align-items: center;
  justify-content: center;
  font-weight: bold;
  font-size: 14px;
  transition: all 0.3s ease;
}

.progress-step.active {
  background-color: #007bff;
  color: white;
}

.progress-step.completed {
  background-color: #28a745;
  color: white;
}

.reservation-form {
  background: white;
  border-radius: 12px;
  padding: 30px;
  box-shadow: 0 4px 6px rgba(0,0,0,0.1);
}

.step {
  margin-bottom: 30px;
  opacity: 0.5;
  transition: opacity 0.3s ease;
}

.step.active {
  opacity: 1;
}

.step.completed {
  opacity: 0.8;
}

.step-header {
  display: flex;
  align-items: center;
  margin-bottom: 20px;
  position: relative;
}

.step-number {
  width: 40px;
  height: 40px;
  border-radius: 50%;
  background-color: #007bff;
  color: white;
  display: flex;
  align-items: center;
  justify-content: center;
  font-weight: bold;
  margin-right: 15px;
}

.step.completed .step-number {
  background-color: #28a745;
}

.loading-spinner {
  margin-left: auto;
  color: #007bff;
  font-size: 18px;
}

.step-content {
  margin-left: 55px;
}

.specialties-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(280px, 1fr));
  gap: 20px;
}

.specialty-card {
  border: 2px solid #e9ecef;
  border-radius: 12px;
  padding: 25px;
  text-align: center;
  cursor: pointer;
  transition: all 0.3s ease;
  position: relative;
  overflow: hidden;
}

.specialty-card:hover {
  border-color: #007bff;
  transform: translateY(-3px);
  box-shadow: 0 8px 25px rgba(0,123,255,0.15);
}

.specialty-card.selected {
  border-color: #007bff;
  background-color: #f8f9ff;
  box-shadow: 0 4px 15px rgba(0,123,255,0.2);
}

.specialty-card.loading {
  pointer-events: none;
  opacity: 0.7;
}

.specialty-icon {
  font-size: 48px;
  color: #007bff;
  margin-bottom: 15px;
  transition: transform 0.3s ease;
}

.specialty-card:hover .specialty-icon {
  transform: scale(1.1);
}

.specialty-card h4 {
  margin-bottom: 10px;
  color: #333;
  font-size: 18px;
}

.specialty-card p {
  color: #666;
  font-size: 14px;
  line-height: 1.4;
}

.medicos-count {
  margin-top: 10px;
  padding: 5px 10px;
  background-color: #e3f2fd;
  color: #1976d2;
  border-radius: 15px;
  font-size: 12px;
  font-weight: 500;
}

.doctors-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(320px, 1fr));
  gap: 20px;
}

.doctor-card {
  border: 2px solid #e9ecef;
  border-radius: 12px;
  padding: 20px;
  display: flex;
  align-items: center;
  cursor: pointer;
  transition: all 0.3s ease;
}

.doctor-card:hover {
  border-color: #007bff;
  transform: translateY(-2px);
  box-shadow: 0 6px 20px rgba(0,123,255,0.15);
}

.doctor-card.selected {
  border-color: #007bff;
  background-color: #f8f9ff;
  box-shadow: 0 4px 15px rgba(0,123,255,0.2);
}

.doctor-avatar {
  width: 70px;
  height: 70px;
  border-radius: 50%;
  background: linear-gradient(135deg, #007bff, #0056b3);
  color: white;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 28px;
  margin-right: 20px;
  flex-shrink: 0;
}

.doctor-info {
  flex: 1;
}

.doctor-info h4 {
  margin-bottom: 8px;
  color: #333;
  font-size: 18px;
}

.license, .email {
  font-size: 14px;
  color: #666;
  margin-bottom: 5px;
}

.rating {
  display: flex;
  align-items: center;
  gap: 5px;
  margin-top: 8px;
}

.rating i {
  color: #ffc107;
  font-size: 14px;
}

.rating span {
  font-size: 12px;
  color: #28a745;
  font-weight: 500;
}

.date-selector {
  display: flex;
  justify-content: center;
  margin-bottom: 20px;
}

.date-input {
  max-width: 250px;
  padding: 15px;
  font-size: 16px;
  border: 2px solid #ddd;
  border-radius: 8px;
  transition: border-color 0.3s ease;
}

.date-input:focus {
  border-color: #007bff;
  outline: none;
  box-shadow: 0 0 0 3px rgba(0,123,255,0.1);
}

.date-info {
  text-align: center;
  margin-top: 15px;
  padding: 10px;
  background-color: #e8f5e8;
  border-radius: 8px;
  color: #2e7d32;
}

.date-info i {
  margin-right: 8px;
}

.schedules-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(220px, 1fr));
  gap: 15px;
}

.schedule-card {
  border: 2px solid #e9ecef;
  border-radius: 12px;
  padding: 20px;
  text-align: center;
  cursor: pointer;
  transition: all 0.3s ease;
}

.schedule-card:hover {
  border-color: #007bff;
  transform: translateY(-2px);
  box-shadow: 0 6px 20px rgba(0,123,255,0.15);
}

.schedule-card.selected {
  border-color: #007bff;
  background-color: #f8f9ff;
  box-shadow: 0 4px 15px rgba(0,123,255,0.2);
}

.schedule-time {
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 10px;
  font-size: 16px;
  font-weight: 600;
  color: #333;
  margin-bottom: 10px;
}

.schedule-status {
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 5px;
  font-size: 14px;
  color: #28a745;
}

.patient-form {
  max-width: 700px;
}

.form-row {
  display: flex;
  gap: 20px;
  margin-bottom: 20px;
}

.form-group {
  flex: 1;
}

.form-group label {
  display: block;
  margin-bottom: 8px;
  font-weight: 600;
  color: #333;
  font-size: 14px;
}

.form-control {
  width: 100%;
  padding: 12px 15px;
  border: 2px solid #e9ecef;
  border-radius: 8px;
  font-size: 14px;
  transition: all 0.3s ease;
}

.form-control:focus {
  border-color: #007bff;
  outline: none;
  box-shadow: 0 0 0 3px rgba(0,123,255,0.1);
}

.form-control.is-valid {
  border-color: #28a745;
}

.form-control.is-invalid {
  border-color: #dc3545;
}

.valid-feedback {
  display: block;
  width: 100%;
  margin-top: 5px;
  font-size: 12px;
  color: #28a745;
}

.invalid-feedback {
  display: block;
  width: 100%;
  margin-top: 5px;
  font-size: 12px;
  color: #dc3545;
}

.form-text {
  font-size: 12px;
  color: #6c757d;
  margin-top: 5px;
}

.reservation-summary {
  background: linear-gradient(135deg, #f8f9fa, #e9ecef);
  border-radius: 12px;
  padding: 25px;
  margin: 30px 0;
  border: 1px solid #dee2e6;
}

.reservation-summary h3 {
  margin-bottom: 20px;
  color: #333;
  display: flex;
  align-items: center;
  gap: 10px;
}

.summary-content {
  display: grid;
  gap: 15px;
}

.summary-item {
  display: flex;
  align-items: center;
  gap: 15px;
  padding: 12px 0;
  border-bottom: 1px solid #dee2e6;
}

.summary-item:last-child {
  border-bottom: none;
}

.summary-item i {
  color: #007bff;
  width: 20px;
}

.navigation-buttons {
  display: flex;
  justify-content: space-between;
  margin-top: 40px;
  padding-top: 25px;
  border-top: 2px solid #dee2e6;
}

.btn {
  padding: 15px 30px;
  border: none;
  border-radius: 8px;
  cursor: pointer;
  font-size: 16px;
  font-weight: 600;
  display: flex;
  align-items: center;
  gap: 10px;
  transition: all 0.3s ease;
  text-decoration: none;
}

.btn:disabled {
  opacity: 0.5;
  cursor: not-allowed;
}

.btn-primary {
  background: linear-gradient(135deg, #007bff, #0056b3);
  color: white;
  box-shadow: 0 4px 15px rgba(0,123,255,0.3);
}

.btn-primary:hover:not(:disabled) {
  transform: translateY(-2px);
  box-shadow: 0 6px 20px rgba(0,123,255,0.4);
}

.btn-outline-primary {
  background: transparent;
  color: #007bff;
  border: 2px solid #007bff;
}

.btn-outline-primary:hover {
  background-color: #007bff;
  color: white;
}

.btn-secondary {
  background: linear-gradient(135deg, #6c757d, #545b62);
  color: white;
}

.btn-secondary:hover {
  transform: translateY(-2px);
  box-shadow: 0 6px 20px rgba(108,117,125,0.3);
}

.btn-success {
  background: linear-gradient(135deg, #28a745, #1e7e34);
  color: white;
  box-shadow: 0 4px 15px rgba(40,167,69,0.3);
}

.btn-success:hover:not(:disabled) {
  transform: translateY(-2px);
  box-shadow: 0 6px 20px rgba(40,167,69,0.4);
}

.no-data {
  text-align: center;
  color: #666;
  padding: 60px 20px;
  background-color: #f8f9fa;
  border-radius: 12px;
  border: 2px dashed #dee2e6;
}

.no-data i {
  font-size: 48px;
  color: #adb5bd;
  margin-bottom: 15px;
  display: block;
}

.no-data p {
  font-size: 16px;
  margin-bottom: 20px;
}

/* Success Modal */
.modal-overlay {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background-color: rgba(0, 0, 0, 0.5);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 1000;
  animation: fadeIn 0.3s ease;
}

.modal {
  background: white;
  border-radius: 15px;
  max-width: 500px;
  width: 90%;
  max-height: 90vh;
  overflow-y: auto;
  animation: slideIn 0.3s ease;
}

.modal-header {
  padding: 25px 30px 20px;
  text-align: center;
  border-bottom: 1px solid #dee2e6;
}

.modal-header.success {
  background: linear-gradient(135deg, #d4edda, #c3e6cb);
  color: #155724;
  border-radius: 15px 15px 0 0;
}

.modal-header i {
  font-size: 48px;
  margin-bottom: 15px;
  display: block;
}

.modal-header h3 {
  margin: 0;
  font-size: 24px;
}

.modal-body {
  padding: 25px 30px;
}

.appointment-details {
  background-color: #f8f9fa;
  border-radius: 8px;
  padding: 20px;
  margin: 20px 0;
}

.appointment-details p {
  margin-bottom: 10px;
  font-size: 16px;
}

.reminder {
  background-color: #fff3cd;
  color: #856404;
  padding: 15px;
  border-radius: 8px;
  margin-top: 20px;
  font-weight: 500;
}

.modal-footer {
  padding: 20px 30px 25px;
  display: flex;
  gap: 15px;
  justify-content: center;
  border-top: 1px solid #dee2e6;
}

/* Animations */
@keyframes fadeIn {
  from { opacity: 0; }
  to { opacity: 1; }
}

@keyframes slideIn {
  from { 
    opacity: 0;
    transform: translateY(-50px) scale(0.9);
  }
  to { 
    opacity: 1;
    transform: translateY(0) scale(1);
  }
}

/* Responsive Design */
@media (max-width: 768px) {
  .reservar-cita-container {
    padding: 15px;
  }
  
  .reservation-form {
    padding: 20px;
  }
  
  .form-row {
    flex-direction: column;
    gap: 15px;
  }
  
  .specialties-grid,
  .doctors-grid,
  .schedules-grid {
    grid-template-columns: 1fr;
  }
  
  .step-content {
    margin-left: 0;
  }
  
  .navigation-buttons {
    flex-direction: column;
    gap: 15px;
  }
  
  .btn {
    width: 100%;
    justify-content: center;
  }
  
  .modal {
    width: 95%;
    margin: 20px;
  }
  
  .modal-footer {
    flex-direction: column;
  }
}

@media (max-width: 480px) {
  .header h2 {
    font-size: 24px;
  }
  
  .subtitle {
    font-size: 14px;
  }
  
  .progress-steps {
    gap: 10px;
  }
  
  .progress-step {
    width: 25px;
    height: 25px;
    font-size: 12px;
  }
  
  .doctor-card {
    flex-direction: column;
    text-align: center;
  }
  
  .doctor-avatar {
    margin-right: 0;
    margin-bottom: 15px;
  }
}
</style>