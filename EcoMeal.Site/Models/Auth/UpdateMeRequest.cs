using System.ComponentModel.DataAnnotations;

namespace EcoMeal.Site.Models.Auth;

public class UpdateMeRequest
{
    [Required(ErrorMessage = "Email-ul este obligatoriu")]
    [EmailAddress(ErrorMessage = "Introdu un email valid")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Numele este obligatoriu")]
    [StringLength(100, ErrorMessage = "Numele poate avea cel mult 100 de caractere")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Contactul este obligatoriu")]
    [StringLength(100, ErrorMessage = "Contactul poate avea cel mult 100 de caractere")]
    public string Contact { get; set; } = string.Empty;
}