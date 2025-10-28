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

    public async Task<IEnumerable<Medico>> GetAllMedicosAsync()
    {
        var medicos = await _context.Medicos
            .Include(m => m.Especialidad)
            .Where(m => m.Activo)
            .OrderBy(m => m.Nombre)
            .ToListAsync();

        // Si no hay médicos en la base de datos, devolver datos de demostración
        if (!medicos.Any())
        {
            var demoMedicos = DemoData.GetMedicos();
            var demoEspecialidades = DemoData.GetEspecialidades();
            
            // Asociar especialidades a médicos
            foreach (var medico in demoMedicos)
            {
                medico.Especialidad = demoEspecialidades.FirstOrDefault(e => e.Id == medico.EspecialidadId);
            }
            
            return demoMedicos;
        }

        return medicos;
    }

    public async Task<Medico?> GetMedicoByIdAsync(int id)
    {
        return await _context.Medicos
            .Include(m => m.Especialidad)
            .Include(m => m.HorariosDisponibles)
            .Include(m => m.Citas)
            .FirstOrDefaultAsync(m => m.Id == id && m.Activo);
    }

    public async Task<IEnumerable<Medico>> GetMedicosByEspecialidadAsync(int especialidadId)
    {
        var medicos = await _context.Medicos
            .Include(m => m.Especialidad)
            .Where(m => m.EspecialidadId == especialidadId && m.Activo)
            .OrderBy(m => m.Nombre)
            .ToListAsync();

        // Si no hay médicos para esta especialidad en la base de datos, devolver datos de demostración
        if (!medicos.Any())
        {
            var demoMedicos = DemoData.GetMedicos().Where(m => m.EspecialidadId == especialidadId);
            var demoEspecialidades = DemoData.GetEspecialidades();
            
            // Asociar especialidades a médicos
            foreach (var medico in demoMedicos)
            {
                medico.Especialidad = demoEspecialidades.FirstOrDefault(e => e.Id == medico.EspecialidadId);
            }
            
            return demoMedicos;
        }

        return medicos;
    }

    public async Task<Medico> CreateMedicoAsync(Medico medico)
    {
        // Verificar que la especialidad existe
        var especialidad = await _context.Especialidades.FindAsync(medico.EspecialidadId);
        if (especialidad == null || !especialidad.Activa)
        {
            throw new InvalidOperationException("La especialidad no existe o no está activa");
        }

        // Verificar que no exista un médico con el mismo email
        var existingEmail = await _context.Medicos
            .FirstOrDefaultAsync(m => m.Email == medico.Email);
        
        if (existingEmail != null)
        {
            throw new InvalidOperationException("Ya existe un médico con este email");
        }

        // Verificar que no exista un médico con el mismo número de licencia
        var existingLicencia = await _context.Medicos
            .FirstOrDefaultAsync(m => m.NumeroLicencia == medico.NumeroLicencia);
        
        if (existingLicencia != null)
        {
            throw new InvalidOperationException("Ya existe un médico con este número de licencia");
        }

        medico.FechaRegistro = DateTime.Now;
        _context.Medicos.Add(medico);
        await _context.SaveChangesAsync();
        return medico;
    }

    public async Task<Medico> UpdateMedicoAsync(int id, Medico medico)
    {
        var existingMedico = await _context.Medicos.FindAsync(id);
        if (existingMedico == null)
        {
            throw new KeyNotFoundException("Médico no encontrado");
        }

        // Verificar que la especialidad existe
        var especialidad = await _context.Especialidades.FindAsync(medico.EspecialidadId);
        if (especialidad == null || !especialidad.Activa)
        {
            throw new InvalidOperationException("La especialidad no existe o no está activa");
        }

        // Verificar que no exista otro médico con el mismo email
        var duplicateEmail = await _context.Medicos
            .FirstOrDefaultAsync(m => m.Email == medico.Email && m.Id != id);
        
        if (duplicateEmail != null)
        {
            throw new InvalidOperationException("Ya existe otro médico con este email");
        }

        // Verificar que no exista otro médico con el mismo número de licencia
        var duplicateLicencia = await _context.Medicos
            .FirstOrDefaultAsync(m => m.NumeroLicencia == medico.NumeroLicencia && m.Id != id);
        
        if (duplicateLicencia != null)
        {
            throw new InvalidOperationException("Ya existe otro médico con este número de licencia");
        }

        existingMedico.Nombre = medico.Nombre;
        existingMedico.Apellido = medico.Apellido;
        existingMedico.Email = medico.Email;
        existingMedico.Telefono = medico.Telefono;
        existingMedico.EspecialidadId = medico.EspecialidadId;
        existingMedico.NumeroLicencia = medico.NumeroLicencia;
        existingMedico.Activo = medico.Activo;

        await _context.SaveChangesAsync();
        return existingMedico;
    }

    public async Task<bool> DeleteMedicoAsync(int id)
    {
        var medico = await _context.Medicos.FindAsync(id);
        if (medico == null)
        {
            return false;
        }

        // Verificar si tiene citas activas
        var citasActivas = await _context.Citas
            .AnyAsync(c => c.MedicoId == id && 
                          c.Estado != "Cancelada" && 
                          c.Estado != "Completada");

        if (citasActivas)
        {
            throw new InvalidOperationException("No se puede eliminar el médico porque tiene citas activas");
        }

        // Soft delete
        medico.Activo = false;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<Medico>> SearchMedicosAsync(string searchTerm)
    {
        return await _context.Medicos
            .Include(m => m.Especialidad)
            .Where(m => m.Activo && 
                       (m.Nombre.Contains(searchTerm) || 
                        m.Apellido.Contains(searchTerm) || 
                        m.Email.Contains(searchTerm) ||
                        m.NumeroLicencia.Contains(searchTerm) ||
                        m.Especialidad.Nombre.Contains(searchTerm)))
            .OrderBy(m => m.Nombre)
            .ToListAsync();
    }

    public async Task<IEnumerable<Medico>> GetMedicosConHorariosDisponiblesAsync(DateTime fecha)
    {
        return await _context.Medicos
            .Include(m => m.Especialidad)
            .Include(m => m.HorariosDisponibles)
            .Where(m => m.Activo && 
                       m.HorariosDisponibles.Any(h => h.Fecha.Date == fecha.Date && h.Disponible))
            .OrderBy(m => m.Nombre)
            .ToListAsync();
    }
}