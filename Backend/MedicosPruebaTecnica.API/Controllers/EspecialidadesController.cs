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
    public async Task<ActionResult<IEnumerable<Especialidad>>> GetEspecialidades()
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
            {
                return NotFound(new { message = "Especialidad no encontrada" });
            }
            return Ok(especialidad);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
        }
    }

    [HttpGet("con-medicos")]
    public async Task<ActionResult<IEnumerable<Especialidad>>> GetEspecialidadesConMedicos()
    {
        try
        {
            var especialidades = await _especialidadService.GetEspecialidadesConMedicosAsync();
            return Ok(especialidades);
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
    public async Task<ActionResult<Especialidad>> UpdateEspecialidad(int id, Especialidad especialidad)
    {
        try
        {
            var especialidadActualizada = await _especialidadService.UpdateEspecialidadAsync(id, especialidad);
            return Ok(especialidadActualizada);
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
    public async Task<ActionResult> DeleteEspecialidad(int id)
    {
        try
        {
            var resultado = await _especialidadService.DeleteEspecialidadAsync(id);
            if (!resultado)
            {
                return NotFound(new { message = "Especialidad no encontrada" });
            }
            return Ok(new { message = "Especialidad eliminada correctamente" });
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