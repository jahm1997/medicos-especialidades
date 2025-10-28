using MedicosPruebaTecnica.Data;
using MedicosPruebaTecnica.Entities;
using Microsoft.EntityFrameworkCore;

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
        return await _context.Usuarios.ToListAsync();
    }

    public async Task<Usuario?> GetUsuarioByIdAsync(int id)
    {
        return await _context.Usuarios.FindAsync(id);
    }

    public async Task<Usuario> CreateUsuarioAsync(Usuario usuario)
    {
        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();
        return usuario;
    }

    public async Task<Usuario?> UpdateUsuarioAsync(int id, Usuario usuario)
    {
        var existingUsuario = await _context.Usuarios.FindAsync(id);
        if (existingUsuario == null)
            return null;

        existingUsuario.Nombre = usuario.Nombre;
        existingUsuario.Email = usuario.Email;
        existingUsuario.Password = usuario.Password;
        existingUsuario.Activo = usuario.Activo;

        await _context.SaveChangesAsync();
        return existingUsuario;
    }

    public async Task<bool> DeleteUsuarioAsync(int id)
    {
        var usuario = await _context.Usuarios.FindAsync(id);
        if (usuario == null)
            return false;

        _context.Usuarios.Remove(usuario);
        await _context.SaveChangesAsync();
        return true;
    }
}