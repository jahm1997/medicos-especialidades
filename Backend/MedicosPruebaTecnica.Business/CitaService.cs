using Microsoft.EntityFrameworkCore;
using MedicosPruebaTecnica.Data;
using MedicosPruebaTecnica.Entities;

namespace MedicosPruebaTecnica.Business;

public class CitaService
{
    private readonly MyDbContext _context;

    public CitaService(MyDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Cita>> GetAllCitasAsync()
    {
        var citas = await _context.Citas
            .Include(c => c.Paciente)
            .Include(c => c.Medico)
            .Include(c => c.Especialidad)
            .Include(c => c.HorarioDisponible)
            .OrderByDescending(c => c.FechaHora)
            .ToListAsync();

        // Si no hay citas en la base de datos, devolver datos de demostración
        if (!citas.Any())
        {
            var demoCitas = DemoData.GetCitas();
            var demoPacientes = DemoData.GetPacientes();
            var demoMedicos = DemoData.GetMedicos();
            var demoEspecialidades = DemoData.GetEspecialidades();
            var demoHorarios = DemoData.GetHorariosDisponibles();

            // Asociar las entidades relacionadas
            foreach (var cita in demoCitas)
            {
                cita.Paciente = demoPacientes.FirstOrDefault(p => p.Id == cita.PacienteId);
                cita.Medico = demoMedicos.FirstOrDefault(m => m.Id == cita.MedicoId);
                cita.Especialidad = demoEspecialidades.FirstOrDefault(e => e.Id == cita.EspecialidadId);
                cita.HorarioDisponible = demoHorarios.FirstOrDefault(h => h.Id == cita.HorarioDisponibleId);
            }

            return demoCitas.OrderByDescending(c => c.FechaHora);
        }

        return citas;
    }

    public async Task<Cita?> GetCitaByIdAsync(int id)
    {
        return await _context.Citas
            .Include(c => c.Paciente)
            .Include(c => c.Medico)
            .Include(c => c.Especialidad)
            .Include(c => c.HorarioDisponible)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<IEnumerable<Cita>> GetCitasByPacienteAsync(int pacienteId)
    {
        var citas = await _context.Citas
            .Include(c => c.Medico)
            .Include(c => c.Especialidad)
            .Include(c => c.HorarioDisponible)
            .Where(c => c.PacienteId == pacienteId)
            .OrderByDescending(c => c.FechaHora)
            .ToListAsync();

        // Si no hay citas en la base de datos, devolver datos de demostración
        if (!citas.Any())
        {
            var demoCitas = DemoData.GetCitas().Where(c => c.PacienteId == pacienteId);
            var demoMedicos = DemoData.GetMedicos();
            var demoEspecialidades = DemoData.GetEspecialidades();
            var demoHorarios = DemoData.GetHorariosDisponibles();

            // Asociar las entidades relacionadas
            foreach (var cita in demoCitas)
            {
                cita.Medico = demoMedicos.FirstOrDefault(m => m.Id == cita.MedicoId);
                cita.Especialidad = demoEspecialidades.FirstOrDefault(e => e.Id == cita.EspecialidadId);
                cita.HorarioDisponible = demoHorarios.FirstOrDefault(h => h.Id == cita.HorarioDisponibleId);
            }

            return demoCitas.OrderByDescending(c => c.FechaHora);
        }

        return citas;
    }

    public async Task<IEnumerable<Cita>> GetCitasByMedicoAsync(int medicoId)
    {
        var citas = await _context.Citas
            .Include(c => c.Paciente)
            .Include(c => c.Especialidad)
            .Include(c => c.HorarioDisponible)
            .Where(c => c.MedicoId == medicoId)
            .OrderByDescending(c => c.FechaHora)
            .ToListAsync();

        // Si no hay citas en la base de datos, devolver datos de demostración
        if (!citas.Any())
        {
            var demoCitas = DemoData.GetCitas().Where(c => c.MedicoId == medicoId);
            var demoPacientes = DemoData.GetPacientes();
            var demoEspecialidades = DemoData.GetEspecialidades();
            var demoHorarios = DemoData.GetHorariosDisponibles();

            // Asociar las entidades relacionadas
            foreach (var cita in demoCitas)
            {
                cita.Paciente = demoPacientes.FirstOrDefault(p => p.Id == cita.PacienteId);
                cita.Especialidad = demoEspecialidades.FirstOrDefault(e => e.Id == cita.EspecialidadId);
                cita.HorarioDisponible = demoHorarios.FirstOrDefault(h => h.Id == cita.HorarioDisponibleId);
            }

            return demoCitas.OrderByDescending(c => c.FechaHora);
        }

        return citas;
    }

    public async Task<IEnumerable<Cita>> GetCitasByFechaAsync(DateTime fecha)
    {
        return await _context.Citas
            .Include(c => c.Paciente)
            .Include(c => c.Medico)
            .Include(c => c.Especialidad)
            .Include(c => c.HorarioDisponible)
            .Where(c => c.FechaHora.Date == fecha.Date)
            .OrderBy(c => c.FechaHora)
            .ToListAsync();
    }

    public async Task<Cita> CreateCitaAsync(Cita cita)
    {
        // Verificar que el paciente existe y está activo
        var paciente = await _context.Pacientes.FindAsync(cita.PacienteId);
        if (paciente == null || !paciente.Activo)
        {
            throw new InvalidOperationException("El paciente no existe o no está activo");
        }

        // Verificar que el médico existe y está activo
        var medico = await _context.Medicos.FindAsync(cita.MedicoId);
        if (medico == null || !medico.Activo)
        {
            throw new InvalidOperationException("El médico no existe o no está activo");
        }

        // Verificar que la especialidad existe y está activa
        var especialidad = await _context.Especialidades.FindAsync(cita.EspecialidadId);
        if (especialidad == null || !especialidad.Activa)
        {
            throw new InvalidOperationException("La especialidad no existe o no está activa");
        }

        // Verificar que el médico pertenece a la especialidad
        if (medico.EspecialidadId != cita.EspecialidadId)
        {
            throw new InvalidOperationException("El médico no pertenece a la especialidad seleccionada");
        }

        // Verificar que el horario existe y está disponible
        var horario = await _context.HorariosDisponibles.FindAsync(cita.HorarioDisponibleId);
        if (horario == null || !horario.Disponible)
        {
            throw new InvalidOperationException("El horario no existe o no está disponible");
        }

        // Verificar que el horario pertenece al médico
        if (horario.MedicoId != cita.MedicoId)
        {
            throw new InvalidOperationException("El horario no pertenece al médico seleccionado");
        }

        // Verificar que no hay otra cita en el mismo horario
        var citaExistente = await _context.Citas
            .AnyAsync(c => c.HorarioDisponibleId == cita.HorarioDisponibleId && 
                          c.Estado != "Cancelada");

        if (citaExistente)
        {
            throw new InvalidOperationException("Ya existe una cita programada para este horario");
        }

        // Verificar que el paciente no tenga otra cita el mismo día con el mismo médico
        var citaMismoDia = await _context.Citas
            .AnyAsync(c => c.PacienteId == cita.PacienteId && 
                          c.MedicoId == cita.MedicoId &&
                          c.FechaHora.Date == horario.Fecha.Date &&
                          c.Estado != "Cancelada");

        if (citaMismoDia)
        {
            throw new InvalidOperationException("El paciente ya tiene una cita programada con este médico para el mismo día");
        }

        // Establecer la fecha y hora de la cita basada en el horario
        cita.FechaHora = horario.Fecha.Add(horario.HoraInicio);
        cita.Estado = "Programada";
        cita.FechaCreacion = DateTime.Now;
        cita.FechaModificacion = DateTime.Now;

        _context.Citas.Add(cita);

        // Marcar el horario como no disponible
        horario.Disponible = false;

        await _context.SaveChangesAsync();
        return cita;
    }

    public async Task<Cita> UpdateCitaAsync(int id, Cita cita)
    {
        var existingCita = await _context.Citas.FindAsync(id);
        if (existingCita == null)
        {
            throw new KeyNotFoundException("Cita no encontrada");
        }

        // Solo permitir actualizar ciertos campos
        existingCita.Motivo = cita.Motivo;
        existingCita.Observaciones = cita.Observaciones;
        existingCita.Estado = cita.Estado;
        existingCita.FechaModificacion = DateTime.Now;

        // Si se cancela la cita, liberar el horario
        if (cita.Estado == "Cancelada" && existingCita.Estado != "Cancelada")
        {
            var horario = await _context.HorariosDisponibles.FindAsync(existingCita.HorarioDisponibleId);
            if (horario != null)
            {
                horario.Disponible = true;
            }
        }

        await _context.SaveChangesAsync();
        return existingCita;
    }

    public async Task<bool> CancelarCitaAsync(int id, string motivo = "")
    {
        var cita = await _context.Citas.FindAsync(id);
        if (cita == null)
        {
            return false;
        }

        if (cita.Estado == "Cancelada")
        {
            throw new InvalidOperationException("La cita ya está cancelada");
        }

        if (cita.Estado == "Completada")
        {
            throw new InvalidOperationException("No se puede cancelar una cita completada");
        }

        cita.Estado = "Cancelada";
        cita.Observaciones = string.IsNullOrEmpty(motivo) ? cita.Observaciones : $"{cita.Observaciones}\nMotivo de cancelación: {motivo}";
        cita.FechaModificacion = DateTime.Now;

        // Liberar el horario
        var horario = await _context.HorariosDisponibles.FindAsync(cita.HorarioDisponibleId);
        if (horario != null)
        {
            horario.Disponible = true;
        }

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> CompletarCitaAsync(int id, string observaciones = "")
    {
        var cita = await _context.Citas.FindAsync(id);
        if (cita == null)
        {
            return false;
        }

        if (cita.Estado == "Cancelada")
        {
            throw new InvalidOperationException("No se puede completar una cita cancelada");
        }

        if (cita.Estado == "Completada")
        {
            throw new InvalidOperationException("La cita ya está completada");
        }

        cita.Estado = "Completada";
        if (!string.IsNullOrEmpty(observaciones))
        {
            cita.Observaciones = string.IsNullOrEmpty(cita.Observaciones) ? observaciones : $"{cita.Observaciones}\n{observaciones}";
        }
        cita.FechaModificacion = DateTime.Now;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteCitaAsync(int id)
    {
        var cita = await _context.Citas.FindAsync(id);
        if (cita == null)
        {
            return false;
        }

        // Solo permitir eliminar citas canceladas
        if (cita.Estado != "Cancelada")
        {
            throw new InvalidOperationException("Solo se pueden eliminar citas canceladas");
        }

        _context.Citas.Remove(cita);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<Cita>> GetCitasPendientesAsync()
    {
        return await _context.Citas
            .Include(c => c.Paciente)
            .Include(c => c.Medico)
            .Include(c => c.Especialidad)
            .Include(c => c.HorarioDisponible)
            .Where(c => c.Estado == "Programada" && c.FechaHora >= DateTime.Now)
            .OrderBy(c => c.FechaHora)
            .ToListAsync();
    }

    public async Task<IEnumerable<Cita>> GetCitasDelDiaAsync(DateTime fecha)
    {
        return await _context.Citas
            .Include(c => c.Paciente)
            .Include(c => c.Medico)
            .Include(c => c.Especialidad)
            .Include(c => c.HorarioDisponible)
            .Where(c => c.FechaHora.Date == fecha.Date && c.Estado != "Cancelada")
            .OrderBy(c => c.FechaHora)
            .ToListAsync();
    }
}