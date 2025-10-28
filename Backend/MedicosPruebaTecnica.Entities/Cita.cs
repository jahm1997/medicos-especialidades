using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicosPruebaTecnica.Entities;

public class Cita
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public int PacienteId { get; set; }
    
    [Required]
    public int MedicoId { get; set; }
    
    [Required]
    public int EspecialidadId { get; set; }
    
    public int? HorarioDisponibleId { get; set; }
    
    [Required]
    public DateTime FechaHora { get; set; }
    
    [MaxLength(500)]
    public string? Motivo { get; set; }
    
    [MaxLength(1000)]
    public string? Observaciones { get; set; }
    
    [Required]
    [MaxLength(20)]
    public string Estado { get; set; } = "Agendada"; // Agendada, Confirmada, Cancelada, Completada
    
    public DateTime FechaCreacion { get; set; } = DateTime.Now;
    
    public DateTime? FechaModificacion { get; set; }
    
    // Navegación
    [ForeignKey("PacienteId")]
    public virtual Paciente Paciente { get; set; } = null!;
    
    [ForeignKey("MedicoId")]
    public virtual Medico Medico { get; set; } = null!;
    
    [ForeignKey("EspecialidadId")]
    public virtual Especialidad Especialidad { get; set; } = null!;
    
    [ForeignKey("HorarioDisponibleId")]
    public virtual HorarioDisponible? HorarioDisponible { get; set; }
}