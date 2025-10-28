using System.ComponentModel.DataAnnotations;

namespace MedicosPruebaTecnica.Entities;

public class Especialidad
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Nombre { get; set; } = string.Empty;
    
    [MaxLength(500)]
    public string? Descripcion { get; set; }
    
    public bool Activa { get; set; } = true;
    
    public DateTime FechaCreacion { get; set; } = DateTime.Now;
    
    // Navegación: Una especialidad puede tener muchos médicos
    public virtual ICollection<Medico> Medicos { get; set; } = new List<Medico>();
}