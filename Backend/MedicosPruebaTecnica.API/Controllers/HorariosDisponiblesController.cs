using Microsoft.AspNetCore.Mvc;
using MedicosPruebaTecnica.Business;
using MedicosPruebaTecnica.Entities;

namespace MedicosPruebaTecnica.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HorariosDisponiblesController : ControllerBase
{
    private readonly HorarioDisponibleService _horarioService;

    public HorariosDisponiblesController(HorarioDisponibleService horarioService)
    {
        _horarioService = horarioService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<HorarioDisponible>>> GetAllHorarios()
    {
        try
        {
            Console.WriteLine("Iniciando GetAllHorarios...");
            var horarios = await _horarioService.GetAllHorariosAsync();
            Console.WriteLine($"Horarios obtenidos: {horarios?.Count() ?? 0}");
            return Ok(horarios);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error en GetAllHorarios: {ex.Message}");
            Console.WriteLine($"StackTrace: {ex.StackTrace}");
            if (ex.InnerException != null)
            {
                Console.WriteLine($"InnerException: {ex.InnerException.Message}");
            }
            return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message, stackTrace = ex.StackTrace });
        }
    }

    [HttpGet("medico/{medicoId}")]
    public async Task<ActionResult<IEnumerable<HorarioDisponible>>> GetHorariosByMedico(int medicoId)
    {
        try
        {
            var horarios = await _horarioService.GetHorariosByMedicoAsync(medicoId);
            return Ok(horarios);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
        }
    }

    [HttpGet("medico/{medicoId}/fecha/{fecha}")]
    public async Task<ActionResult<IEnumerable<HorarioDisponible>>> GetHorariosDisponibles(int medicoId, DateTime fecha)
    {
        try
        {
            var horarios = await _horarioService.GetHorariosDisponiblesAsync(medicoId, fecha);
            return Ok(horarios);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
        }
    }

    [HttpGet("especialidad/{especialidadId}/fecha/{fecha}")]
    public async Task<ActionResult<IEnumerable<HorarioDisponible>>> GetHorariosByEspecialidad(int especialidadId, DateTime fecha)
    {
        try
        {
            var horarios = await _horarioService.GetHorariosDisponiblesByEspecialidadAsync(especialidadId, fecha);
            return Ok(horarios);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<HorarioDisponible>> GetHorario(int id)
    {
        try
        {
            var horario = await _horarioService.GetHorarioByIdAsync(id);
            if (horario == null)
            {
                return NotFound(new { message = "Horario no encontrado" });
            }
            return Ok(horario);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
        }
    }

    [HttpPost]
    public async Task<ActionResult<HorarioDisponible>> CreateHorario(HorarioDisponible horario)
    {
        try
        {
            var nuevoHorario = await _horarioService.CreateHorarioAsync(horario);
            return CreatedAtAction(nameof(GetHorario), new { id = nuevoHorario.Id }, nuevoHorario);
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

    [HttpPost("recurrentes")]
    public async Task<ActionResult<IEnumerable<HorarioDisponible>>> CreateHorariosRecurrentes(
        [FromBody] HorarioRecurrenteRequest request)
    {
        try
        {
            var horariosCreados = await _horarioService.CreateHorariosRecurrentesAsync(
                request.MedicoId,
                request.FechaInicio,
                request.FechaFin,
                request.HoraInicio,
                request.HoraFin,
                request.DiasSemana);

            return Ok(horariosCreados);
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
    public async Task<ActionResult<HorarioDisponible>> UpdateHorario(int id, HorarioDisponible horario)
    {
        try
        {
            var horarioActualizado = await _horarioService.UpdateHorarioAsync(id, horario);
            return Ok(horarioActualizado);
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

    [HttpPut("{id}/marcar-no-disponible")]
    public async Task<ActionResult> MarcarComoNoDisponible(int id)
    {
        try
        {
            var resultado = await _horarioService.MarcarComoNoDisponibleAsync(id);
            if (!resultado)
            {
                return NotFound(new { message = "Horario no encontrado" });
            }
            return Ok(new { message = "Horario marcado como no disponible" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteHorario(int id)
    {
        try
        {
            var resultado = await _horarioService.DeleteHorarioAsync(id);
            if (!resultado)
            {
                return NotFound(new { message = "Horario no encontrado" });
            }
            return Ok(new { message = "Horario eliminado correctamente" });
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

public class HorarioRecurrenteRequest
{
    public int MedicoId { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public TimeSpan HoraInicio { get; set; }
    public TimeSpan HoraFin { get; set; }
    public List<DayOfWeek> DiasSemana { get; set; } = new();
}