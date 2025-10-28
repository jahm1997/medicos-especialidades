import axios from 'axios'

// Configuración base de Axios
const API_BASE_URL = 'http://localhost:5242/api'

const api = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
})

// Interceptor para manejo de errores
api.interceptors.response.use(
  (response) => response,
  (error) => {
    // No mostrar alertas automáticas, solo loggear el error
    console.error('API Error:', error.response?.data || error.message)
    
    // Agregar información adicional al error para manejo en componentes
    if (error.response) {
      error.userMessage = getErrorMessage(error.response.status, error.response.data)
    } else {
      error.userMessage = 'Error de conexión. Verifique su conexión a internet.'
    }
    
    return Promise.reject(error)
  }
)

// Función para obtener mensajes de error amigables
function getErrorMessage(status, data) {
  switch (status) {
    case 404:
      return 'No se encontraron datos'
    case 500:
      return 'Error interno del servidor. Intente nuevamente más tarde.'
    case 400:
      return data?.message || 'Datos inválidos'
    case 401:
      return 'No autorizado'
    case 403:
      return 'Acceso denegado'
    default:
      return 'Ha ocurrido un error inesperado'
  }
}

// Servicios para Usuarios
export const usuarioService = {
  getAll: () => api.get('/usuarios'),
  getById: (id) => api.get(`/usuarios/${id}`),
  create: (usuario) => api.post('/usuarios', usuario),
  update: (id, usuario) => api.put(`/usuarios/${id}`, usuario),
  delete: (id) => api.delete(`/usuarios/${id}`)
};

// Servicios para Médicos
export const medicoService = {
  getAll: () => api.get('/medicos'),
  getById: (id) => api.get(`/medicos/${id}`),
  getByEspecialidad: (especialidadId) => api.get(`/medicos/especialidad/${especialidadId}`),
  getConHorarios: (fecha) => api.get(`/medicos/con-horarios/${fecha}`),
  search: (searchTerm) => api.get(`/medicos/search/${searchTerm}`),
  create: (medico) => api.post('/medicos', medico),
  update: (id, medico) => api.put(`/medicos/${id}`, medico),
  delete: (id) => api.delete(`/medicos/${id}`)
};

// Servicios para Pacientes
export const pacienteService = {
  getAll: () => api.get('/pacientes'),
  getById: (id) => api.get(`/pacientes/${id}`),
  getByCedula: (cedula) => api.get(`/pacientes/cedula/${cedula}`),
  search: (searchTerm) => api.get(`/pacientes/search/${searchTerm}`),
  create: (paciente) => api.post('/pacientes', paciente),
  update: (id, paciente) => api.put(`/pacientes/${id}`, paciente),
  delete: (id) => api.delete(`/pacientes/${id}`)
};

// Servicios para Especialidades
export const especialidadService = {
  getAll: () => api.get('/especialidades'),
  getById: (id) => api.get(`/especialidades/${id}`),
  getConMedicos: () => api.get('/especialidades/con-medicos'),
  create: (especialidad) => api.post('/especialidades', especialidad),
  update: (id, especialidad) => api.put(`/especialidades/${id}`, especialidad),
  delete: (id) => api.delete(`/especialidades/${id}`)
};

// Servicios para Horarios Disponibles
export const horarioService = {
  getAll: () => api.get('/horariosDisponibles'),
  getByMedico: (medicoId) => api.get(`/horariosDisponibles/medico/${medicoId}`),
  getDisponibles: (medicoId, fecha) => api.get(`/horariosDisponibles/medico/${medicoId}/fecha/${fecha}`),
  getByEspecialidad: (especialidadId, fecha) => api.get(`/horariosDisponibles/especialidad/${especialidadId}/fecha/${fecha}`),
  getById: (id) => api.get(`/horariosDisponibles/${id}`),
  create: (horario) => api.post('/horariosDisponibles', horario),
  createRecurrentes: (horarioRecurrente) => api.post('/horariosDisponibles/recurrentes', horarioRecurrente),
  update: (id, horario) => api.put(`/horariosDisponibles/${id}`, horario),
  marcarNoDisponible: (id) => api.put(`/horariosDisponibles/${id}/marcar-no-disponible`),
  delete: (id) => api.delete(`/horariosDisponibles/${id}`)
};

// Servicios para Citas
export const citaService = {
  getAll: () => api.get('/citas'),
  getById: (id) => api.get(`/citas/${id}`),
  getByPaciente: (pacienteId) => api.get(`/citas/paciente/${pacienteId}`),
  getByMedico: (medicoId) => api.get(`/citas/medico/${medicoId}`),
  getByFecha: (fecha) => api.get(`/citas/fecha/${fecha}`),
  getPendientes: () => api.get('/citas/pendientes'),
  getDelDia: () => api.get('/citas/hoy'),
  create: (cita) => api.post('/citas', cita),
  update: (id, cita) => api.put(`/citas/${id}`, cita),
  cancelar: (id, motivo) => api.put(`/citas/${id}/cancelar`, { motivo }),
  completar: (id, observaciones) => api.put(`/citas/${id}/completar`, { observaciones }),
  delete: (id) => api.delete(`/citas/${id}`)
};

export default api;