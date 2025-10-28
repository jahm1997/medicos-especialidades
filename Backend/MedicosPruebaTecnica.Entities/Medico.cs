using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MedicosPruebaTecnica.Entities;

public class Medico
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Nombre { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(100)]
    public string Apellido { get; set; } = string.Empty;
    
    [Required]
    public int EspecialidadId { get; set; }
    
    [MaxLength(20)]
    public string? Telefono { get; set; }
    
    [MaxLength(100)]
    public string? Email { get; set; }
    
    [MaxLength(200)]
    public string? Direccion { get; set; }
    
    [MaxLength(50)]
    public string? NumeroLicencia { get; set; }
    
    public DateTime FechaRegistro { get; set; } = DateTime.Now;
    
    public bool Activo { get; set; } = true;
    
    // Navegación
    [ForeignKey("EspecialidadId")]
    public virtual Especialidad? Especialidad { get; set; }
    
    // Un médico puede tener muchos horarios disponibles
    [JsonIgnore]
    public virtual ICollection<HorarioDisponible> HorariosDisponibles { get; set; } = new List<HorarioDisponible>();
    
    // Un médico puede tener muchas citas
    [JsonIgnore]
    public virtual ICollection<Cita> Citas { get; set; } = new List<Cita>();
}