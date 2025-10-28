using System.ComponentModel.DataAnnotations;

namespace MedicosPruebaTecnica.Entities;

public class Usuario
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Nombre { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(100)]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(255)]
    public string Password { get; set; } = string.Empty;
    
    public DateTime FechaCreacion { get; set; } = DateTime.Now;
    
    public bool Activo { get; set; } = true;
}