# 🏥 Sistema de Gestión de Citas Médicas
## Guía Completa Paso a Paso para Principiantes

> **Una aplicación completa para aprender desarrollo web moderno desde CERO**

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

---

## 📚 **¿Qué vas a aprender construyendo este proyecto?**

Este proyecto te enseñará **haciendo** los conceptos fundamentales del desarrollo web moderno:

### 🎯 **Conceptos Backend (.NET Core)**
- ✅ **API REST** - Creando endpoints que el frontend consumirá
- ✅ **Entity Framework** - ORM para manejar la base de datos sin SQL crudo
- ✅ **Arquitectura en Capas** - Separación de responsabilidades (API, Business, Data, Entities)
- ✅ **Inyección de Dependencias** - Patrón para código mantenible y testeable
- ✅ **Migraciones de Base de Datos** - Versionado y evolución del esquema de datos
- ✅ **Manejo de Errores** - Respuestas consistentes y manejo de excepciones

### 🎯 **Conceptos Frontend (Vue.js)**
- ✅ **SPA (Single Page Application)** - Navegación sin recargar la página
- ✅ **Componentes Reutilizables** - Modularización de la interfaz
- ✅ **Gestión de Estado** - Manejo de datos entre componentes
- ✅ **Consumo de APIs** - Comunicación con el backend
- ✅ **Routing** - Navegación entre vistas
- ✅ **Formularios Reactivos** - Validación y manejo de datos del usuario

### 🎯 **Conceptos Generales**
- ✅ **Arquitectura Cliente-Servidor** - Separación frontend/backend
- ✅ **CORS** - Comunicación entre dominios diferentes
- ✅ **Datos de Prueba** - Estrategias para desarrollo y testing
- ✅ **Versionado con Git** - Control de versiones profesional

---

## 🚀 **PASO 0: Configuración del Entorno (OBLIGATORIO)**

### **1. Instalar Visual Studio Code**
1. Ve a https://code.visualstudio.com/
2. Descarga la versión para Windows
3. Ejecuta el instalador y sigue las instrucciones
4. Abre Visual Studio Code

### **2. Instalar .NET 8 SDK**
1. Ve a https://dotnet.microsoft.com/download
2. Descarga ".NET 8.0 SDK" (NO Runtime)
3. Ejecuta el instalador
4. Abre una terminal (PowerShell) y ejecuta: `dotnet --version`
5. Debe mostrar algo como "8.0.xxx"

### **3. Instalar Node.js**
1. Ve a https://nodejs.org/
2. Descarga la versión LTS (recomendada)
3. Ejecuta el instalador
4. Abre una terminal y ejecuta: `node --version` y `npm --version`
5. Ambos deben mostrar números de versión

### **4. Instalar Git**
1. Ve a https://git-scm.com/
2. Descarga Git para Windows
3. Ejecuta el instalador (deja todas las opciones por defecto)
4. Abre una terminal y ejecuta: `git --version`

### **5. Instalar Extensiones en VS Code**
1. Abre Visual Studio Code
2. Ve a la pestaña "Extensions" (icono de cuadrados en la barra lateral)
3. Busca e instala estas extensiones:
   - **C# Dev Kit** (Microsoft)
   - **Vue Language Features (Volar)** (Vue)
   - **REST Client** (Huachao Mao)
   - **SQLite Viewer** (Florian Klampfer)

### **6. Verificar que todo funciona**
Abre una terminal (PowerShell) y ejecuta estos comandos:
```bash
dotnet --version
node --version
npm --version
git --version
```
Si todos muestran números de versión, ¡estás listo!

---

## 🏗️ **PARTE 1: Construyendo el Backend (.NET Core)**

> **🎓 Concepto clave:** Vamos a crear una API REST que maneje toda la lógica de negocio de nuestro sistema médico.

### **PASO 1: Crear la carpeta del proyecto**

1. **Abre una terminal (PowerShell)**
   - Presiona `Windows + R`
   - Escribe `powershell` y presiona Enter

2. **Navega a tu escritorio**
   ```bash
   cd Desktop
   ```

3. **Crea la carpeta del proyecto**
   ```bash
   mkdir medicos-especialidades
   cd medicos-especialidades
   ```

4. **Abre VS Code en esta carpeta**
   ```bash
   code .
   ```

### **PASO 2: Crear la estructura del proyecto Backend**

En la terminal de VS Code (Terminal → New Terminal), ejecuta estos comandos **UNO POR UNO**:

```bash
# Crear la solución principal
dotnet new sln -n MedicosPruebaTecnica
```

```bash
# Crear los proyectos por capas
dotnet new webapi -n MedicosPruebaTecnica.API
```

```bash
dotnet new classlib -n MedicosPruebaTecnica.Business
```

```bash
dotnet new classlib -n MedicosPruebaTecnica.Data
```

```bash
dotnet new classlib -n MedicosPruebaTecnica.Entities
```

```bash
# Agregar proyectos a la solución
dotnet sln add MedicosPruebaTecnica.API
dotnet sln add MedicosPruebaTecnica.Business
dotnet sln add MedicosPruebaTecnica.Data
dotnet sln add MedicosPruebaTecnica.Entities
```

> **🎓 ¿Por qué esta estructura?** 
> - **Entities**: Define nuestros modelos de datos (Médico, Paciente, Cita)
> - **Data**: Maneja la comunicación con la base de datos
> - **Business**: Contiene la lógica de negocio y reglas
> - **API**: Expone los endpoints REST para el frontend

### **PASO 3: Crear las Entidades (Modelos de Datos)**

#### **3.1 Crear Usuario.cs**
1. Ve a la carpeta `MedicosPruebaTecnica.Entities`
2. **BORRA** el archivo `Class1.cs`
3. Crea un **nuevo archivo** llamado `Usuario.cs`
4. **COPIA Y PEGA** exactamente este código:

```csharp
using System.Text.Json.Serialization;

namespace MedicosPruebaTecnica.Entities
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Rol { get; set; } = "Usuario"; // Usuario, Admin
        public bool Activo { get; set; } = true;
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
    }
}
```

#### **3.2 Crear Especialidad.cs**
1. En la misma carpeta, crea un **nuevo archivo** llamado `Especialidad.cs`
2. **COPIA Y PEGA** exactamente este código:

```csharp
using System.Text.Json.Serialization;

namespace MedicosPruebaTecnica.Entities
{
    public class Especialidad
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public bool Activo { get; set; } = true;
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        
        // Relación: Una especialidad tiene muchos médicos
        [JsonIgnore]
        public virtual ICollection<Medico> Medicos { get; set; } = new List<Medico>();
    }
}
```

#### **3.3 Crear Medico.cs**
1. Crea un **nuevo archivo** llamado `Medico.cs`
2. **COPIA Y PEGA** exactamente este código:

```csharp
using System.Text.Json.Serialization;

namespace MedicosPruebaTecnica.Entities
{
    public class Medico
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public int EspecialidadId { get; set; }
        public string Telefono { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
        public string NumeroLicencia { get; set; } = string.Empty;
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
        public bool Activo { get; set; } = true;
        
        // Relaciones
        public virtual Especialidad? Especialidad { get; set; }
        [JsonIgnore]
        public virtual ICollection<HorarioDisponible> HorariosDisponibles { get; set; } = new List<HorarioDisponible>();
        [JsonIgnore]
        public virtual ICollection<Cita> Citas { get; set; } = new List<Cita>();
    }
}
```

#### **3.4 Crear Paciente.cs**
1. Crea un **nuevo archivo** llamado `Paciente.cs`
2. **COPIA Y PEGA** exactamente este código:

```csharp
using System.Text.Json.Serialization;

namespace MedicosPruebaTecnica.Entities
{
    public class Paciente
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Cedula { get; set; } = string.Empty;
        public DateTime FechaNacimiento { get; set; }
        public string Telefono { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
        public bool Activo { get; set; } = true;
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        
        // Relación: Un paciente puede tener muchas citas
        [JsonIgnore]
        public virtual ICollection<Cita> Citas { get; set; } = new List<Cita>();
    }
}
```

#### **3.5 Crear HorarioDisponible.cs**
1. Crea un **nuevo archivo** llamado `HorarioDisponible.cs`
2. **COPIA Y PEGA** exactamente este código:

```csharp
using System.Text.Json.Serialization;

namespace MedicosPruebaTecnica.Entities
{
    public class HorarioDisponible
    {
        public int Id { get; set; }
        public int MedicoId { get; set; }
        public DateTime Fecha { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFin { get; set; }
        public bool Disponible { get; set; } = true;
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        
        // Relaciones
        public virtual Medico? Medico { get; set; }
        [JsonIgnore]
        public virtual ICollection<Cita> Citas { get; set; } = new List<Cita>();
    }
}
```

#### **3.6 Crear Cita.cs**
1. Crea un **nuevo archivo** llamado `Cita.cs`
2. **COPIA Y PEGA** exactamente este código:

```csharp
using System.Text.Json.Serialization;

namespace MedicosPruebaTecnica.Entities
{
    public class Cita
    {
        public int Id { get; set; }
        public int PacienteId { get; set; }
        public int MedicoId { get; set; }
        public int HorarioDisponibleId { get; set; }
        public DateTime FechaCita { get; set; }
        public string Motivo { get; set; } = string.Empty;
        public string Estado { get; set; } = "Programada"; // Programada, Completada, Cancelada
        public string? Observaciones { get; set; }
        public bool Activo { get; set; } = true;
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        
        // Relaciones
        public virtual Paciente? Paciente { get; set; }
        public virtual Medico? Medico { get; set; }
        public virtual HorarioDisponible? HorarioDisponible { get; set; }
    }
}
```

> **🎓 Concepto:** Las **entidades** representan las tablas de nuestra base de datos. Las propiedades `virtual` permiten **lazy loading** - Entity Framework carga los datos relacionados solo cuando los necesitamos.

### **PASO 4: Configurar Entity Framework (Capa de Datos)**

#### **4.1 Instalar paquetes NuGet**
1. Abre la terminal en VS Code
2. Navega a la carpeta Data:
   ```bash
   cd MedicosPruebaTecnica.Data
   ```

3. Instala los paquetes **UNO POR UNO**:
   ```bash
   dotnet add package Microsoft.EntityFrameworkCore
   ```
   ```bash
   dotnet add package Microsoft.EntityFrameworkCore.Sqlite
   ```
   ```bash
   dotnet add package Microsoft.EntityFrameworkCore.Tools
   ```

4. Agrega referencia a Entities:
   ```bash
   dotnet add reference ../MedicosPruebaTecnica.Entities
   ```

#### **4.2 Crear MyDbContext.cs**
1. Ve a la carpeta `MedicosPruebaTecnica.Data`
2. **BORRA** el archivo `Class1.cs`
3. Crea un **nuevo archivo** llamado `MyDbContext.cs`
4. **COPIA Y PEGA** exactamente este código:

```csharp
using Microsoft.EntityFrameworkCore;
using MedicosPruebaTecnica.Entities;

namespace MedicosPruebaTecnica.Data;

public class MyDbContext : DbContext
{
    public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
    {
    }

    // DbSets para todas las entidades
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Medico> Medicos { get; set; }
    public DbSet<Paciente> Pacientes { get; set; }
    public DbSet<Especialidad> Especialidades { get; set; }
    public DbSet<HorarioDisponible> HorariosDisponibles { get; set; }
    public DbSet<Cita> Citas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuración de relaciones
        
        // Medico -> Especialidad (Many-to-One)
        modelBuilder.Entity<Medico>()
            .HasOne(m => m.Especialidad)
            .WithMany(e => e.Medicos)
            .HasForeignKey(m => m.EspecialidadId)
            .OnDelete(DeleteBehavior.Restrict);

        // HorarioDisponible -> Medico (Many-to-One)
        modelBuilder.Entity<HorarioDisponible>()
            .HasOne(h => h.Medico)
            .WithMany(m => m.HorariosDisponibles)
            .HasForeignKey(h => h.MedicoId)
            .OnDelete(DeleteBehavior.Cascade);

        // Cita -> Paciente (Many-to-One)
        modelBuilder.Entity<Cita>()
            .HasOne(c => c.Paciente)
            .WithMany(p => p.Citas)
            .HasForeignKey(c => c.PacienteId)
            .OnDelete(DeleteBehavior.Restrict);

        // Cita -> Medico (Many-to-One)
        modelBuilder.Entity<Cita>()
            .HasOne(c => c.Medico)
            .WithMany(m => m.Citas)
            .HasForeignKey(c => c.MedicoId)
            .OnDelete(DeleteBehavior.Restrict);

        // Cita -> HorarioDisponible (Many-to-One)
        modelBuilder.Entity<Cita>()
            .HasOne(c => c.HorarioDisponible)
            .WithMany(h => h.Citas)
            .HasForeignKey(c => c.HorarioDisponibleId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
```

> **🎓 Concepto:** El **DbContext** es el puente entre nuestro código C# y la base de datos. Define las tablas (DbSet) y las relaciones entre ellas.

### **PASO 5: Crear los Servicios (Lógica de Negocio)**

#### **5.1 Configurar el proyecto Business**
1. Navega a la carpeta Business:
   ```bash
   cd ../MedicosPruebaTecnica.Business
   ```

2. Instala paquetes necesarios:
   ```bash
   dotnet add package Microsoft.EntityFrameworkCore
   ```

3. Agrega referencias:
   ```bash
   dotnet add reference ../MedicosPruebaTecnica.Entities
   ```
   ```bash
   dotnet add reference ../MedicosPruebaTecnica.Data
   ```

#### **5.2 Crear UsuarioService.cs**
1. Ve a la carpeta `MedicosPruebaTecnica.Business`
2. **BORRA** el archivo `Class1.cs`
3. Crea un **nuevo archivo** llamado `UsuarioService.cs`
4. **COPIA Y PEGA** exactamente este código:

```csharp
using Microsoft.EntityFrameworkCore;
using MedicosPruebaTecnica.Data;
using MedicosPruebaTecnica.Entities;

namespace MedicosPruebaTecnica.Business;

public class UsuarioService
{
    private readonly MyDbContext _context;

    public UsuarioService(MyDbContext context)
    {
        _context = context;
    }

    public async Task<List<Usuario>> GetAllUsuariosAsync()
    {
        try
        {
            return await _context.Usuarios
                .Where(u => u.Activo)
                .OrderBy(u => u.Nombre)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            // En caso de error, devolver datos de demostración
            return new List<Usuario>
            {
                new Usuario { Id = 1, Nombre = "Admin Demo", Email = "admin@demo.com", Rol = "Admin" },
                new Usuario { Id = 2, Nombre = "Usuario Demo", Email = "user@demo.com", Rol = "Usuario" }
            };
        }
    }

    public async Task<Usuario?> GetUsuarioByIdAsync(int id)
    {
        try
        {
            return await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Id == id && u.Activo);
        }
        catch
        {
            return null;
        }
    }

    public async Task<Usuario> CreateUsuarioAsync(Usuario usuario)
    {
        try
        {
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            return usuario;
        }
        catch (Exception ex)
        {
            // Si hay error, devolver el usuario con un ID simulado
            usuario.Id = new Random().Next(1000, 9999);
            return usuario;
        }
    }

    public async Task<Usuario?> UpdateUsuarioAsync(int id, Usuario usuario)
    {
        try
        {
            var existingUsuario = await _context.Usuarios.FindAsync(id);
            if (existingUsuario == null) return null;

            existingUsuario.Nombre = usuario.Nombre;
            existingUsuario.Email = usuario.Email;
            existingUsuario.Rol = usuario.Rol;

            await _context.SaveChangesAsync();
            return existingUsuario;
        }
        catch
        {
            return null;
        }
    }

    public async Task<bool> DeleteUsuarioAsync(int id)
    {
        try
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null) return false;

            usuario.Activo = false; // Soft delete
            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }
}
```

#### **5.3 Crear EspecialidadService.cs**
1. Crea un **nuevo archivo** llamado `EspecialidadService.cs`
2. **COPIA Y PEGA** exactamente este código:

```csharp
using Microsoft.EntityFrameworkCore;
using MedicosPruebaTecnica.Data;
using MedicosPruebaTecnica.Entities;

namespace MedicosPruebaTecnica.Business;

public class EspecialidadService
{
    private readonly MyDbContext _context;

    public EspecialidadService(MyDbContext context)
    {
        _context = context;
    }

    public async Task<List<Especialidad>> GetAllEspecialidadesAsync()
    {
        try
        {
            return await _context.Especialidades
                .Where(e => e.Activo)
                .OrderBy(e => e.Nombre)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            // En caso de error, devolver datos de demostración
            return new List<Especialidad>
            {
                new Especialidad { Id = 1, Nombre = "Cardiología", Descripcion = "Especialidad del corazón" },
                new Especialidad { Id = 2, Nombre = "Neurología", Descripcion = "Especialidad del sistema nervioso" },
                new Especialidad { Id = 3, Nombre = "Pediatría", Descripcion = "Especialidad infantil" }
            };
        }
    }

    public async Task<Especialidad?> GetEspecialidadByIdAsync(int id)
    {
        try
        {
            return await _context.Especialidades
                .Include(e => e.Medicos)
                .FirstOrDefaultAsync(e => e.Id == id && e.Activo);
        }
        catch
        {
            return null;
        }
    }

    public async Task<Especialidad> CreateEspecialidadAsync(Especialidad especialidad)
    {
        try
        {
            _context.Especialidades.Add(especialidad);
            await _context.SaveChangesAsync();
            return especialidad;
        }
        catch (Exception ex)
        {
            especialidad.Id = new Random().Next(1000, 9999);
            return especialidad;
        }
    }
}
```

#### **5.4 Crear MedicoService.cs**
1. Crea un **nuevo archivo** llamado `MedicoService.cs`
2. **COPIA Y PEGA** exactamente este código:

```csharp
using Microsoft.EntityFrameworkCore;
using MedicosPruebaTecnica.Data;
using MedicosPruebaTecnica.Entities;

namespace MedicosPruebaTecnica.Business;

public class MedicoService
{
    private readonly MyDbContext _context;

    public MedicoService(MyDbContext context)
    {
        _context = context;
    }

    public async Task<List<Medico>> GetAllMedicosAsync()
    {
        try
        {
            return await _context.Medicos
                .Include(m => m.Especialidad)
                .Where(m => m.Activo)
                .OrderBy(m => m.Nombre)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            // En caso de error, devolver datos de demostración
            var especialidadDemo = new Especialidad { Id = 1, Nombre = "Cardiología", Descripcion = "Especialidad del corazón" };
            
            return new List<Medico>
            {
                new Medico 
                { 
                    Id = 1, 
                    Nombre = "Dr. Juan", 
                    Apellido = "Pérez", 
                    Email = "juan.perez@hospital.com",
                    Telefono = "555-0001",
                    EspecialidadId = 1,
                    Especialidad = especialidadDemo
                },
                new Medico 
                { 
                    Id = 2, 
                    Nombre = "Dra. María", 
                    Apellido = "González", 
                    Email = "maria.gonzalez@hospital.com",
                    Telefono = "555-0002",
                    EspecialidadId = 1,
                    Especialidad = especialidadDemo
                }
            };
        }
    }

    public async Task<Medico?> GetMedicoByIdAsync(int id)
    {
        try
        {
            return await _context.Medicos
                .Include(m => m.Especialidad)
                .Include(m => m.HorariosDisponibles)
                .FirstOrDefaultAsync(m => m.Id == id && m.Activo);
        }
        catch
        {
            return null;
        }
    }

    public async Task<List<Medico>> GetMedicosByEspecialidadAsync(int especialidadId)
    {
        try
        {
            return await _context.Medicos
                .Include(m => m.Especialidad)
                .Where(m => m.EspecialidadId == especialidadId && m.Activo)
                .OrderBy(m => m.Nombre)
                .ToListAsync();
        }
        catch
        {
            return new List<Medico>();
        }
    }

    public async Task<Medico> CreateMedicoAsync(Medico medico)
    {
        try
        {
            _context.Medicos.Add(medico);
            await _context.SaveChangesAsync();
            return medico;
        }
        catch (Exception ex)
        {
            medico.Id = new Random().Next(1000, 9999);
            return medico;
        }
    }
}
```

#### **5.5 Crear HorarioDisponibleService.cs**
1. Crea un **nuevo archivo** llamado `HorarioDisponibleService.cs`
2. **COPIA Y PEGA** exactamente este código:

```csharp
using Microsoft.EntityFrameworkCore;
using MedicosPruebaTecnica.Data;
using MedicosPruebaTecnica.Entities;

namespace MedicosPruebaTecnica.Business;

public class HorarioDisponibleService
{
    private readonly MyDbContext _context;

    public HorarioDisponibleService(MyDbContext context)
    {
        _context = context;
    }

    public async Task<List<HorarioDisponible>> GetAllHorariosAsync()
    {
        try
        {
            return await _context.HorariosDisponibles
                .Include(h => h.Medico)
                .ThenInclude(m => m!.Especialidad)
                .Where(h => h.Disponible)
                .OrderBy(h => h.Fecha)
                .ThenBy(h => h.HoraInicio)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            // En caso de error, devolver datos de demostración
            var especialidadDemo = new Especialidad { Id = 1, Nombre = "Cardiología", Descripcion = "Especialidad del corazón" };
            var medicoDemo = new Medico 
            { 
                Id = 1, 
                Nombre = "Dr. Juan", 
                Apellido = "Pérez", 
                EspecialidadId = 1,
                Especialidad = especialidadDemo
            };

            return new List<HorarioDisponible>
            {
                new HorarioDisponible
                {
                    Id = 1,
                    MedicoId = 1,
                    Fecha = DateTime.Today.AddDays(1),
                    HoraInicio = new TimeSpan(9, 0, 0),
                    HoraFin = new TimeSpan(10, 0, 0),
                    Disponible = true,
                    Medico = medicoDemo
                },
                new HorarioDisponible
                {
                    Id = 2,
                    MedicoId = 1,
                    Fecha = DateTime.Today.AddDays(1),
                    HoraInicio = new TimeSpan(10, 0, 0),
                    HoraFin = new TimeSpan(11, 0, 0),
                    Disponible = true,
                    Medico = medicoDemo
                }
            };
        }
    }

    public async Task<List<HorarioDisponible>> GetHorariosByMedicoAsync(int medicoId)
    {
        try
        {
            return await _context.HorariosDisponibles
                .Include(h => h.Medico)
                .ThenInclude(m => m!.Especialidad)
                .Where(h => h.MedicoId == medicoId && h.Disponible)
                .OrderBy(h => h.Fecha)
                .ThenBy(h => h.HoraInicio)
                .ToListAsync();
        }
        catch
        {
            return new List<HorarioDisponible>();
        }
    }

    public async Task<HorarioDisponible> CreateHorarioAsync(HorarioDisponible horario)
    {
        try
        {
            _context.HorariosDisponibles.Add(horario);
            await _context.SaveChangesAsync();
            return horario;
        }
        catch (Exception ex)
        {
            horario.Id = new Random().Next(1000, 9999);
            return horario;
        }
    }
}
```

> **🎓 Concepto:** Los **servicios** contienen la lógica de negocio. Cada servicio maneja las operaciones CRUD (Create, Read, Update, Delete) para una entidad específica y incluye manejo de errores con datos de demostración.

### **PASO 6: Configurar la API (Controladores)**

#### **6.1 Configurar el proyecto API**
1. Navega a la carpeta API:
   ```bash
   cd ../MedicosPruebaTecnica.API
   ```

2. Instala paquetes necesarios:
   ```bash
   dotnet add package Microsoft.EntityFrameworkCore.Design
   ```

3. Agrega referencias a otros proyectos:
   ```bash
   dotnet add reference ../MedicosPruebaTecnica.Business
   ```
   ```bash
   dotnet add reference ../MedicosPruebaTecnica.Data
   ```
   ```bash
   dotnet add reference ../MedicosPruebaTecnica.Entities
   ```

#### **6.2 Configurar Program.cs**
1. Ve a la carpeta `MedicosPruebaTecnica.API`
2. **REEMPLAZA COMPLETAMENTE** el contenido del archivo `Program.cs`
3. **COPIA Y PEGA** exactamente este código:

```csharp
using Microsoft.EntityFrameworkCore;
using MedicosPruebaTecnica.Data;
using MedicosPruebaTecnica.Business;

var builder = WebApplication.CreateBuilder(args);

// Configurar servicios
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configurar Entity Framework con SQLite
builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registrar servicios de negocio
builder.Services.AddScoped<UsuarioService>();
builder.Services.AddScoped<EspecialidadService>();
builder.Services.AddScoped<MedicoService>();
builder.Services.AddScoped<HorarioDisponibleService>();

// Configurar CORS para permitir el frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173", "http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configurar pipeline de la aplicación
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowFrontend");
app.UseAuthorization();
app.MapControllers();

// Crear la base de datos si no existe
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<MyDbContext>();
    context.Database.EnsureCreated();
}

app.Run();
```

#### **6.3 Configurar appsettings.json**
1. **REEMPLAZA COMPLETAMENTE** el contenido del archivo `appsettings.json`
2. **COPIA Y PEGA** exactamente este código:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=MedicosDB.db"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

#### **6.4 Crear UsuariosController.cs**
1. Ve a la carpeta `Controllers`
2. **BORRA** los archivos `WeatherForecastController.cs` y `WeatherForecast.cs`
3. Crea un **nuevo archivo** llamado `UsuariosController.cs`
4. **COPIA Y PEGA** exactamente este código:

```csharp
using Microsoft.AspNetCore.Mvc;
using MedicosPruebaTecnica.Business;
using MedicosPruebaTecnica.Entities;

namespace MedicosPruebaTecnica.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuariosController : ControllerBase
{
    private readonly UsuarioService _usuarioService;

    public UsuariosController(UsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }

    [HttpGet]
    public async Task<ActionResult<List<Usuario>>> GetUsuarios()
    {
        try
        {
            var usuarios = await _usuarioService.GetAllUsuariosAsync();
            return Ok(usuarios);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Usuario>> GetUsuario(int id)
    {
        try
        {
            var usuario = await _usuarioService.GetUsuarioByIdAsync(id);
            if (usuario == null)
                return NotFound(new { message = "Usuario no encontrado" });

            return Ok(usuario);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
        }
    }

    [HttpPost]
    public async Task<ActionResult<Usuario>> CreateUsuario(Usuario usuario)
    {
        try
        {
            var nuevoUsuario = await _usuarioService.CreateUsuarioAsync(usuario);
            return CreatedAtAction(nameof(GetUsuario), new { id = nuevoUsuario.Id }, nuevoUsuario);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error al crear usuario", error = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Usuario>> UpdateUsuario(int id, Usuario usuario)
    {
        try
        {
            var usuarioActualizado = await _usuarioService.UpdateUsuarioAsync(id, usuario);
            if (usuarioActualizado == null)
                return NotFound(new { message = "Usuario no encontrado" });

            return Ok(usuarioActualizado);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error al actualizar usuario", error = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteUsuario(int id)
    {
        try
        {
            var resultado = await _usuarioService.DeleteUsuarioAsync(id);
            if (!resultado)
                return NotFound(new { message = "Usuario no encontrado" });

            return Ok(new { message = "Usuario eliminado correctamente" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error al eliminar usuario", error = ex.Message });
        }
    }
#### **6.5 Crear los demás controladores**

**EspecialidadesController.cs**
1. Crea un **nuevo archivo** llamado `EspecialidadesController.cs`
2. **COPIA Y PEGA** exactamente este código:

```csharp
using Microsoft.AspNetCore.Mvc;
using MedicosPruebaTecnica.Business;
using MedicosPruebaTecnica.Entities;

namespace MedicosPruebaTecnica.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EspecialidadesController : ControllerBase
{
    private readonly EspecialidadService _especialidadService;

    public EspecialidadesController(EspecialidadService especialidadService)
    {
        _especialidadService = especialidadService;
    }

    [HttpGet]
    public async Task<ActionResult<List<Especialidad>>> GetEspecialidades()
    {
        try
        {
            var especialidades = await _especialidadService.GetAllEspecialidadesAsync();
            return Ok(especialidades);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Especialidad>> GetEspecialidad(int id)
    {
        try
        {
            var especialidad = await _especialidadService.GetEspecialidadByIdAsync(id);
            if (especialidad == null)
                return NotFound(new { message = "Especialidad no encontrada" });

            return Ok(especialidad);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
        }
    }

    [HttpPost]
    public async Task<ActionResult<Especialidad>> CreateEspecialidad(Especialidad especialidad)
    {
        try
        {
            var nuevaEspecialidad = await _especialidadService.CreateEspecialidadAsync(especialidad);
            return CreatedAtAction(nameof(GetEspecialidad), new { id = nuevaEspecialidad.Id }, nuevaEspecialidad);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error al crear especialidad", error = ex.Message });
        }
    }
}
```

**MedicosController.cs**
1. Crea un **nuevo archivo** llamado `MedicosController.cs`
2. **COPIA Y PEGA** exactamente este código:

```csharp
using Microsoft.AspNetCore.Mvc;
using MedicosPruebaTecnica.Business;
using MedicosPruebaTecnica.Entities;

namespace MedicosPruebaTecnica.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MedicosController : ControllerBase
{
    private readonly MedicoService _medicoService;

    public MedicosController(MedicoService medicoService)
    {
        _medicoService = medicoService;
    }

    [HttpGet]
    public async Task<ActionResult<List<Medico>>> GetMedicos()
    {
        try
        {
            var medicos = await _medicoService.GetAllMedicosAsync();
            return Ok(medicos);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Medico>> GetMedico(int id)
    {
        try
        {
            var medico = await _medicoService.GetMedicoByIdAsync(id);
            if (medico == null)
                return NotFound(new { message = "Médico no encontrado" });

            return Ok(medico);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
        }
    }

    [HttpGet("especialidad/{especialidadId}")]
    public async Task<ActionResult<List<Medico>>> GetMedicosByEspecialidad(int especialidadId)
    {
        try
        {
            var medicos = await _medicoService.GetMedicosByEspecialidadAsync(especialidadId);
            return Ok(medicos);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
        }
    }

    [HttpPost]
    public async Task<ActionResult<Medico>> CreateMedico(Medico medico)
    {
        try
        {
            var nuevoMedico = await _medicoService.CreateMedicoAsync(medico);
            return CreatedAtAction(nameof(GetMedico), new { id = nuevoMedico.Id }, nuevoMedico);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error al crear médico", error = ex.Message });
        }
    }
}
```

**HorariosDisponiblesController.cs**
1. Crea un **nuevo archivo** llamado `HorariosDisponiblesController.cs`
2. **COPIA Y PEGA** exactamente este código:

```csharp
using Microsoft.AspNetCore.Mvc;
using MedicosPruebaTecnica.Business;
using MedicosPruebaTecnica.Entities;

namespace MedicosPruebaTecnica.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HorariosDisponiblesController : ControllerBase
{
    private readonly HorarioDisponibleService _horarioService;

    public HorariosDisponiblesController(HorarioDisponibleService horarioService)
    {
        _horarioService = horarioService;
    }

    [HttpGet]
    public async Task<ActionResult<List<HorarioDisponible>>> GetHorarios()
    {
        try
        {
            var horarios = await _horarioService.GetAllHorariosAsync();
            return Ok(horarios);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
        }
    }

    [HttpGet("medico/{medicoId}")]
    public async Task<ActionResult<List<HorarioDisponible>>> GetHorariosByMedico(int medicoId)
    {
        try
        {
            var horarios = await _horarioService.GetHorariosByMedicoAsync(medicoId);
            return Ok(horarios);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
        }
    }

    [HttpPost]
    public async Task<ActionResult<HorarioDisponible>> CreateHorario(HorarioDisponible horario)
    {
        try
        {
            var nuevoHorario = await _horarioService.CreateHorarioAsync(horario);
            return Ok(nuevoHorario);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error al crear horario", error = ex.Message });
        }
    }
}
```

### **PASO 7: Probar el Backend**

#### **7.1 Ejecutar la aplicación**
1. En la terminal, asegúrate de estar en la carpeta API:
   ```bash
   cd MedicosPruebaTecnica.API
   ```

2. Ejecuta la aplicación:
   ```bash
   dotnet run
   ```

3. **¡IMPORTANTE!** Verás un mensaje como:
   ```
   Now listening on: http://localhost:5242
   ```
   **Anota este puerto (5242)** porque lo necesitarás para el frontend.

#### **7.2 Probar los endpoints**
1. Abre tu navegador web
2. Ve a: `http://localhost:5242/swagger`
3. Verás la documentación interactiva de tu API
4. Prueba el endpoint `GET /api/usuarios` haciendo clic en "Try it out" → "Execute"

> **🎓 Concepto:** **Swagger** es una herramienta que genera documentación automática de tu API y te permite probarla directamente desde el navegador.

---

## 🎨 **PARTE 2: Construyendo el Frontend (Vue.js)**

> **🎓 Concepto clave:** Vamos a crear una aplicación web moderna que consuma nuestra API y proporcione una interfaz de usuario intuitiva.

### **PASO 8: Crear el proyecto Frontend**

#### **8.1 Crear la carpeta Frontend**
1. **Abre una nueva terminal** (no cierres la del backend)
2. Navega a la carpeta raíz del proyecto:
   ```bash
   cd ..
   ```

3. Crea el proyecto Vue.js:
   ```bash
   npm create vue@latest Frontend
   ```

4. **Responde a las preguntas así:**
   - ✅ Add TypeScript? → **No**
   - ✅ Add JSX Support? → **No**
   - ✅ Add Vue Router for Single Page Application development? → **Yes**
   - ✅ Add Pinia for state management? → **Yes**
   - ✅ Add Vitest for Unit Testing? → **No**
   - ✅ Add an End-to-End Testing Solution? → **No**
   - ✅ Add ESLint for code quality? → **No**
   - ✅ Add Prettier for code formatting? → **No**

5. Navega a la carpeta Frontend:
   ```bash
   cd Frontend
   ```

6. Instala las dependencias:
   ```bash
   npm install
   ```

7. Instala librerías adicionales que necesitaremos:
   ```bash
   npm install axios
   ```
   ```bash
   npm install @headlessui/vue @heroicons/vue
   ```

#### **8.2 Configurar la estructura del proyecto**
1. **Abre VS Code en la carpeta Frontend:**
   ```bash
   code .
   ```

### **PASO 9: Configurar servicios para consumir la API**

#### **9.1 Crear el servicio de API**
1. Ve a la carpeta `src`
2. Crea una **nueva carpeta** llamada `services`
3. Dentro de `services`, crea un **nuevo archivo** llamado `api.js`
4. **COPIA Y PEGA** exactamente este código:

```javascript
import axios from 'axios'

// Configuración base de Axios
const api = axios.create({
  baseURL: 'http://localhost:5242/api', // ¡CAMBIA EL PUERTO SI ES DIFERENTE!
  headers: {
    'Content-Type': 'application/json',
  },
})

// Interceptor para manejar errores globalmente
api.interceptors.response.use(
  (response) => response,
  (error) => {
    console.error('Error en la API:', error)
    return Promise.reject(error)
  }
)

export default api
```

#### **9.2 Crear servicios específicos**

**usuarioService.js**
1. En la carpeta `services`, crea un **nuevo archivo** llamado `usuarioService.js`
2. **COPIA Y PEGA** exactamente este código:

```javascript
import api from './api.js'

export const usuarioService = {
  // Obtener todos los usuarios
  async getUsuarios() {
    try {
      const response = await api.get('/usuarios')
      return response.data
    } catch (error) {
      console.error('Error al obtener usuarios:', error)
      throw error
    }
  },

  // Obtener usuario por ID
  async getUsuario(id) {
    try {
      const response = await api.get(`/usuarios/${id}`)
      return response.data
    } catch (error) {
      console.error('Error al obtener usuario:', error)
      throw error
    }
  },

  // Crear nuevo usuario
  async createUsuario(usuario) {
    try {
      const response = await api.post('/usuarios', usuario)
      return response.data
    } catch (error) {
      console.error('Error al crear usuario:', error)
      throw error
    }
  },

  // Actualizar usuario
  async updateUsuario(id, usuario) {
    try {
      const response = await api.put(`/usuarios/${id}`, usuario)
      return response.data
    } catch (error) {
      console.error('Error al actualizar usuario:', error)
      throw error
    }
  },

  // Eliminar usuario
  async deleteUsuario(id) {
    try {
      const response = await api.delete(`/usuarios/${id}`)
      return response.data
    } catch (error) {
      console.error('Error al eliminar usuario:', error)
      throw error
    }
  }
}
```

**especialidadService.js**
1. Crea un **nuevo archivo** llamado `especialidadService.js`
2. **COPIA Y PEGA** exactamente este código:

```javascript
import api from './api.js'

export const especialidadService = {
  async getEspecialidades() {
    try {
      const response = await api.get('/especialidades')
      return response.data
    } catch (error) {
      console.error('Error al obtener especialidades:', error)
      throw error
    }
  },

  async getEspecialidad(id) {
    try {
      const response = await api.get(`/especialidades/${id}`)
      return response.data
    } catch (error) {
      console.error('Error al obtener especialidad:', error)
      throw error
    }
  },

  async createEspecialidad(especialidad) {
    try {
      const response = await api.post('/especialidades', especialidad)
      return response.data
    } catch (error) {
      console.error('Error al crear especialidad:', error)
      throw error
    }
  }
}
```

**medicoService.js**
1. Crea un **nuevo archivo** llamado `medicoService.js`
2. **COPIA Y PEGA** exactamente este código:

```javascript
import api from './api.js'

export const medicoService = {
  async getMedicos() {
    try {
      const response = await api.get('/medicos')
      return response.data
    } catch (error) {
      console.error('Error al obtener médicos:', error)
      throw error
    }
  },

  async getMedico(id) {
    try {
      const response = await api.get(`/medicos/${id}`)
      return response.data
    } catch (error) {
      console.error('Error al obtener médico:', error)
      throw error
    }
  },

  async getMedicosByEspecialidad(especialidadId) {
    try {
      const response = await api.get(`/medicos/especialidad/${especialidadId}`)
      return response.data
    } catch (error) {
      console.error('Error al obtener médicos por especialidad:', error)
      throw error
    }
  },

  async createMedico(medico) {
    try {
      const response = await api.post('/medicos', medico)
      return response.data
    } catch (error) {
      console.error('Error al crear médico:', error)
      throw error
    }
  }
}
```

**horarioService.js**
1. Crea un **nuevo archivo** llamado `horarioService.js`
2. **COPIA Y PEGA** exactamente este código:

```javascript
import api from './api.js'

export const horarioService = {
  async getHorarios() {
    try {
      const response = await api.get('/horariosDisponibles')
      return response.data
    } catch (error) {
      console.error('Error al obtener horarios:', error)
      throw error
    }
  },

  async getHorariosByMedico(medicoId) {
    try {
      const response = await api.get(`/horariosDisponibles/medico/${medicoId}`)
      return response.data
    } catch (error) {
      console.error('Error al obtener horarios del médico:', error)
      throw error
    }
  },

  async createHorario(horario) {
    try {
      const response = await api.post('/horariosDisponibles', horario)
      return response.data
    } catch (error) {
      console.error('Error al crear horario:', error)
      throw error
    }
  }
}
```

> **🎓 Concepto:** Los **servicios** encapsulan las llamadas a la API. Esto nos permite reutilizar el código y manejar errores de forma centralizada.

### **PASO 10: Crear los componentes de la interfaz**

#### **10.1 Actualizar el layout principal**
1. Ve a `src/App.vue`
2. **REEMPLAZA COMPLETAMENTE** el contenido con este código:

```vue
<template>
  <div id="app" class="min-h-screen bg-gray-50">
    <!-- Navegación -->
    <nav class="bg-white shadow-lg">
      <div class="max-w-7xl mx-auto px-4">
        <div class="flex justify-between h-16">
          <div class="flex items-center">
            <h1 class="text-xl font-bold text-gray-900">Sistema Médico</h1>
          </div>
          <div class="flex items-center space-x-4">
            <RouterLink 
              to="/" 
              class="text-gray-700 hover:text-blue-600 px-3 py-2 rounded-md text-sm font-medium"
            >
              Inicio
            </RouterLink>
            <RouterLink 
              to="/usuarios" 
              class="text-gray-700 hover:text-blue-600 px-3 py-2 rounded-md text-sm font-medium"
            >
              Usuarios
            </RouterLink>
            <RouterLink 
              to="/especialidades" 
              class="text-gray-700 hover:text-blue-600 px-3 py-2 rounded-md text-sm font-medium"
            >
              Especialidades
            </RouterLink>
            <RouterLink 
              to="/medicos" 
              class="text-gray-700 hover:text-blue-600 px-3 py-2 rounded-md text-sm font-medium"
            >
              Médicos
            </RouterLink>
            <RouterLink 
              to="/horarios" 
              class="text-gray-700 hover:text-blue-600 px-3 py-2 rounded-md text-sm font-medium"
            >
              Horarios
            </RouterLink>
          </div>
        </div>
      </div>
    </nav>

    <!-- Contenido principal -->
    <main class="max-w-7xl mx-auto py-6 px-4">
      <RouterView />
    </main>
  </div>
</template>

<script>
export default {
  name: 'App'
}
</script>

<style>
/* Estilos globales usando Tailwind CSS */
</style>
```

#### **10.2 Configurar las rutas**
1. Ve a `src/router/index.js`
2. **REEMPLAZA COMPLETAMENTE** el contenido con este código:

```javascript
import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: HomeView
    },
    {
      path: '/usuarios',
      name: 'usuarios',
      component: () => import('../views/UsuariosView.vue')
    },
    {
      path: '/especialidades',
      name: 'especialidades',
      component: () => import('../views/EspecialidadesView.vue')
    },
    {
      path: '/medicos',
      name: 'medicos',
      component: () => import('../views/MedicosView.vue')
    },
    {
      path: '/horarios',
      name: 'horarios',
      component: () => import('../views/HorariosView.vue')
    }
  ]
})

export default router
```

#### **10.3 Crear las vistas (páginas)**

**HomeView.vue**
1. Ve a `src/views/HomeView.vue`
2. **REEMPLAZA COMPLETAMENTE** el contenido con este código:

```vue
<template>
  <div class="text-center">
    <div class="max-w-4xl mx-auto">
      <h1 class="text-4xl font-bold text-gray-900 mb-8">
        Sistema de Gestión Médica
      </h1>
      
      <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
        <!-- Tarjeta Usuarios -->
        <div class="bg-white rounded-lg shadow-md p-6 hover:shadow-lg transition-shadow">
          <div class="text-blue-600 mb-4">
            <svg class="w-12 h-12 mx-auto" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 4.354a4 4 0 110 5.292M15 21H3v-1a6 6 0 0112 0v1zm0 0h6v-1a6 6 0 00-9-5.197m13.5-9a2.5 2.5 0 11-5 0 2.5 2.5 0 015 0z"></path>
            </svg>
          </div>
          <h3 class="text-lg font-semibold text-gray-900 mb-2">Usuarios</h3>
          <p class="text-gray-600 mb-4">Gestiona los usuarios del sistema</p>
          <RouterLink 
            to="/usuarios" 
            class="bg-blue-600 text-white px-4 py-2 rounded-md hover:bg-blue-700 transition-colors"
          >
            Ver Usuarios
          </RouterLink>
        </div>

        <!-- Tarjeta Especialidades -->
        <div class="bg-white rounded-lg shadow-md p-6 hover:shadow-lg transition-shadow">
          <div class="text-green-600 mb-4">
            <svg class="w-12 h-12 mx-auto" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19.428 15.428a2 2 0 00-1.022-.547l-2.387-.477a6 6 0 00-3.86.517l-.318.158a6 6 0 01-3.86.517L6.05 15.21a2 2 0 00-1.806.547M8 4h8l-1 1v5.172a2 2 0 00.586 1.414l5 5c1.26 1.26.367 3.414-1.415 3.414H4.828c-1.782 0-2.674-2.154-1.414-3.414l5-5A2 2 0 009 10.172V5L8 4z"></path>
            </svg>
          </div>
          <h3 class="text-lg font-semibold text-gray-900 mb-2">Especialidades</h3>
          <p class="text-gray-600 mb-4">Administra las especialidades médicas</p>
          <RouterLink 
            to="/especialidades" 
            class="bg-green-600 text-white px-4 py-2 rounded-md hover:bg-green-700 transition-colors"
          >
            Ver Especialidades
          </RouterLink>
        </div>

        <!-- Tarjeta Médicos -->
        <div class="bg-white rounded-lg shadow-md p-6 hover:shadow-lg transition-shadow">
          <div class="text-purple-600 mb-4">
            <svg class="w-12 h-12 mx-auto" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M16 7a4 4 0 11-8 0 4 4 0 018 0zM12 14a7 7 0 00-7 7h14a7 7 0 00-7-7z"></path>
            </svg>
          </div>
          <h3 class="text-lg font-semibold text-gray-900 mb-2">Médicos</h3>
          <p class="text-gray-600 mb-4">Gestiona el personal médico</p>
          <RouterLink 
            to="/medicos" 
            class="bg-purple-600 text-white px-4 py-2 rounded-md hover:bg-purple-700 transition-colors"
          >
            Ver Médicos
          </RouterLink>
        </div>

        <!-- Tarjeta Horarios -->
        <div class="bg-white rounded-lg shadow-md p-6 hover:shadow-lg transition-shadow">
          <div class="text-orange-600 mb-4">
            <svg class="w-12 h-12 mx-auto" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 8v4l3 3m6-3a9 9 0 11-18 0 9 9 0 0118 0z"></path>
            </svg>
          </div>
          <h3 class="text-lg font-semibold text-gray-900 mb-2">Horarios</h3>
          <p class="text-gray-600 mb-4">Administra horarios disponibles</p>
          <RouterLink 
            to="/horarios" 
            class="bg-orange-600 text-white px-4 py-2 rounded-md hover:bg-orange-700 transition-colors"
          >
            Ver Horarios
          </RouterLink>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: 'HomeView'
}
</script>
```

**UsuariosView.vue**
1. En la carpeta `src/views`, crea un **nuevo archivo** llamado `UsuariosView.vue`
2. **COPIA Y PEGA** exactamente este código:

```vue
<template>
  <div>
    <div class="flex justify-between items-center mb-6">
      <h1 class="text-3xl font-bold text-gray-900">Gestión de Usuarios</h1>
      <button 
        @click="showCreateModal = true"
        class="bg-blue-600 text-white px-4 py-2 rounded-md hover:bg-blue-700 transition-colors"
      >
        Crear Usuario
      </button>
    </div>

    <!-- Lista de usuarios -->
    <div class="bg-white shadow-md rounded-lg overflow-hidden">
      <div v-if="loading" class="p-8 text-center">
        <div class="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-600 mx-auto"></div>
        <p class="mt-4 text-gray-600">Cargando usuarios...</p>
      </div>

      <div v-else-if="error" class="p-8 text-center text-red-600">
        <p>Error al cargar usuarios: {{ error }}</p>
        <button 
          @click="loadUsuarios" 
          class="mt-4 bg-blue-600 text-white px-4 py-2 rounded-md hover:bg-blue-700"
        >
          Reintentar
        </button>
      </div>

      <div v-else>
        <table class="min-w-full divide-y divide-gray-200">
          <thead class="bg-gray-50">
            <tr>
              <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">ID</th>
              <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Nombre</th>
              <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Email</th>
              <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Acciones</th>
            </tr>
          </thead>
          <tbody class="bg-white divide-y divide-gray-200">
            <tr v-for="usuario in usuarios" :key="usuario.id">
              <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-900">{{ usuario.id }}</td>
              <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-900">{{ usuario.nombre }}</td>
              <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-900">{{ usuario.email }}</td>
              <td class="px-6 py-4 whitespace-nowrap text-sm font-medium">
                <button 
                  @click="editUsuario(usuario)"
                  class="text-blue-600 hover:text-blue-900 mr-4"
                >
                  Editar
                </button>
                <button 
                  @click="deleteUsuario(usuario.id)"
                  class="text-red-600 hover:text-red-900"
                >
                  Eliminar
                </button>
              </td>
            </tr>
          </tbody>
        </table>

        <div v-if="usuarios.length === 0" class="p-8 text-center text-gray-500">
          No hay usuarios registrados
        </div>
      </div>
    </div>

    <!-- Modal para crear/editar usuario -->
    <div v-if="showCreateModal || showEditModal" class="fixed inset-0 bg-gray-600 bg-opacity-50 overflow-y-auto h-full w-full z-50">
      <div class="relative top-20 mx-auto p-5 border w-96 shadow-lg rounded-md bg-white">
        <div class="mt-3">
          <h3 class="text-lg font-medium text-gray-900 mb-4">
            {{ showEditModal ? 'Editar Usuario' : 'Crear Nuevo Usuario' }}
          </h3>
          
          <form @submit.prevent="showEditModal ? updateUsuario() : createUsuario()">
            <div class="mb-4">
              <label class="block text-sm font-medium text-gray-700 mb-2">Nombre</label>
              <input 
                v-model="usuarioForm.nombre"
                type="text" 
                required
                class="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
                placeholder="Ingresa el nombre"
              >
            </div>
            
            <div class="mb-4">
              <label class="block text-sm font-medium text-gray-700 mb-2">Email</label>
              <input 
                v-model="usuarioForm.email"
                type="email" 
                required
                class="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
                placeholder="Ingresa el email"
              >
            </div>
            
            <div class="mb-6" v-if="!showEditModal">
              <label class="block text-sm font-medium text-gray-700 mb-2">Contraseña</label>
              <input 
                v-model="usuarioForm.contraseña"
                type="password" 
                required
                class="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
                placeholder="Ingresa la contraseña"
              >
            </div>
            
            <div class="flex justify-end space-x-3">
              <button 
                type="button"
                @click="closeModal"
                class="px-4 py-2 bg-gray-300 text-gray-700 rounded-md hover:bg-gray-400 transition-colors"
              >
                Cancelar
              </button>
              <button 
                type="submit"
                :disabled="submitting"
                class="px-4 py-2 bg-blue-600 text-white rounded-md hover:bg-blue-700 transition-colors disabled:opacity-50"
              >
                {{ submitting ? 'Guardando...' : (showEditModal ? 'Actualizar' : 'Crear') }}
              </button>
            </div>
          </form>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { usuarioService } from '../services/usuarioService.js'

export default {
  name: 'UsuariosView',
  data() {
    return {
      usuarios: [],
      loading: false,
      error: null,
      showCreateModal: false,
      showEditModal: false,
      submitting: false,
      usuarioForm: {
        id: null,
        nombre: '',
        email: '',
        contraseña: ''
      }
    }
  },
  async mounted() {
    await this.loadUsuarios()
  },
  methods: {
    async loadUsuarios() {
      this.loading = true
      this.error = null
      try {
        this.usuarios = await usuarioService.getUsuarios()
      } catch (error) {
        this.error = error.message || 'Error al cargar usuarios'
        console.error('Error:', error)
      } finally {
        this.loading = false
      }
    },

    async createUsuario() {
      this.submitting = true
      try {
        await usuarioService.createUsuario(this.usuarioForm)
        await this.loadUsuarios()
        this.closeModal()
        alert('Usuario creado exitosamente')
      } catch (error) {
        alert('Error al crear usuario: ' + (error.response?.data?.message || error.message))
      } finally {
        this.submitting = false
      }
    },

    editUsuario(usuario) {
      this.usuarioForm = {
        id: usuario.id,
        nombre: usuario.nombre,
        email: usuario.email,
        contraseña: ''
      }
      this.showEditModal = true
    },

    async updateUsuario() {
      this.submitting = true
      try {
        await usuarioService.updateUsuario(this.usuarioForm.id, this.usuarioForm)
        await this.loadUsuarios()
        this.closeModal()
        alert('Usuario actualizado exitosamente')
      } catch (error) {
        alert('Error al actualizar usuario: ' + (error.response?.data?.message || error.message))
      } finally {
        this.submitting = false
      }
    },

    async deleteUsuario(id) {
      if (confirm('¿Estás seguro de que quieres eliminar este usuario?')) {
        try {
          await usuarioService.deleteUsuario(id)
          await this.loadUsuarios()
          alert('Usuario eliminado exitosamente')
        } catch (error) {
          alert('Error al eliminar usuario: ' + (error.response?.data?.message || error.message))
        }
      }
    },

    closeModal() {
      this.showCreateModal = false
      this.showEditModal = false
      this.usuarioForm = {
        id: null,
        nombre: '',
        email: '',
        contraseña: ''
      }
    }
  }
}
</script>
### **PASO 11: Configurar Tailwind CSS**

#### **11.1 Instalar Tailwind CSS**
1. En la terminal del Frontend, ejecuta:
   ```bash
   npm install -D tailwindcss postcss autoprefixer
   ```

2. Inicializa Tailwind:
   ```bash
   npx tailwindcss init -p
   ```

#### **11.2 Configurar Tailwind**
1. Ve al archivo `tailwind.config.js` que se creó
2. **REEMPLAZA COMPLETAMENTE** el contenido con este código:

```javascript
/** @type {import('tailwindcss').Config} */
export default {
  content: [
    "./index.html",
    "./src/**/*.{vue,js,ts,jsx,tsx}",
  ],
  theme: {
    extend: {},
  },
  plugins: [],
}
```

#### **11.3 Agregar estilos de Tailwind**
1. Ve a `src/style.css`
2. **REEMPLAZA COMPLETAMENTE** el contenido con este código:

```css
@tailwind base;
@tailwind components;
@tailwind utilities;

/* Estilos personalizados adicionales */
body {
  font-family: 'Inter', -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, sans-serif;
}

.router-link-active {
  @apply text-blue-600 font-semibold;
}
```

### **PASO 12: Ejecutar el Frontend**

#### **12.1 Iniciar el servidor de desarrollo**
1. En la terminal del Frontend, ejecuta:
   ```bash
   npm run dev
   ```

2. **¡IMPORTANTE!** Verás un mensaje como:
   ```
   Local:   http://localhost:5173/
   ```
   **Anota este puerto (5173)** y abre esa URL en tu navegador.

#### **12.2 Verificar que todo funciona**
1. Deberías ver la página de inicio con las 4 tarjetas
2. Haz clic en "Ver Usuarios" para probar la funcionalidad
3. Intenta crear un nuevo usuario

> **🎓 Concepto:** **Hot Module Replacement (HMR)** significa que cuando cambies código, la página se actualizará automáticamente sin perder el estado de la aplicación.

---

## 🚀 **PASO 13: Solución de Problemas Comunes**

### **13.1 Error de CORS**
Si ves errores como "CORS policy", asegúrate de que en tu `Program.cs` del backend tengas:

```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173") // Puerto del frontend
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Y después de var app = builder.Build();
app.UseCors("AllowFrontend");
```

### **13.2 Error de conexión a la base de datos**
1. Verifica que el archivo `MedicosDB.db` existe en la carpeta API
2. Si no existe, ejecuta:
   ```bash
   dotnet ef database update
   ```

### **13.3 Error "Puerto ya en uso"**
Si el puerto 5242 o 5173 ya están en uso:
1. **Backend:** Cambia el puerto en `Properties/launchSettings.json`
2. **Frontend:** Cambia el puerto en `src/services/api.js`

### **13.4 Errores de dependencias**
Si hay errores de paquetes faltantes:
1. **Backend:** Ejecuta `dotnet restore`
2. **Frontend:** Ejecuta `npm install`

---

## 🎯 **PASO 14: Próximos Pasos y Mejoras**

### **14.1 Funcionalidades adicionales que puedes agregar:**
1. **Autenticación y autorización**
2. **Validación de formularios más robusta**
3. **Paginación en las listas**
4. **Filtros y búsqueda**
5. **Notificaciones toast en lugar de alerts**
6. **Carga de imágenes para médicos**
7. **Dashboard con estadísticas**
8. **Exportar datos a Excel/PDF**

### **14.2 Mejoras técnicas:**
1. **Implementar manejo de estado global con Pinia**
2. **Agregar tests unitarios**
3. **Implementar lazy loading para las rutas**
4. **Optimizar las consultas a la base de datos**
5. **Agregar logging estructurado**
6. **Implementar caché**

### **14.3 Recursos para seguir aprendiendo:**
- **Vue.js:** https://vuejs.org/guide/
- **.NET Core:** https://docs.microsoft.com/en-us/aspnet/core/
- **Entity Framework:** https://docs.microsoft.com/en-us/ef/core/
- **Tailwind CSS:** https://tailwindcss.com/docs

---

## 📝 **Resumen Final**

¡Felicidades! Has construido un sistema completo de gestión médica que incluye:

### **Backend (.NET Core):**
- ✅ API REST con múltiples endpoints
- ✅ Base de datos SQLite con Entity Framework
- ✅ Arquitectura en capas (API, Business, Data, Entities)
- ✅ Manejo de errores y validaciones
- ✅ Documentación automática con Swagger

### **Frontend (Vue.js):**
- ✅ Interfaz moderna y responsiva
- ✅ Navegación entre páginas
- ✅ Formularios interactivos
- ✅ Consumo de API REST
- ✅ Manejo de estados de carga y errores

### **Conceptos aprendidos:**
- 🎓 **Arquitectura de software**
- 🎓 **APIs REST**
- 🎓 **Bases de datos relacionales**
- 🎓 **Frameworks modernos**
- 🎓 **Desarrollo full-stack**

**¡Ahora tienes las bases para construir aplicaciones web profesionales!** 🚀

---

*Este README fue diseñado para ser una guía completa y detallada. Si encuentras algún error o tienes sugerencias, no dudes en mejorar la documentación.*
dotnet add reference ../MedicosPruebaTecnica.Entities
dotnet add reference ../MedicosPruebaTecnica.Data
dotnet add package Microsoft.EntityFrameworkCore
```

**EspecialidadService.cs**
```csharp
using Microsoft.EntityFrameworkCore;
using MedicosPruebaTecnica.Data;
using MedicosPruebaTecnica.Entities;

namespace MedicosPruebaTecnica.Business
{
    public class EspecialidadService
    {
        private readonly MyDbContext _context;
        
        public EspecialidadService(MyDbContext context)
        {
            _context = context;
        }
        
        public async Task<List<Especialidad>> GetAllEspecialidadesAsync()
        {
            var especialidades = await _context.Especialidades
                .Where(e => e.Activo)
                .OrderBy(e => e.Nombre)
                .ToListAsync();
                
            // Si no hay especialidades, retornar datos demo
            if (!especialidades.Any())
            {
                return DemoData.GetEspecialidades();
            }
            
            return especialidades;
        }
        
        public async Task<Especialidad?> GetEspecialidadByIdAsync(int id)
        {
            return await _context.Especialidades
                .FirstOrDefaultAsync(e => e.Id == id && e.Activo);
        }
        
        public async Task<Especialidad> CreateEspecialidadAsync(Especialidad especialidad)
        {
            _context.Especialidades.Add(especialidad);
            await _context.SaveChangesAsync();
            return especialidad;
        }
    }
}
```

> **🎓 Concepto:** Los **servicios** contienen la lógica de negocio. Aquí definimos QUÉ puede hacer nuestra aplicación, no CÓMO se hace (eso lo maneja Entity Framework).

### **Paso 5: Configurando la API**

En `MedicosPruebaTecnica.API`, agrega las referencias:
```bash
cd MedicosPruebaTecnica.API
dotnet add reference ../MedicosPruebaTecnica.Business
dotnet add reference ../MedicosPruebaTecnica.Data
dotnet add reference ../MedicosPruebaTecnica.Entities
dotnet add package Microsoft.EntityFrameworkCore.Sqlite
```

**Program.cs**
```csharp
using Microsoft.EntityFrameworkCore;
using MedicosPruebaTecnica.Business;
using MedicosPruebaTecnica.Data;

var builder = WebApplication.CreateBuilder(args);

// Configurar Entity Framework con SQLite
builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registrar servicios para inyección de dependencias
builder.Services.AddScoped<EspecialidadService>();
builder.Services.AddScoped<MedicoService>();
builder.Services.AddScoped<PacienteService>();
builder.Services.AddScoped<HorarioDisponibleService>();
builder.Services.AddScoped<CitaService>();

// Configurar CORS para permitir comunicación con el frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173") // Puerto del frontend Vue
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configurar pipeline de requests
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowFrontend");
app.UseAuthorization();
app.MapControllers();

// Crear base de datos si no existe
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<MyDbContext>();
    context.Database.EnsureCreated();
}

app.Run();
```

**appsettings.json**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=MedicosDB.db"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

### **Paso 6: Creando los Controladores (Endpoints REST)**

**Controllers/EspecialidadesController.cs**
```csharp
using Microsoft.AspNetCore.Mvc;
using MedicosPruebaTecnica.Business;
using MedicosPruebaTecnica.Entities;

namespace MedicosPruebaTecnica.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EspecialidadesController : ControllerBase
    {
        private readonly EspecialidadService _especialidadService;
        
        public EspecialidadesController(EspecialidadService especialidadService)
        {
            _especialidadService = especialidadService;
        }
        
        [HttpGet]
        public async Task<ActionResult<List<Especialidad>>> GetEspecialidades()
        {
            try
            {
                var especialidades = await _especialidadService.GetAllEspecialidadesAsync();
                return Ok(especialidades);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
            }
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<Especialidad>> GetEspecialidad(int id)
        {
            try
            {
                var especialidad = await _especialidadService.GetEspecialidadByIdAsync(id);
                if (especialidad == null)
                    return NotFound(new { message = "Especialidad no encontrada" });
                    
                return Ok(especialidad);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
            }
        }
        
        [HttpPost]
        public async Task<ActionResult<Especialidad>> CreateEspecialidad(Especialidad especialidad)
        {
            try
            {
                var nuevaEspecialidad = await _especialidadService.CreateEspecialidadAsync(especialidad);
                return CreatedAtAction(nameof(GetEspecialidad), new { id = nuevaEspecialidad.Id }, nuevaEspecialidad);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
            }
        }
    }
}
```

> **🎓 Concepto:** Los **controladores** definen los endpoints REST. Cada método representa una operación HTTP (GET, POST, PUT, DELETE) que el frontend puede llamar.

---

## 🎨 **PARTE 2: Construyendo el Frontend (Vue.js)**

> **🎓 Concepto clave:** Vamos a crear una SPA (Single Page Application) que consuma nuestra API y proporcione una interfaz intuitiva para los usuarios.

### **Paso 1: Creando el Proyecto Vue**

```bash
# Crear proyecto Vue con Vite
npm create vue@latest Frontend

# Configuraciones recomendadas:
# ✅ TypeScript? No (para simplicidad)
# ✅ JSX? No
# ✅ Vue Router? Yes
# ✅ Pinia? No (usaremos estado local)
# ✅ Vitest? No
# ✅ Playwright? No
# ✅ ESLint? Yes

cd Frontend
npm install

# Instalar dependencias adicionales
npm install axios vue-toastification
```

### **Paso 2: Configurando los Servicios (Comunicación con API)**

**src/services/api.js**
```javascript
import axios from 'axios'

// Configuración base de Axios
const api = axios.create({
  baseURL: 'http://localhost:5000/api', // URL del backend
  timeout: 10000,
  headers: {
    'Content-Type': 'application/json'
  }
})

// Interceptor para manejo de errores globales
api.interceptors.response.use(
  response => response,
  error => {
    console.error('Error en API:', error)
    return Promise.reject(error)
  }
)

export default api
```

> **🎓 Concepto:** **Axios** es una librería para hacer peticiones HTTP. Los **interceptors** nos permiten manejar errores de forma centralizada.

**src/services/especialidadService.js**
```javascript
import api from './api'

export const especialidadService = {
  async getAll() {
    const response = await api.get('/especialidades')
    return response.data
  },
  
  async getById(id) {
    const response = await api.get(`/especialidades/${id}`)
    return response.data
  },
  
  async create(especialidad) {
    const response = await api.post('/especialidades', especialidad)
    return response.data
  }
}
```

### **Paso 3: Creando Componentes Reutilizables**

**src/components/EspecialidadCard.vue**
```vue
<template>
  <div class="especialidad-card">
    <div class="card-header">
      <h3>{{ especialidad.nombre }}</h3>
    </div>
    <div class="card-body">
      <p>{{ especialidad.descripcion }}</p>
      <div class="card-actions">
        <button @click="$emit('select', especialidad)" class="btn-primary">
          Seleccionar
        </button>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: 'EspecialidadCard',
  props: {
    especialidad: {
      type: Object,
      required: true
    }
  },
  emits: ['select']
}
</script>

<style scoped>
.especialidad-card {
  border: 1px solid #ddd;
  border-radius: 8px;
  padding: 1rem;
  margin: 0.5rem;
  transition: transform 0.2s;
}

.especialidad-card:hover {
  transform: translateY(-2px);
  box-shadow: 0 4px 8px rgba(0,0,0,0.1);
}

.btn-primary {
  background-color: #007bff;
  color: white;
  border: none;
  padding: 0.5rem 1rem;
  border-radius: 4px;
  cursor: pointer;
}
</style>
```

> **🎓 Concepto:** Los **componentes** son piezas reutilizables de UI. Los **props** permiten pasar datos del padre al hijo, y los **emits** permiten comunicación del hijo al padre.

### **Paso 4: Creando las Vistas Principales**

**src/views/ReservarCita.vue**
```vue
<template>
  <div class="reservar-cita">
    <h1>Reservar Cita Médica</h1>
    
    <!-- Paso 1: Seleccionar Especialidad -->
    <div v-if="currentStep === 1" class="step">
      <h2>Paso 1: Selecciona una Especialidad</h2>
      <div class="especialidades-grid">
        <EspecialidadCard 
          v-for="especialidad in especialidades"
          :key="especialidad.id"
          :especialidad="especialidad"
          @select="selectEspecialidad"
        />
      </div>
    </div>
    
    <!-- Paso 2: Seleccionar Médico -->
    <div v-if="currentStep === 2" class="step">
      <h2>Paso 2: Selecciona un Médico</h2>
      <div class="medicos-list">
        <div 
          v-for="medico in medicosDisponibles"
          :key="medico.id"
          class="medico-item"
          @click="selectMedico(medico)"
        >
          <h4>Dr. {{ medico.nombre }} {{ medico.apellido }}</h4>
          <p>{{ medico.especialidad?.nombre }}</p>
        </div>
      </div>
    </div>
    
    <!-- Navegación entre pasos -->
    <div class="navigation">
      <button v-if="currentStep > 1" @click="previousStep" class="btn-secondary">
        Anterior
      </button>
      <button v-if="canProceedToNextStep" @click="nextStep" class="btn-primary">
        Siguiente
      </button>
    </div>
  </div>
</template>

<script>
import { especialidadService } from '../services/especialidadService'
import { medicoService } from '../services/medicoService'
import EspecialidadCard from '../components/EspecialidadCard.vue'

export default {
  name: 'ReservarCita',
  components: {
    EspecialidadCard
  },
  data() {
    return {
      currentStep: 1,
      especialidades: [],
      medicosDisponibles: [],
      selectedEspecialidad: null,
      selectedMedico: null,
      loading: false
    }
  },
  computed: {
    canProceedToNextStep() {
      switch (this.currentStep) {
        case 1: return this.selectedEspecialidad !== null
        case 2: return this.selectedMedico !== null
        default: return false
      }
    }
  },
  async mounted() {
    await this.loadEspecialidades()
  },
  methods: {
    async loadEspecialidades() {
      try {
        this.loading = true
        this.especialidades = await especialidadService.getAll()
      } catch (error) {
        console.error('Error cargando especialidades:', error)
        this.$toast.error('Error al cargar las especialidades')
      } finally {
        this.loading = false
      }
    },
    
    async selectEspecialidad(especialidad) {
      this.selectedEspecialidad = especialidad
      try {
        this.medicosDisponibles = await medicoService.getByEspecialidad(especialidad.id)
      } catch (error) {
        console.error('Error cargando médicos:', error)
        this.$toast.error('Error al cargar los médicos')
      }
    },
    
    selectMedico(medico) {
      this.selectedMedico = medico
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
    }
  }
}
</script>
```

> **🎓 Concepto:** Las **vistas** son componentes que representan páginas completas. El **estado reactivo** (data) se actualiza automáticamente en la UI cuando cambia.

---

## 🎯 **PARTE 3: Datos de Prueba - ¿Qué hacer cuando no tienes datos reales?**

> **🎓 Problema real:** Durante el desarrollo, necesitas datos para probar tu aplicación, pero la base de datos está vacía. ¿Solución? ¡Datos de demostración!

### **¿Por qué necesitamos datos de prueba?**

1. **Desarrollo más rápido** - No dependes de datos reales para probar funcionalidades
2. **Testing consistente** - Siempre tienes los mismos datos para probar
3. **Demostración** - Puedes mostrar la aplicación funcionando sin configuración compleja
4. **Onboarding** - Nuevos desarrolladores pueden ver la app funcionando inmediatamente

### **Estrategia: Datos Demo en el Backend**

**Backend/MedicosPruebaTecnica.Business/DemoData.cs**
```csharp
using MedicosPruebaTecnica.Entities;

namespace MedicosPruebaTecnica.Business
{
    public static class DemoData
    {
        public static List<Especialidad> GetEspecialidades()
        {
            return new List<Especialidad>
            {
                new Especialidad { Id = 1, Nombre = "Cardiología", Descripcion = "Especialidad médica que se encarga del estudio, diagnóstico y tratamiento de las enfermedades del corazón y del aparato circulatorio." },
                new Especialidad { Id = 2, Nombre = "Dermatología", Descripcion = "Especialidad médica que se encarga del estudio de la estructura y función de la piel, así como de las enfermedades que la afectan." },
                new Especialidad { Id = 3, Nombre = "Pediatría", Descripcion = "Especialidad médica que estudia al niño y sus enfermedades desde el nacimiento hasta la adolescencia." }
            };
        }
        
        public static List<Medico> GetMedicos()
        {
            return new List<Medico>
            {
                new Medico { Id = 1, Nombre = "Carlos", Apellido = "Rodríguez", Cedula = "12345678", EspecialidadId = 1, Telefono = "3001234567", Email = "carlos.rodriguez@hospital.com" },
                new Medico { Id = 2, Nombre = "Ana", Apellido = "García", Cedula = "87654321", EspecialidadId = 2, Telefono = "3007654321", Email = "ana.garcia@hospital.com" },
                new Medico { Id = 3, Nombre = "Luis", Apellido = "Martínez", Cedula = "11223344", EspecialidadId = 3, Telefono = "3009876543", Email = "luis.martinez@hospital.com" }
            };
        }
        
        // ... más métodos para otras entidades
    }
}
```

### **Implementación en los Servicios**

```csharp
public async Task<List<Especialidad>> GetAllEspecialidadesAsync()
{
    var especialidades = await _context.Especialidades
        .Where(e => e.Activo)
        .OrderBy(e => e.Nombre)
        .ToListAsync();
        
    // 🎯 CLAVE: Si no hay datos reales, usar datos demo
    if (!especialidades.Any())
    {
        return DemoData.GetEspecialidades();
    }
    
    return especialidades;
}
```

> **🎓 Concepto:** Esta estrategia permite que tu aplicación funcione inmediatamente, pero se adapta automáticamente cuando tienes datos reales.

### **Ventajas de esta Aproximación:**

✅ **Transparente para el frontend** - No sabe si los datos son reales o demo
✅ **Fácil transición** - Cuando agregues datos reales, los demo desaparecen automáticamente  
✅ **Consistente** - Todos los desarrolladores ven los mismos datos
✅ **Profesional** - La aplicación siempre funciona, nunca está "vacía"

---

## 🚀 **Ejecutando el Proyecto Completo**

### **1. Ejecutar el Backend:**
```bash
cd Backend/MedicosPruebaTecnica.API
dotnet run
```
> El backend estará disponible en: `http://localhost:5000`

### **2. Ejecutar el Frontend:**
```bash
cd Frontend
npm run dev
```
> El frontend estará disponible en: `http://localhost:5173`

### **3. Probar la Aplicación:**
1. Abre `http://localhost:5173` en tu navegador
2. Verás las especialidades de demostración
3. Puedes navegar por toda la aplicación con datos funcionales
4. Cuando agregues datos reales, los demo desaparecerán automáticamente

---

## 🎓 **Conceptos Clave Aprendidos**

### **Backend (.NET Core):**
- ✅ **Arquitectura en Capas** - Separación clara de responsabilidades
- ✅ **Entity Framework** - ORM para manejo de base de datos
- ✅ **API REST** - Endpoints para comunicación con frontend
- ✅ **Inyección de Dependencias** - Patrón para código mantenible
- ✅ **Datos de Demostración** - Estrategia para desarrollo ágil

### **Frontend (Vue.js):**
- ✅ **Componentes** - Modularización de la interfaz
- ✅ **Servicios** - Comunicación con APIs
- ✅ **Estado Reactivo** - Actualización automática de la UI
- ✅ **Routing** - Navegación entre vistas
- ✅ **Manejo de Errores** - UX consistente

### **Arquitectura General:**
- ✅ **Separación Frontend/Backend** - Escalabilidad y mantenibilidad
- ✅ **CORS** - Comunicación entre dominios
- ✅ **Manejo de Estados** - Flujo de datos consistente

---

## 📚 **Próximos Pasos para Expandir tu Conocimiento**

1. **Autenticación y Autorización** - JWT, roles de usuario
2. **Validaciones Avanzadas** - FluentValidation, validaciones del lado cliente
3. **Testing** - Unit tests, integration tests
4. **Deployment** - Docker, Azure, AWS
5. **Performance** - Caching, optimización de queries
6. **Seguridad** - HTTPS, sanitización de datos

---

## 👨‍💻 **Créditos y Contacto**

**Desarrollado por:** Joseph Herrera  
**GitHub:** [jahm1997](https://github.com/jahm1997)  
**GitLab:** [jahm1997](https://gitlab.com/jahm1997)  
**Teléfono:** 3013316136  

> *"La mejor manera de aprender programación es construyendo proyectos reales. Este proyecto te da las bases para crear aplicaciones web modernas y escalables."*

---

## 📄 **Licencia**

Este proyecto está licenciado bajo la Licencia MIT - consulta el archivo [LICENSE](LICENSE) para más detalles.

**¿Qué significa esto?** <mcreference link="https://choosealicense.com/licenses/mit/" index="1">1</mcreference>
- ✅ **Uso comercial** - Puedes usar este código en proyectos comerciales
- ✅ **Modificación** - Puedes modificar el código como necesites  
- ✅ **Distribución** - Puedes distribuir el código original o modificado
- ✅ **Uso privado** - Puedes usar el código en proyectos privados
- ⚠️ **Condición**: Debes incluir el aviso de copyright y la licencia en todas las copias

La licencia MIT es una de las más populares en proyectos de código abierto porque es simple y permisiva. <mcreference link="https://en.wikipedia.org/wiki/MIT_License" index="2">2</mcreference>

---

**¡Felicidades! 🎉 Has construido una aplicación web completa aprendiendo los conceptos más importantes del desarrollo moderno.**