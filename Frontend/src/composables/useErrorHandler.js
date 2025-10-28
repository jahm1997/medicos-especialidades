import { ref, reactive } from 'vue'

// Estado global de errores
const globalErrors = reactive({
  errors: [],
  isLoading: false
})

export function useErrorHandler() {
  const errors = ref([])
  const isLoading = ref(false)

  // Agregar error
  const addError = (error, context = '') => {
    const errorObj = {
      id: Date.now(),
      message: getErrorMessage(error),
      context,
      timestamp: new Date(),
      type: getErrorType(error)
    }
    
    errors.value.push(errorObj)
    globalErrors.errors.push(errorObj)
    
    // Auto-remover después de 5 segundos
    setTimeout(() => {
      removeError(errorObj.id)
    }, 5000)
  }

  // Remover error
  const removeError = (errorId) => {
    errors.value = errors.value.filter(e => e.id !== errorId)
    globalErrors.errors = globalErrors.errors.filter(e => e.id !== errorId)
  }

  // Limpiar todos los errores
  const clearErrors = () => {
    errors.value = []
    globalErrors.errors = []
  }

  // Obtener mensaje de error legible
  const getErrorMessage = (error) => {
    if (typeof error === 'string') return error
    
    if (error.response) {
      // Error de respuesta HTTP
      const status = error.response.status
      const data = error.response.data
      
      switch (status) {
        case 400:
          return data.message || 'Datos inválidos enviados al servidor'
        case 401:
          return 'No autorizado. Por favor, inicia sesión nuevamente'
        case 403:
          return 'No tienes permisos para realizar esta acción'
        case 404:
          return 'Recurso no encontrado'
        case 500:
          return 'Error interno del servidor. Intenta nuevamente'
        default:
          return data.message || `Error del servidor (${status})`
      }
    }
    
    if (error.request) {
      // Error de red
      return 'Error de conexión. Verifica tu conexión a internet'
    }
    
    return error.message || 'Ha ocurrido un error inesperado'
  }

  // Obtener tipo de error
  const getErrorType = (error) => {
    if (error.response) {
      const status = error.response.status
      if (status >= 400 && status < 500) return 'client'
      if (status >= 500) return 'server'
    }
    if (error.request) return 'network'
    return 'unknown'
  }

  // Wrapper para manejar operaciones async
  const handleAsync = async (operation, context = '') => {
    isLoading.value = true
    globalErrors.isLoading = true
    
    try {
      const result = await operation()
      return { success: true, data: result }
    } catch (error) {
      addError(error, context)
      return { success: false, error }
    } finally {
      isLoading.value = false
      globalErrors.isLoading = false
    }
  }

  // Notificación de éxito
  const addSuccess = (message) => {
    const successObj = {
      id: Date.now(),
      message,
      type: 'success',
      timestamp: new Date()
    }
    
    errors.value.push(successObj)
    
    // Auto-remover después de 3 segundos
    setTimeout(() => {
      removeError(successObj.id)
    }, 3000)
  }

  return {
    errors,
    isLoading,
    globalErrors,
    addError,
    removeError,
    clearErrors,
    handleAsync,
    addSuccess
  }
}