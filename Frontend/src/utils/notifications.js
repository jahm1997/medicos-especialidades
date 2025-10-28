// Sistema de notificaciones no intrusivo
export class NotificationManager {
  constructor() {
    this.container = null
    this.init()
  }

  init() {
    // Crear contenedor de notificaciones si no existe
    if (!document.getElementById('notification-container')) {
      this.container = document.createElement('div')
      this.container.id = 'notification-container'
      this.container.style.cssText = `
        position: fixed;
        top: 20px;
        right: 20px;
        z-index: 10000;
        pointer-events: none;
      `
      document.body.appendChild(this.container)
    } else {
      this.container = document.getElementById('notification-container')
    }
  }

  show(message, type = 'info', duration = 4000) {
    const notification = document.createElement('div')
    notification.style.cssText = `
      background: ${this.getBackgroundColor(type)};
      color: white;
      padding: 12px 20px;
      margin-bottom: 10px;
      border-radius: 6px;
      box-shadow: 0 4px 12px rgba(0,0,0,0.15);
      font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
      font-size: 14px;
      max-width: 350px;
      word-wrap: break-word;
      pointer-events: auto;
      transform: translateX(100%);
      transition: transform 0.3s ease-in-out;
      display: flex;
      align-items: center;
      gap: 10px;
    `

    const icon = this.getIcon(type)
    notification.innerHTML = `
      <span style="font-size: 16px;">${icon}</span>
      <span>${message}</span>
    `

    this.container.appendChild(notification)

    // Animación de entrada
    setTimeout(() => {
      notification.style.transform = 'translateX(0)'
    }, 10)

    // Auto-remove después del tiempo especificado
    setTimeout(() => {
      notification.style.transform = 'translateX(100%)'
      setTimeout(() => {
        if (notification.parentNode) {
          notification.parentNode.removeChild(notification)
        }
      }, 300)
    }, duration)

    // Permitir cerrar haciendo clic
    notification.addEventListener('click', () => {
      notification.style.transform = 'translateX(100%)'
      setTimeout(() => {
        if (notification.parentNode) {
          notification.parentNode.removeChild(notification)
        }
      }, 300)
    })
  }

  getBackgroundColor(type) {
    const colors = {
      success: '#28a745',
      error: '#dc3545',
      warning: '#ffc107',
      info: '#17a2b8'
    }
    return colors[type] || colors.info
  }

  getIcon(type) {
    const icons = {
      success: '✓',
      error: '✕',
      warning: '⚠',
      info: 'ℹ'
    }
    return icons[type] || icons.info
  }

  success(message, duration = 4000) {
    this.show(message, 'success', duration)
  }

  error(message, duration = 6000) {
    this.show(message, 'error', duration)
  }

  warning(message, duration = 5000) {
    this.show(message, 'warning', duration)
  }

  info(message, duration = 4000) {
    this.show(message, 'info', duration)
  }
}

// Instancia global
export const notify = new NotificationManager()

// Función para mostrar estados de "no hay datos"
export function showNoDataState(container, message = 'No hay datos disponibles') {
  if (typeof container === 'string') {
    container = document.querySelector(container)
  }
  
  if (!container) return

  const noDataDiv = document.createElement('div')
  noDataDiv.className = 'no-data-state'
  noDataDiv.style.cssText = `
    text-align: center;
    padding: 40px 20px;
    color: #6c757d;
    font-size: 16px;
    background: #f8f9fa;
    border-radius: 8px;
    border: 2px dashed #dee2e6;
  `
  
  noDataDiv.innerHTML = `
    <div style="font-size: 48px; margin-bottom: 16px; opacity: 0.5;">📋</div>
    <div style="font-weight: 500; margin-bottom: 8px;">${message}</div>
    <div style="font-size: 14px; opacity: 0.7;">Los datos aparecerán aquí cuando estén disponibles</div>
  `

  // Limpiar contenedor y agregar estado de no datos
  container.innerHTML = ''
  container.appendChild(noDataDiv)
}