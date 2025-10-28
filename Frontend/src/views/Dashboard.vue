<template>
  <div class="dashboard-container">
    <!-- Header -->
    <header class="dashboard-header">
      <div class="container-fluid">
        <div class="row align-items-center">
          <div class="col">
            <h1 class="dashboard-title">
              <i class="bi bi-hospital"></i>
              Sistema de Reservas Médicas
            </h1>
          </div>
          <div class="col-auto">
            <router-link to="/reservar-cita" class="btn btn-primary btn-lg">
              <i class="bi bi-calendar-plus"></i>
              Reservar Cita
            </router-link>
          </div>
        </div>
      </div>
    </header>

    <!-- Navigation Tabs -->
    <div class="container-fluid mt-4">
      <ul class="nav nav-tabs" id="dashboardTabs" role="tablist">
        <li class="nav-item" role="presentation">
          <button 
            class="nav-link active" 
            id="citas-tab" 
            data-bs-toggle="tab" 
            data-bs-target="#citas" 
            type="button" 
            role="tab"
          >
            <i class="bi bi-calendar-check"></i>
            Citas
          </button>
        </li>
        <li class="nav-item" role="presentation">
          <button 
            class="nav-link" 
            id="pacientes-tab" 
            data-bs-toggle="tab" 
            data-bs-target="#pacientes" 
            type="button" 
            role="tab"
          >
            <i class="bi bi-people"></i>
            Pacientes
          </button>
        </li>
        <li class="nav-item" role="presentation">
          <button 
            class="nav-link" 
            id="medicos-tab" 
            data-bs-toggle="tab" 
            data-bs-target="#medicos" 
            type="button" 
            role="tab"
          >
            <i class="bi bi-person-badge"></i>
            Médicos
          </button>
        </li>
        <li class="nav-item" role="presentation">
          <button 
            class="nav-link" 
            id="especialidades-tab" 
            data-bs-toggle="tab" 
            data-bs-target="#especialidades" 
            type="button" 
            role="tab"
          >
            <i class="bi bi-heart-pulse"></i>
            Especialidades
          </button>
        </li>
        <li class="nav-item" role="presentation">
          <button 
            class="nav-link" 
            id="horarios-tab" 
            data-bs-toggle="tab" 
            data-bs-target="#horarios" 
            type="button" 
            role="tab"
          >
            <i class="bi bi-clock"></i>
            Horarios
          </button>
        </li>
      </ul>

      <!-- Tab Content -->
      <div class="tab-content" id="dashboardTabContent">
        <div class="tab-pane fade show active" id="citas" role="tabpanel">
          <Citas />
        </div>
        <div class="tab-pane fade" id="pacientes" role="tabpanel">
          <Pacientes />
        </div>
        <div class="tab-pane fade" id="medicos" role="tabpanel">
          <UsuariosList />
        </div>
        <div class="tab-pane fade" id="especialidades" role="tabpanel">
          <Especialidades />
        </div>
        <div class="tab-pane fade" id="horarios" role="tabpanel">
          <HorariosDisponibles />
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { defineAsyncComponent } from 'vue'

// Lazy loading de componentes para mejor performance
const Citas = defineAsyncComponent(() => import('../components/Citas.vue'))
const Pacientes = defineAsyncComponent(() => import('../components/Pacientes.vue'))
const UsuariosList = defineAsyncComponent(() => import('../components/UsuariosList.vue'))
const Especialidades = defineAsyncComponent(() => import('../components/Especialidades.vue'))
const HorariosDisponibles = defineAsyncComponent(() => import('../components/HorariosDisponibles.vue'))

export default {
  name: 'Dashboard',
  components: {
    Citas,
    Pacientes,
    UsuariosList,
    Especialidades,
    HorariosDisponibles
  }
}
</script>

<style scoped>
.dashboard-container {
  min-height: 100vh;
  background-color: #f8f9fa;
}

.dashboard-header {
  background: linear-gradient(135deg, #007bff 0%, #0056b3 100%);
  color: white;
  padding: 20px 0;
  box-shadow: 0 2px 10px rgba(0,0,0,0.1);
}

.dashboard-title {
  margin: 0;
  font-size: 2rem;
  font-weight: 600;
}

.dashboard-title i {
  margin-right: 15px;
  color: #ffc107;
}

.nav-tabs {
  border-bottom: 2px solid #dee2e6;
  background-color: white;
  border-radius: 8px 8px 0 0;
  padding: 0 15px;
}

.nav-tabs .nav-link {
  border: none;
  color: #6c757d;
  font-weight: 500;
  padding: 15px 20px;
  margin-right: 5px;
  border-radius: 8px 8px 0 0;
  transition: all 0.3s ease;
}

.nav-tabs .nav-link:hover {
  color: #007bff;
  background-color: #f8f9fa;
}

.nav-tabs .nav-link.active {
  color: #007bff;
  background-color: white;
  border-bottom: 3px solid #007bff;
  font-weight: 600;
}

.nav-tabs .nav-link i {
  margin-right: 8px;
  font-size: 1.1em;
}

.tab-content {
  background-color: white;
  border-radius: 0 0 8px 8px;
  box-shadow: 0 2px 10px rgba(0,0,0,0.1);
  min-height: 600px;
}

.tab-pane {
  padding: 20px;
}

.btn-lg {
  padding: 12px 24px;
  font-size: 1.1rem;
  border-radius: 8px;
  font-weight: 500;
  box-shadow: 0 2px 8px rgba(0,123,255,0.3);
  transition: all 0.3s ease;
}

.btn-lg:hover {
  transform: translateY(-2px);
  box-shadow: 0 4px 12px rgba(0,123,255,0.4);
}

.btn-lg i {
  margin-right: 8px;
}

@media (max-width: 768px) {
  .dashboard-title {
    font-size: 1.5rem;
  }
  
  .nav-tabs .nav-link {
    padding: 10px 15px;
    font-size: 0.9rem;
  }
  
  .btn-lg {
    font-size: 1rem;
    padding: 10px 20px;
  }
}
</style>