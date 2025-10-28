using Microsoft.EntityFrameworkCore;
using MedicosPruebaTecnica.Data;
using MedicosPruebaTecnica.Entities;

namespace MedicosPruebaTecnica.Business;

public class PacienteService
{
    private readonly MyDbContext _context;

    public PacienteService(MyDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Paciente>> GetAllPacientesAsync()
    {
        var pacientes = await _context.Pacientes
            .Where(p => p.Activo)
            .OrderBy(p => p.Nombre)
            .ToListAsync();

        // Si no hay pacientes en la base de datos, devolver datos de demostración
        if (!pacientes.Any())
        {
            return DemoData.GetPacientes();
        }

        return pacientes;
    }

    public async Task<Paciente?> GetPacienteByIdAsync(int id)
    {
        return await _context.Pacientes
            .Include(p => p.Citas)
                .ThenInclude(c => c.Medico)
            .Include(p => p.Citas)
                .ThenInclude(c => c.Especialidad)
            .FirstOrDefaultAsync(p => p.Id == id && p.Activo);
    }

    public async Task<Paciente?> GetPacienteByCedulaAsync(string cedula)
    {
        var paciente = await _context.Pacientes
            .FirstOrDefaultAsync(p => p.Cedula == cedula && p.Activo);

        // Si no se encuentra el paciente en la base de datos, buscar en datos de demostración
        if (paciente == null)
        {
            return DemoData.GetPacientes().FirstOrDefault(p => p.Cedula == cedula);
        }

        return paciente;
    }

    public async Task<Paciente> CreatePacienteAsync(Paciente paciente)
    {
        // Verificar que no exista un paciente con la misma cédula
        var existingPaciente = await _context.Pacientes
            .FirstOrDefaultAsync(p => p.Cedula == paciente.Cedula);
        
        if (existingPaciente != null)
        {
            throw new InvalidOperationException("Ya existe un paciente con esta cédula");
        }

        // Verificar que no exista un paciente con el mismo email
        var existingEmail = await _context.Pacientes
            .FirstOrDefaultAsync(p => p.Email == paciente.Email);
        
        if (existingEmail != null)
        {
            throw new InvalidOperationException("Ya existe un paciente con este email");
        }

        paciente.FechaRegistro = DateTime.Now;
        _context.Pacientes.Add(paciente);
        await _context.SaveChangesAsync();
        return paciente;
    }

    public async Task<Paciente> UpdatePacienteAsync(int id, Paciente paciente)
    {
        var existingPaciente = await _context.Pacientes.FindAsync(id);
        if (existingPaciente == null)
        {
            throw new KeyNotFoundException("Paciente no encontrado");
        }

        // Verificar que no exista otro paciente con la misma cédula
        var duplicateCedula = await _context.Pacientes
            .FirstOrDefaultAsync(p => p.Cedula == paciente.Cedula && p.Id != id);
        
        if (duplicateCedula != null)
        {
            throw new InvalidOperationException("Ya existe otro paciente con esta cédula");
        }

        // Verificar que no exista otro paciente con el mismo email
        var duplicateEmail = await _context.Pacientes
            .FirstOrDefaultAsync(p => p.Email == paciente.Email && p.Id != id);
        
        if (duplicateEmail != null)
        {
            throw new InvalidOperationException("Ya existe otro paciente con este email");
        }

        existingPaciente.Nombre = paciente.Nombre;
        existingPaciente.Apellido = paciente.Apellido;
        existingPaciente.Cedula = paciente.Cedula;
        existingPaciente.Email = paciente.Email;
        existingPaciente.Telefono = paciente.Telefono;
        existingPaciente.FechaNacimiento = paciente.FechaNacimiento;
        existingPaciente.Direccion = paciente.Direccion;
        existingPaciente.Activo = paciente.Activo;

        await _context.SaveChangesAsync();
        return existingPaciente;
    }

    public async Task<bool> DeletePacienteAsync(int id)
    {
        var paciente = await _context.Pacientes.FindAsync(id);
        if (paciente == null)
        {
            return false;
        }

        // Verificar si tiene citas activas
        var citasActivas = await _context.Citas
            .AnyAsync(c => c.PacienteId == id && 
                          c.Estado != "Cancelada" && 
                          c.Estado != "Completada");

        if (citasActivas)
        {
            throw new InvalidOperationException("No se puede eliminar el paciente porque tiene citas activas");
        }

        // Soft delete
        paciente.Activo = false;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<Paciente>> SearchPacientesAsync(string searchTerm)
    {
        return await _context.Pacientes
            .Where(p => p.Activo && 
                       (p.Nombre.Contains(searchTerm) || 
                        p.Apellido.Contains(searchTerm) || 
                        p.Cedula.Contains(searchTerm) || 
                        p.Email.Contains(searchTerm)))
            .OrderBy(p => p.Nombre)
            .ToListAsync();
    }
}