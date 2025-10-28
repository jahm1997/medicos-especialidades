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

        // Cita -> Especialidad (Many-to-One)
        modelBuilder.Entity<Cita>()
            .HasOne(c => c.Especialidad)
            .WithMany()
            .HasForeignKey(c => c.EspecialidadId)
            .OnDelete(DeleteBehavior.Restrict);

        // Cita -> HorarioDisponible (Many-to-One, opcional)
        modelBuilder.Entity<Cita>()
            .HasOne(c => c.HorarioDisponible)
            .WithMany(h => h.Citas)
            .HasForeignKey(c => c.HorarioDisponibleId)
            .OnDelete(DeleteBehavior.SetNull);

        // Índices únicos
        modelBuilder.Entity<Paciente>()
            .HasIndex(p => p.Cedula)
            .IsUnique();

        modelBuilder.Entity<Paciente>()
            .HasIndex(p => p.Email)
            .IsUnique();

        modelBuilder.Entity<Usuario>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<Especialidad>()
            .HasIndex(e => e.Nombre)
            .IsUnique();

        // Configuración de precisión para TimeSpan
        modelBuilder.Entity<HorarioDisponible>()
            .Property(h => h.HoraInicio)
            .HasColumnType("time");

        modelBuilder.Entity<HorarioDisponible>()
            .Property(h => h.HoraFin)
            .HasColumnType("time");

        // Datos semilla para Especialidades
        var fechaCreacionSemilla = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        modelBuilder.Entity<Especialidad>().HasData(
            new Especialidad { Id = 1, Nombre = "Medicina General", Descripcion = "Atención médica general y preventiva", Activa = true, FechaCreacion = fechaCreacionSemilla },
            new Especialidad { Id = 2, Nombre = "Cardiología", Descripcion = "Especialidad en enfermedades del corazón", Activa = true, FechaCreacion = fechaCreacionSemilla },
            new Especialidad { Id = 3, Nombre = "Dermatología", Descripcion = "Especialidad en enfermedades de la piel", Activa = true, FechaCreacion = fechaCreacionSemilla },
            new Especialidad { Id = 4, Nombre = "Pediatría", Descripcion = "Atención médica para niños y adolescentes", Activa = true, FechaCreacion = fechaCreacionSemilla },
            new Especialidad { Id = 5, Nombre = "Ginecología", Descripcion = "Especialidad en salud femenina", Activa = true, FechaCreacion = fechaCreacionSemilla },
            new Especialidad { Id = 6, Nombre = "Neurología", Descripcion = "Especialidad en enfermedades del sistema nervioso", Activa = true, FechaCreacion = fechaCreacionSemilla },
            new Especialidad { Id = 7, Nombre = "Traumatología", Descripcion = "Especialidad en lesiones del sistema musculoesquelético", Activa = true, FechaCreacion = fechaCreacionSemilla }
        );
    }
}
