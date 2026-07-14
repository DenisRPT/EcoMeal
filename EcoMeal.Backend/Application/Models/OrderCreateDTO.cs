using System.ComponentModel.DataAnnotations;

namespace EcoMeal.Backend.Models;

public class OrderCreateDTO
{
    [Required]
    public int PackageId{get;set;}
}