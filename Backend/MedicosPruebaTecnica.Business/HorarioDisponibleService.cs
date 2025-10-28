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

    public async Task<IEnumerable<HorarioDisponible>> GetAllHorariosAsync()
    {
        try
        {
            var horarios = await _context.HorariosDisponibles
                .Include(h => h.Medico)
                    .ThenInclude(m => m!.Especialidad)
                .OrderBy(h => h.Fecha)
                .ThenBy(h => h.HoraInicio)
                .ToListAsync();

            // Si no hay horarios en la base de datos, devolver datos de demostración
            if (!horarios.Any())
            {
                var demoHorarios = DemoData.GetHorariosDisponibles();
                var demoMedicos = DemoData.GetMedicos();
                var demoEspecialidades = DemoData.GetEspecialidades();

                // Asociar los médicos y especialidades de demostración
                foreach (var horario in demoHorarios)
                {
                    horario.Medico = demoMedicos.FirstOrDefault(m => m.Id == horario.MedicoId);
                    if (horario.Medico != null)
                    {
                        horario.Medico.Especialidad = demoEspecialidades.FirstOrDefault(e => e.Id == horario.Medico.EspecialidadId);
                    }
                }

                return demoHorarios;
            }

            return horarios;
        }
        catch (Exception ex)
        {
            // Si hay error con la base de datos, devolver datos de demostración
            var demoHorarios = DemoData.GetHorariosDisponibles();
            var demoMedicos = DemoData.GetMedicos();
            var demoEspecialidades = DemoData.GetEspecialidades();

            // Asociar los médicos y especialidades de demostración
            foreach (var horario in demoHorarios)
            {
                horario.Medico = demoMedicos.FirstOrDefault(m => m.Id == horario.MedicoId);
                if (horario.Medico != null)
                {
                    horario.Medico.Especialidad = demoEspecialidades.FirstOrDefault(e => e.Id == horario.Medico.EspecialidadId);
                }
            }

            return demoHorarios;
        }
    }

    public async Task<IEnumerable<HorarioDisponible>> GetHorariosByMedicoAsync(int medicoId)
    {
        var horarios = await _context.HorariosDisponibles
            .Include(h => h.Medico)
            .Where(h => h.MedicoId == medicoId)
            .OrderBy(h => h.Fecha)
            .ThenBy(h => h.HoraInicio)
            .ToListAsync();

        // Si no hay horarios en la base de datos, devolver datos de demostración
        if (!horarios.Any())
        {
            var demoHorarios = DemoData.GetHorariosDisponibles()
                .Where(h => h.MedicoId == medicoId)
                .ToList();

            // Asociar los médicos de demostración
            var demoMedicos = DemoData.GetMedicos();
            foreach (var horario in demoHorarios)
            {
                horario.Medico = demoMedicos.FirstOrDefault(m => m.Id == horario.MedicoId);
            }

            return demoHorarios;
        }

        return horarios;
    }

    public async Task<IEnumerable<HorarioDisponible>> GetHorariosDisponiblesAsync(int medicoId, DateTime fecha)
    {
        var horarios = await _context.HorariosDisponibles
            .Include(h => h.Medico)
            .Where(h => h.MedicoId == medicoId && 
                       h.Fecha.Date == fecha.Date && 
                       h.Disponible)
            .OrderBy(h => h.HoraInicio)
            .ToListAsync();

        // Si no hay horarios en la base de datos, devolver datos de demostración
        if (!horarios.Any())
        {
            var demoHorarios = DemoData.GetHorariosDisponibles()
                .Where(h => h.MedicoId == medicoId && 
                           h.Fecha.Date == fecha.Date && 
                           h.Disponible)
                .ToList();

            // Asociar los médicos de demostración
            var demoMedicos = DemoData.GetMedicos();
            foreach (var horario in demoHorarios)
            {
                horario.Medico = demoMedicos.FirstOrDefault(m => m.Id == horario.MedicoId);
            }

            return demoHorarios;
        }

        return horarios;
    }

    public async Task<IEnumerable<HorarioDisponible>> GetHorariosDisponiblesByEspecialidadAsync(int especialidadId, DateTime fecha)
    {
        var horarios = await _context.HorariosDisponibles
            .Include(h => h.Medico)
                .ThenInclude(m => m.Especialidad)
            .Where(h => h.Medico.EspecialidadId == especialidadId && 
                       h.Fecha.Date == fecha.Date && 
                       h.Disponible &&
                       h.Medico.Activo)
            .OrderBy(h => h.Medico.Nombre)
            .ThenBy(h => h.HoraInicio)
            .ToListAsync();

        // Si no hay horarios en la base de datos, devolver datos de demostración
        if (!horarios.Any())
        {
            var demoMedicos = DemoData.GetMedicos().Where(m => m.EspecialidadId == especialidadId);
            var demoEspecialidades = DemoData.GetEspecialidades();
            var demoHorarios = new List<HorarioDisponible>();

            foreach (var medico in demoMedicos)
            {
                var horariosDelMedico = DemoData.GetHorariosDisponibles()
                    .Where(h => h.MedicoId == medico.Id && 
                               h.Fecha.Date == fecha.Date && 
                               h.Disponible)
                    .ToList();

                foreach (var horario in horariosDelMedico)
                {
                    horario.Medico = medico;
                    horario.Medico.Especialidad = demoEspecialidades.FirstOrDefault(e => e.Id == medico.EspecialidadId);
                }

                demoHorarios.AddRange(horariosDelMedico);
            }

            return demoHorarios.OrderBy(h => h.Medico.Nombre).ThenBy(h => h.HoraInicio);
        }

        return horarios;
    }

    public async Task<HorarioDisponible?> GetHorarioByIdAsync(int id)
    {
        return await _context.HorariosDisponibles
            .Include(h => h.Medico)
                .ThenInclude(m => m.Especialidad)
            .FirstOrDefaultAsync(h => h.Id == id);
    }

    public async Task<HorarioDisponible> CreateHorarioAsync(HorarioDisponible horario)
    {
        // Verificar que el médico existe y está activo
        var medico = await _context.Medicos.FindAsync(horario.MedicoId);
        if (medico == null || !medico.Activo)
        {
            throw new InvalidOperationException("El médico no existe o no está activo");
        }

        // Verificar que no haya conflicto de horarios
        var conflicto = await _context.HorariosDisponibles
            .AnyAsync(h => h.MedicoId == horario.MedicoId && 
                          h.Fecha.Date == horario.Fecha.Date &&
                          ((h.HoraInicio <= horario.HoraInicio && h.HoraFin > horario.HoraInicio) ||
                           (h.HoraInicio < horario.HoraFin && h.HoraFin >= horario.HoraFin) ||
                           (horario.HoraInicio <= h.HoraInicio && horario.HoraFin >= h.HoraFin)));

        if (conflicto)
        {
            throw new InvalidOperationException("Ya existe un horario que se superpone con el horario especificado");
        }

        // Validar que la hora de inicio sea menor que la hora de fin
        if (horario.HoraInicio >= horario.HoraFin)
        {
            throw new InvalidOperationException("La hora de inicio debe ser menor que la hora de fin");
        }

        // Validar que la fecha no sea en el pasado
        if (horario.Fecha.Date < DateTime.Now.Date)
        {
            throw new InvalidOperationException("No se pueden crear horarios para fechas pasadas");
        }

        horario.FechaCreacion = DateTime.Now;
        _context.HorariosDisponibles.Add(horario);
        await _context.SaveChangesAsync();
        return horario;
    }

    public async Task<HorarioDisponible> UpdateHorarioAsync(int id, HorarioDisponible horario)
    {
        var existingHorario = await _context.HorariosDisponibles.FindAsync(id);
        if (existingHorario == null)
        {
            throw new KeyNotFoundException("Horario no encontrado");
        }

        // Verificar que no haya citas asociadas si se cambia la disponibilidad
        if (!horario.Disponible && existingHorario.Disponible)
        {
            var citasAsociadas = await _context.Citas
                .AnyAsync(c => c.HorarioDisponibleId == id && 
                              c.Estado != "Cancelada" && 
                              c.Estado != "Completada");

            if (citasAsociadas)
            {
                throw new InvalidOperationException("No se puede marcar como no disponible un horario que tiene citas activas");
            }
        }

        existingHorario.Fecha = horario.Fecha;
        existingHorario.HoraInicio = horario.HoraInicio;
        existingHorario.HoraFin = horario.HoraFin;
        existingHorario.Disponible = horario.Disponible;

        await _context.SaveChangesAsync();
        return existingHorario;
    }

    public async Task<bool> DeleteHorarioAsync(int id)
    {
        var horario = await _context.HorariosDisponibles.FindAsync(id);
        if (horario == null)
        {
            return false;
        }

        // Verificar si tiene citas asociadas
        var citasAsociadas = await _context.Citas
            .AnyAsync(c => c.HorarioDisponibleId == id && 
                          c.Estado != "Cancelada" && 
                          c.Estado != "Completada");

        if (citasAsociadas)
        {
            throw new InvalidOperationException("No se puede eliminar un horario que tiene citas activas");
        }

        _context.HorariosDisponibles.Remove(horario);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> MarcarComoNoDisponibleAsync(int id)
    {
        var horario = await _context.HorariosDisponibles.FindAsync(id);
        if (horario == null)
        {
            return false;
        }

        horario.Disponible = false;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<HorarioDisponible>> CreateHorariosRecurrentesAsync(
        int medicoId, 
        DateTime fechaInicio, 
        DateTime fechaFin, 
        TimeSpan horaInicio, 
        TimeSpan horaFin, 
        List<DayOfWeek> diasSemana)
    {
        var horariosCreados = new List<HorarioDisponible>();
        
        for (var fecha = fechaInicio.Date; fecha <= fechaFin.Date; fecha = fecha.AddDays(1))
        {
            if (diasSemana.Contains(fecha.DayOfWeek))
            {
                var horario = new HorarioDisponible
                {
                    MedicoId = medicoId,
                    Fecha = fecha,
                    HoraInicio = horaInicio,
                    HoraFin = horaFin,
                    Disponible = true,
                    FechaCreacion = DateTime.Now
                };

                try
                {
                    var horarioCreado = await CreateHorarioAsync(horario);
                    horariosCreados.Add(horarioCreado);
                }
                catch (InvalidOperationException)
                {
                    // Si hay conflicto, continúa con el siguiente día
                    continue;
                }
            }
        }

        return horariosCreados;
    }
}