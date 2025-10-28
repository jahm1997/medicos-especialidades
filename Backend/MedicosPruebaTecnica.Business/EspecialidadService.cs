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

    public async Task<IEnumerable<Especialidad>> GetAllEspecialidadesAsync()
    {
        var especialidades = await _context.Especialidades
            .Where(e => e.Activa)
            .OrderBy(e => e.Nombre)
            .ToListAsync();

        // Si no hay especialidades en la base de datos, devolver datos de demostración
        if (!especialidades.Any())
        {
            return DemoData.GetEspecialidades();
        }

        return especialidades;
    }

    public async Task<Especialidad?> GetEspecialidadByIdAsync(int id)
    {
        return await _context.Especialidades
            .Include(e => e.Medicos.Where(m => m.Activo))
            .FirstOrDefaultAsync(e => e.Id == id && e.Activa);
    }

    public async Task<Especialidad> CreateEspecialidadAsync(Especialidad especialidad)
    {
        // Verificar que no exista una especialidad con el mismo nombre
        var existingEspecialidad = await _context.Especialidades
            .FirstOrDefaultAsync(e => e.Nombre.ToLower() == especialidad.Nombre.ToLower());
        
        if (existingEspecialidad != null)
        {
            throw new InvalidOperationException("Ya existe una especialidad con este nombre");
        }

        especialidad.FechaCreacion = DateTime.Now;
        _context.Especialidades.Add(especialidad);
        await _context.SaveChangesAsync();
        return especialidad;
    }

    public async Task<Especialidad> UpdateEspecialidadAsync(int id, Especialidad especialidad)
    {
        var existingEspecialidad = await _context.Especialidades.FindAsync(id);
        if (existingEspecialidad == null)
        {
            throw new KeyNotFoundException("Especialidad no encontrada");
        }

        // Verificar que no exista otra especialidad con el mismo nombre
        var duplicateName = await _context.Especialidades
            .FirstOrDefaultAsync(e => e.Nombre.ToLower() == especialidad.Nombre.ToLower() && e.Id != id);
        
        if (duplicateName != null)
        {
            throw new InvalidOperationException("Ya existe otra especialidad con este nombre");
        }

        existingEspecialidad.Nombre = especialidad.Nombre;
        existingEspecialidad.Descripcion = especialidad.Descripcion;
        existingEspecialidad.Activa = especialidad.Activa;

        await _context.SaveChangesAsync();
        return existingEspecialidad;
    }

    public async Task<bool> DeleteEspecialidadAsync(int id)
    {
        var especialidad = await _context.Especialidades.FindAsync(id);
        if (especialidad == null)
        {
            return false;
        }

        // Verificar si tiene médicos asociados
        var medicosAsociados = await _context.Medicos
            .AnyAsync(m => m.EspecialidadId == id && m.Activo);

        if (medicosAsociados)
        {
            throw new InvalidOperationException("No se puede eliminar la especialidad porque tiene médicos asociados");
        }

        // Soft delete
        especialidad.Activa = false;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<Especialidad>> GetEspecialidadesConMedicosAsync()
    {
        var especialidades = await _context.Especialidades
            .Where(e => e.Activa && e.Medicos.Any(m => m.Activo))
            .Include(e => e.Medicos.Where(m => m.Activo))
            .OrderBy(e => e.Nombre)
            .ToListAsync();

        // Si no hay especialidades con médicos en la base de datos, devolver datos de demostración
        if (!especialidades.Any())
        {
            var demoEspecialidades = DemoData.GetEspecialidades();
            var demoMedicos = DemoData.GetMedicos();
            
            // Asociar médicos a especialidades
            foreach (var especialidad in demoEspecialidades)
            {
                especialidad.Medicos = demoMedicos.Where(m => m.EspecialidadId == especialidad.Id).ToList();
            }
            
            return demoEspecialidades.Where(e => e.Medicos.Any());
        }

        return especialidades;
    }
}