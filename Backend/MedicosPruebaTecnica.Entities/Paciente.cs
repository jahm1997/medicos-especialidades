using System.ComponentModel.DataAnnotations;

namespace MedicosPruebaTecnica.Entities;

public class Paciente
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
    [MaxLength(20)]
    public string Cedula { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(100)]
    public string Email { get; set; } = string.Empty;
    
    [MaxLength(20)]
    public string? Telefono { get; set; }
    
    public DateTime FechaNacimiento { get; set; }
    
    [MaxLength(200)]
    public string? Direccion { get; set; }
    
    public DateTime FechaRegistro { get; set; } = DateTime.Now;
    
    public bool Activo { get; set; } = true;
    
    // Navegación: Un paciente puede tener muchas citas
    public virtual ICollection<Cita> Citas { get; set; } = new List<Cita>();
}