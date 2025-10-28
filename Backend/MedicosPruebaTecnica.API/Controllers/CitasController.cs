using Microsoft.AspNetCore.Mvc;
using MedicosPruebaTecnica.Business;
using MedicosPruebaTecnica.Entities;

namespace MedicosPruebaTecnica.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CitasController : ControllerBase
{
    private readonly CitaService _citaService;

    public CitasController(CitaService citaService)
    {
        _citaService = citaService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Cita>>> GetCitas()
    {
        try
        {
            var citas = await _citaService.GetAllCitasAsync();
            return Ok(citas);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Cita>> GetCita(int id)
    {
        try
        {
            var cita = await _citaService.GetCitaByIdAsync(id);
            if (cita == null)
            {
                return NotFound(new { message = "Cita no encontrada" });
            }
            return Ok(cita);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
        }
    }

    [HttpGet("paciente/{pacienteId}")]
    public async Task<ActionResult<IEnumerable<Cita>>> GetCitasByPaciente(int pacienteId)
    {
        try
        {
            var citas = await _citaService.GetCitasByPacienteAsync(pacienteId);
            return Ok(citas);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
        }
    }

    [HttpGet("medico/{medicoId}")]
    public async Task<ActionResult<IEnumerable<Cita>>> GetCitasByMedico(int medicoId)
    {
        try
        {
            var citas = await _citaService.GetCitasByMedicoAsync(medicoId);
            return Ok(citas);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
        }
    }

    [HttpGet("fecha/{fecha}")]
    public async Task<ActionResult<IEnumerable<Cita>>> GetCitasByFecha(DateTime fecha)
    {
        try
        {
            var citas = await _citaService.GetCitasByFechaAsync(fecha);
            return Ok(citas);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
        }
    }

    [HttpGet("pendientes")]
    public async Task<ActionResult<IEnumerable<Cita>>> GetCitasPendientes()
    {
        try
        {
            var citas = await _citaService.GetCitasPendientesAsync();
            return Ok(citas);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
        }
    }

    [HttpGet("hoy")]
    public async Task<ActionResult<IEnumerable<Cita>>> GetCitasDelDia()
    {
        try
        {
            var citas = await _citaService.GetCitasDelDiaAsync(DateTime.Today);
            return Ok(citas);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
        }
    }

    [HttpPost]
    public async Task<ActionResult<Cita>> CreateCita(Cita cita)
    {
        try
        {
            var nuevaCita = await _citaService.CreateCitaAsync(cita);
            return CreatedAtAction(nameof(GetCita), new { id = nuevaCita.Id }, nuevaCita);
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
    public async Task<ActionResult<Cita>> UpdateCita(int id, Cita cita)
    {
        try
        {
            var citaActualizada = await _citaService.UpdateCitaAsync(id, cita);
            return Ok(citaActualizada);
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

    [HttpPut("{id}/cancelar")]
    public async Task<ActionResult> CancelarCita(int id, [FromBody] CancelarCitaRequest request)
    {
        try
        {
            var resultado = await _citaService.CancelarCitaAsync(id, request.Motivo);
            if (!resultado)
            {
                return NotFound(new { message = "Cita no encontrada" });
            }
            return Ok(new { message = "Cita cancelada correctamente" });
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

    [HttpPut("{id}/completar")]
    public async Task<ActionResult> CompletarCita(int id, [FromBody] CompletarCitaRequest request)
    {
        try
        {
            var resultado = await _citaService.CompletarCitaAsync(id, request.Observaciones);
            if (!resultado)
            {
                return NotFound(new { message = "Cita no encontrada" });
            }
            return Ok(new { message = "Cita completada correctamente" });
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
    public async Task<ActionResult> DeleteCita(int id)
    {
        try
        {
            var resultado = await _citaService.DeleteCitaAsync(id);
            if (!resultado)
            {
                return NotFound(new { message = "Cita no encontrada" });
            }
            return Ok(new { message = "Cita eliminada correctamente" });
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

public class CancelarCitaRequest
{
    public string Motivo { get; set; } = string.Empty;
}

public class CompletarCitaRequest
{
    public string Observaciones { get; set; } = string.Empty;
}