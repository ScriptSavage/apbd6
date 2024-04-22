using System.ComponentModel.DataAnnotations;

namespace Apka2.DTOs;

public class AddAnimal
{
    
    // Walidacja Danych
    [Required]
    [MaxLength(200)]
    public string Name { get; set; }
    
    [Required]
    [MaxLength(200)]
    public string Description { get; set; }
    
    [Required]
    [MaxLength(200)]
    public string Category { get; set; }
    
    [Required]
    [MaxLength(200)]
    public string Area { get; set; }
    
}


