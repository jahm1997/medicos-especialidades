using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MedicosPruebaTecnica.Entities;

public class HorarioDisponible
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public int MedicoId { get; set; }
    
    [Required]
    public DateTime Fecha { get; set; }
    
    [Required]
    public TimeSpan HoraInicio { get; set; }
    
    [Required]
    public TimeSpan HoraFin { get; set; }
    
    public bool Disponible { get; set; } = true;
    
    public DateTime FechaCreacion { get; set; } = DateTime.Now;
    
    // Navegación
    [ForeignKey("MedicoId")]
    public virtual Medico? Medico { get; set; }
    
    // Un horario disponible puede tener muchas citas (franjas de 30 minutos)
    [JsonIgnore]
    public virtual ICollection<Cita> Citas { get; set; } = new List<Cita>();
}