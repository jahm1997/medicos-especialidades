<template>
  <teleport to="body">
    <div class="error-notifications">
      <transition-group name="notification" tag="div">
        <div
          v-for="error in errors"
          :key="error.id"
          :class="[
            'notification',
            `notification-${error.type || 'error'}`
          ]"
        >
          <div class="notification-content">
            <div class="notification-icon">
              <i :class="getIconClass(error.type)"></i>
            </div>
            <div class="notification-message">
              <strong v-if="error.context">{{ error.context }}:</strong>
              {{ error.message }}
            </div>
            <button 
              class="notification-close"
              @click="removeError(error.id)"
            >
              <i class="bi bi-x"></i>
            </button>
          </div>
          <div 
            class="notification-progress"
            :class="`progress-${error.type || 'error'}`"
          ></div>
        </div>
      </transition-group>
    </div>
  </teleport>
</template>

<script>
import { useErrorHandler } from '../composables/useErrorHandler'

export default {
  name: 'ErrorNotification',
  setup() {
    const { globalErrors, removeError } = useErrorHandler()

    const getIconClass = (type) => {
      switch (type) {
        case 'success':
          return 'bi bi-check-circle-fill'
        case 'warning':
          return 'bi bi-exclamation-triangle-fill'
        case 'network':
          return 'bi bi-wifi-off'
        case 'server':
          return 'bi bi-server'
        default:
          return 'bi bi-exclamation-circle-fill'
      }
    }

    return {
      errors: globalErrors.errors,
      removeError,
      getIconClass
    }
  }
}
</script>

<style scoped>
.error-notifications {
  position: fixed;
  top: 20px;
  right: 20px;
  z-index: 9999;
  max-width: 400px;
}

.notification {
  background: white;
  border-radius: 8px;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
  margin-bottom: 10px;
  overflow: hidden;
  position: relative;
}

.notification-error {
  border-left: 4px solid #dc3545;
}

.notification-success {
  border-left: 4px solid #28a745;
}

.notification-warning {
  border-left: 4px solid #ffc107;
}

.notification-network {
  border-left: 4px solid #6c757d;
}

.notification-server {
  border-left: 4px solid #fd7e14;
}

.notification-content {
  display: flex;
  align-items: flex-start;
  padding: 16px;
  gap: 12px;
}

.notification-icon {
  flex-shrink: 0;
  font-size: 20px;
}

.notification-error .notification-icon {
  color: #dc3545;
}

.notification-success .notification-icon {
  color: #28a745;
}

.notification-warning .notification-icon {
  color: #ffc107;
}

.notification-network .notification-icon {
  color: #6c757d;
}

.notification-server .notification-icon {
  color: #fd7e14;
}

.notification-message {
  flex: 1;
  font-size: 14px;
  line-height: 1.4;
  color: #333;
}

.notification-message strong {
  color: #000;
  margin-right: 4px;
}

.notification-close {
  background: none;
  border: none;
  color: #6c757d;
  cursor: pointer;
  font-size: 18px;
  padding: 0;
  width: 24px;
  height: 24px;
  display: flex;
  align-items: center;
  justify-content: center;
  border-radius: 4px;
  transition: all 0.2s ease;
}

.notification-close:hover {
  background-color: #f8f9fa;
  color: #495057;
}

.notification-progress {
  height: 3px;
  width: 100%;
  animation: progress 5s linear forwards;
}

.progress-error {
  background-color: #dc3545;
}

.progress-success {
  background-color: #28a745;
}

.progress-warning {
  background-color: #ffc107;
}

.progress-network {
  background-color: #6c757d;
}

.progress-server {
  background-color: #fd7e14;
}

@keyframes progress {
  from {
    width: 100%;
  }
  to {
    width: 0%;
  }
}

/* Transiciones */
.notification-enter-active,
.notification-leave-active {
  transition: all 0.3s ease;
}

.notification-enter-from {
  opacity: 0;
  transform: translateX(100%);
}

.notification-leave-to {
  opacity: 0;
  transform: translateX(100%);
}

.notification-move {
  transition: transform 0.3s ease;
}

@media (max-width: 768px) {
  .error-notifications {
    top: 10px;
    right: 10px;
    left: 10px;
    max-width: none;
  }
  
  .notification-content {
    padding: 12px;
  }
  
  .notification-message {
    font-size: 13px;
  }
}
</style>