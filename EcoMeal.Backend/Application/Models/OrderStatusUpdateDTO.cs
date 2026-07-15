using System.ComponentModel.DataAnnotations;

namespace EcoMeal.Backend.Models;

public class OrderStatusUpdateDTO
{
    [Required]
    public required string Status { get; set; }
}