<template>
  <div id="app">
    <router-view />
    
    <!-- Componente global de notificaciones -->
    <ErrorNotification />
    
    <!-- Footer con Copyright -->
    <footer class="app-footer">
      <div class="footer-content">
        <p>&copy; {{ currentYear }} Sistema de Reservas Médicas. Desarrollado con ❤️ por 
          <a href="https://github.com/JosephHerrera" target="_blank" rel="noopener noreferrer" class="github-link">
            Joseph Herrera
          </a>
        </p>
        <div class="footer-links">
          <a href="https://github.com/JosephHerrera" target="_blank" rel="noopener noreferrer" class="footer-link">
            <i class="bi bi-github"></i> GitHub
          </a>
          <span class="separator">|</span>
          <span class="tech-stack">Vue.js + .NET Core</span>
        </div>
      </div>
    </footer>
  </div>
</template>

<script>
import ErrorNotification from './components/ErrorNotification.vue'

export default {
  name: 'App',
  components: {
    ErrorNotification
  },
  computed: {
    currentYear() {
      return new Date().getFullYear()
    }
  }
}
</script>

<style>
#app {
  font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  margin: 0;
  padding: 0;
  min-height: 100vh;
  display: flex;
  flex-direction: column;
}

body {
  margin: 0;
  padding: 0;
  background-color: #f8f9fa;
}

* {
  box-sizing: border-box;
}

/* Footer Styles */
.app-footer {
  background: linear-gradient(135deg, #2c3e50 0%, #34495e 100%);
  color: white;
  padding: 20px 0;
  margin-top: auto;
  box-shadow: 0 -2px 10px rgba(0,0,0,0.1);
}

.footer-content {
  max-width: 1200px;
  margin: 0 auto;
  text-align: center;
  padding: 0 20px;
}

.footer-content p {
  margin: 0 0 10px 0;
  font-size: 14px;
  line-height: 1.5;
}

.github-link {
  color: #3498db;
  text-decoration: none;
  font-weight: 600;
  transition: color 0.3s ease;
}

.github-link:hover {
  color: #2980b9;
  text-decoration: underline;
}

.footer-links {
  display: flex;
  justify-content: center;
  align-items: center;
  gap: 10px;
  font-size: 13px;
  opacity: 0.8;
}

.footer-link {
  color: #ecf0f1;
  text-decoration: none;
  display: flex;
  align-items: center;
  gap: 5px;
  transition: color 0.3s ease;
}

.footer-link:hover {
  color: #3498db;
}

.separator {
  color: #7f8c8d;
}

.tech-stack {
  color: #e74c3c;
  font-weight: 500;
}

@media (max-width: 768px) {
  .footer-content p {
    font-size: 13px;
  }
  
  .footer-links {
    flex-direction: column;
    gap: 5px;
  }
  
  .separator {
    display: none;
  }
}
</style>
