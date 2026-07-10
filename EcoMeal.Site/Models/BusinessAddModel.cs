using System.ComponentModel.DataAnnotations;

namespace EcoMeal.Site.Models;

public class BusinessAddModel
{
    [Required(ErrorMessage = "Numele este obligatoriu")]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Adresa este obligatorie")]
    public string Address { get; set; } = string.Empty;

    public string? Description { get; set; }

    [Required(ErrorMessage = "Contactul este obligatoriu")]
    public string Contact { get; set; } = string.Empty;

    [Required(ErrorMessage = "Trebuie ales un tip de business")]
    public int BusinessTypeId { get; set; }
}
