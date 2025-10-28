using Microsoft.AspNetCore.Mvc;
using MedicosPruebaTecnica.Business;
using MedicosPruebaTecnica.Entities;

namespace MedicosPruebaTecnica.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MedicosController : ControllerBase
{
    private readonly MedicoService _medicoService;

    public MedicosController(MedicoService medicoService)
    {
        _medicoService = medicoService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Medico>>> GetMedicos()
    {
        try
        {
            var medicos = await _medicoService.GetAllMedicosAsync();
            return Ok(medicos);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Medico>> GetMedico(int id)
    {
        try
        {
            var medico = await _medicoService.GetMedicoByIdAsync(id);
            if (medico == null)
            {
                return NotFound(new { message = "Médico no encontrado" });
            }
            return Ok(medico);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
        }
    }

    [HttpGet("especialidad/{especialidadId}")]
    public async Task<ActionResult<IEnumerable<Medico>>> GetMedicosByEspecialidad(int especialidadId)
    {
        try
        {
            var medicos = await _medicoService.GetMedicosByEspecialidadAsync(especialidadId);
            return Ok(medicos);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
        }
    }

    [HttpGet("con-horarios/{fecha}")]
    public async Task<ActionResult<IEnumerable<Medico>>> GetMedicosConHorarios(DateTime fecha)
    {
        try
        {
            var medicos = await _medicoService.GetMedicosConHorariosDisponiblesAsync(fecha);
            return Ok(medicos);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
        }
    }

    [HttpGet("search/{searchTerm}")]
    public async Task<ActionResult<IEnumerable<Medico>>> SearchMedicos(string searchTerm)
    {
        try
        {
            var medicos = await _medicoService.SearchMedicosAsync(searchTerm);
            return Ok(medicos);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
        }
    }

    [HttpPost]
    public async Task<ActionResult<Medico>> CreateMedico(Medico medico)
    {
        try
        {
            var nuevoMedico = await _medicoService.CreateMedicoAsync(medico);
            return CreatedAtAction(nameof(GetMedico), new { id = nuevoMedico.Id }, nuevoMedico);
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
    public async Task<ActionResult<Medico>> UpdateMedico(int id, Medico medico)
    {
        try
        {
            var medicoActualizado = await _medicoService.UpdateMedicoAsync(id, medico);
            return Ok(medicoActualizado);
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
    public async Task<ActionResult> DeleteMedico(int id)
    {
        try
        {
            var resultado = await _medicoService.DeleteMedicoAsync(id);
            if (!resultado)
            {
                return NotFound(new { message = "Médico no encontrado" });
            }
            return Ok(new { message = "Médico eliminado correctamente" });
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