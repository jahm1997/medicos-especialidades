import { createRouter, createWebHistory } from 'vue-router'
import Dashboard from '../views/Dashboard.vue'
import ReservarCita from '../components/ReservarCita.vue'

const routes = [
  {
    path: '/',
    name: 'Dashboard',
    component: Dashboard
  },
  {
    path: '/reservar-cita',
    name: 'ReservarCita',
    component: ReservarCita
  }
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

export default router