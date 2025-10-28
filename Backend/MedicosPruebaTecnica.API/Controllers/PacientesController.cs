using Microsoft.AspNetCore.Mvc;
using MedicosPruebaTecnica.Business;
using MedicosPruebaTecnica.Entities;

namespace MedicosPruebaTecnica.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PacientesController : ControllerBase
{
    private readonly PacienteService _pacienteService;

    public PacientesController(PacienteService pacienteService)
    {
        _pacienteService = pacienteService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Paciente>>> GetPacientes()
    {
        try
        {
            var pacientes = await _pacienteService.GetAllPacientesAsync();
            return Ok(pacientes);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Paciente>> GetPaciente(int id)
    {
        try
        {
            var paciente = await _pacienteService.GetPacienteByIdAsync(id);
            if (paciente == null)
            {
                return NotFound(new { message = "Paciente no encontrado" });
            }
            return Ok(paciente);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
        }
    }

    [HttpGet("cedula/{cedula}")]
    public async Task<ActionResult<Paciente>> GetPacienteByCedula(string cedula)
    {
        try
        {
            var paciente = await _pacienteService.GetPacienteByCedulaAsync(cedula);
            if (paciente == null)
            {
                return NotFound(new { message = "Paciente no encontrado" });
            }
            return Ok(paciente);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
        }
    }

    [HttpGet("search/{searchTerm}")]
    public async Task<ActionResult<IEnumerable<Paciente>>> SearchPacientes(string searchTerm)
    {
        try
        {
            var pacientes = await _pacienteService.SearchPacientesAsync(searchTerm);
            return Ok(pacientes);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
        }
    }

    [HttpPost]
    public async Task<ActionResult<Paciente>> CreatePaciente(Paciente paciente)
    {
        try
        {
            var nuevoPaciente = await _pacienteService.CreatePacienteAsync(paciente);
            return CreatedAtAction(nameof(GetPaciente), new { id = nuevoPaciente.Id }, nuevoPaciente);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Paciente>> UpdatePaciente(int id, Paciente paciente)
    {
        try
        {
            var pacienteActualizado = await _pacienteService.UpdatePacienteAsync(id, paciente);
            return Ok(pacienteActualizado);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeletePaciente(int id)
    {
        try
        {
            var resultado = await _pacienteService.DeletePacienteAsync(id);
            if (!resultado)
            {
                return NotFound(new { message = "Paciente no encontrado" });
            }
            return Ok(new { message = "Paciente eliminado correctamente" });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
        }
    }
}