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
        var usuarios = await _usuarioService.GetAllUsuariosAsync();
        return Ok(usuarios);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Usuario>> GetUsuario(int id)
    {
        var usuario = await _usuarioService.GetUsuarioByIdAsync(id);
        if (usuario == null)
            return NotFound();
        
        return Ok(usuario);
    }

    [HttpPost]
    public async Task<ActionResult<Usuario>> CreateUsuario(Usuario usuario)
    {
        var createdUsuario = await _usuarioService.CreateUsuarioAsync(usuario);
        return CreatedAtAction(nameof(GetUsuario), new { id = createdUsuario.Id }, createdUsuario);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Usuario>> UpdateUsuario(int id, Usuario usuario)
    {
        var updatedUsuario = await _usuarioService.UpdateUsuarioAsync(id, usuario);
        if (updatedUsuario == null)
            return NotFound();
        
        return Ok(updatedUsuario);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteUsuario(int id)
    {
        var deleted = await _usuarioService.DeleteUsuarioAsync(id);
        if (!deleted)
            return NotFound();
        
        return NoContent();
    }
}