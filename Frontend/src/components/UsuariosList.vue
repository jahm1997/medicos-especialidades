<template>
  <div class="container mt-4">
    <div class="row">
      <div class="col-12">
        <div class="d-flex justify-content-between align-items-center mb-4">
          <h2 class="text-primary">
            <i class="bi bi-people-fill me-2"></i>
            Gestión de Usuarios
          </h2>
          <button 
            class="btn btn-success" 
            @click="showCreateForm = true"
          >
            <i class="bi bi-plus-circle me-2"></i>
            Nuevo Usuario
          </button>
        </div>

        <!-- Formulario de creación/edición -->
        <div v-if="showCreateForm || editingUser" class="card mb-4">
          <div class="card-header">
            <h5 class="mb-0">
              {{ editingUser ? 'Editar Usuario' : 'Crear Nuevo Usuario' }}
            </h5>
          </div>
          <div class="card-body">
            <form @submit.prevent="saveUser">
              <div class="row">
                <div class="col-md-6 mb-3">
                  <label for="nombre" class="form-label">Nombre</label>
                  <input 
                    type="text" 
                    class="form-control" 
                    id="nombre"
                    v-model="userForm.nombre" 
                    required
                  >
                </div>
                <div class="col-md-6 mb-3">
                  <label for="email" class="form-label">Email</label>
                  <input 
                    type="email" 
                    class="form-control" 
                    id="email"
                    v-model="userForm.email" 
                    required
                  >
                </div>
              </div>
              <div class="row">
                <div class="col-md-6 mb-3">
                  <label for="password" class="form-label">Contraseña</label>
                  <input 
                    type="password" 
                    class="form-control" 
                    id="password"
                    v-model="userForm.password" 
                    :required="!editingUser"
                  >
                </div>
                <div class="col-md-6 mb-3">
                  <div class="form-check mt-4">
                    <input 
                      class="form-check-input" 
                      type="checkbox" 
                      id="activo"
                      v-model="userForm.activo"
                    >
                    <label class="form-check-label" for="activo">
                      Usuario Activo
                    </label>
                  </div>
                </div>
              </div>
              <div class="d-flex gap-2">
                <button type="submit" class="btn btn-primary" :disabled="loading">
                  <i class="bi bi-check-circle me-2"></i>
                  {{ editingUser ? 'Actualizar' : 'Crear' }}
                </button>
                <button 
                  type="button" 
                  class="btn btn-secondary" 
                  @click="cancelForm"
                >
                  <i class="bi bi-x-circle me-2"></i>
                  Cancelar
                </button>
              </div>
            </form>
          </div>
        </div>

        <!-- Lista de usuarios -->
        <div class="card">
          <div class="card-header">
            <h5 class="mb-0">Lista de Usuarios</h5>
          </div>
          <div class="card-body">
            <div v-if="loading" class="text-center py-4">
              <div class="spinner-border text-primary" role="status">
                <span class="visually-hidden">Cargando...</span>
              </div>
            </div>
            
            <div v-else-if="usuarios.length === 0" class="text-center py-4">
              <i class="bi bi-inbox display-1 text-muted"></i>
              <p class="text-muted mt-3">No hay usuarios registrados</p>
            </div>
            
            <div v-else class="table-responsive">
              <table class="table table-hover">
                <thead class="table-dark">
                  <tr>
                    <th>ID</th>
                    <th>Nombre</th>
                    <th>Email</th>
                    <th>Fecha Creación</th>
                    <th>Estado</th>
                    <th>Acciones</th>
                  </tr>
                </thead>
                <tbody>
                  <tr v-for="usuario in usuarios" :key="usuario.id">
                    <td>{{ usuario.id }}</td>
                    <td>{{ usuario.nombre }}</td>
                    <td>{{ usuario.email }}</td>
                    <td>{{ formatDate(usuario.fechaCreacion) }}</td>
                    <td>
                      <span 
                        :class="usuario.activo ? 'badge bg-success' : 'badge bg-danger'"
                      >
                        {{ usuario.activo ? 'Activo' : 'Inactivo' }}
                      </span>
                    </td>
                    <td>
                      <div class="btn-group" role="group">
                        <button 
                          class="btn btn-sm btn-outline-primary" 
                          @click="editUser(usuario)"
                          title="Editar"
                        >
                          <i class="bi bi-pencil"></i>
                        </button>
                        <button 
                          class="btn btn-sm btn-outline-danger" 
                          @click="deleteUser(usuario.id)"
                          title="Eliminar"
                        >
                          <i class="bi bi-trash"></i>
                        </button>
                      </div>
                    </td>
                  </tr>
                </tbody>
              </table>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { usuarioService } from '../services/api.js'

export default {
  name: 'UsuariosList',
  data() {
    return {
      usuarios: [],
      loading: false,
      showCreateForm: false,
      editingUser: null,
      userForm: {
        nombre: '',
        email: '',
        password: '',
        activo: true
      }
    }
  },
  async mounted() {
    await this.loadUsuarios()
  },
  methods: {
    async loadUsuarios() {
      this.loading = true
      try {
        const response = await usuarioService.getAll()
        this.usuarios = response.data
      } catch (error) {
        console.error('Error al cargar usuarios:', error)
        alert('Error al cargar los usuarios')
      } finally {
        this.loading = false
      }
    },
    
    async saveUser() {
      this.loading = true
      try {
        if (this.editingUser) {
          await usuarioService.update(this.editingUser.id, this.userForm)
        } else {
          await usuarioService.create(this.userForm)
        }
        
        await this.loadUsuarios()
        this.cancelForm()
        alert(this.editingUser ? 'Usuario actualizado correctamente' : 'Usuario creado correctamente')
      } catch (error) {
        console.error('Error al guardar usuario:', error)
        alert('Error al guardar el usuario')
      } finally {
        this.loading = false
      }
    },
    
    editUser(usuario) {
      this.editingUser = usuario
      this.userForm = {
        nombre: usuario.nombre,
        email: usuario.email,
        password: '',
        activo: usuario.activo
      }
      this.showCreateForm = false
    },
    
    async deleteUser(id) {
      if (confirm('¿Estás seguro de que quieres eliminar este usuario?')) {
        this.loading = true
        try {
          await usuarioService.delete(id)
          await this.loadUsuarios()
          alert('Usuario eliminado correctamente')
        } catch (error) {
          console.error('Error al eliminar usuario:', error)
          alert('Error al eliminar el usuario')
        } finally {
          this.loading = false
        }
      }
    },
    
    cancelForm() {
      this.showCreateForm = false
      this.editingUser = null
      this.userForm = {
        nombre: '',
        email: '',
        password: '',
        activo: true
      }
    },
    
    formatDate(dateString) {
      if (!dateString) return 'N/A'
      return new Date(dateString).toLocaleDateString('es-ES')
    }
  }
}
</script>

<style scoped>
.card {
  box-shadow: 0 0.125rem 0.25rem rgba(0, 0, 0, 0.075);
  border: 1px solid rgba(0, 0, 0, 0.125);
}

.table th {
  font-weight: 600;
}

.btn-group .btn {
  margin-right: 0.25rem;
}

.btn-group .btn:last-child {
  margin-right: 0;
}
</style>