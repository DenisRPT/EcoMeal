using System.ComponentModel.DataAnnotations;

namespace EcoMeal.Site.Models;

public class PackageEditModel
{
    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string Description { get; set; } = string.Empty;

    [Range(0, 1000)]
    public double Price { get; set; }

    [Required]
    public DateTime PickupStart { get; set; }

    [Required]
    public DateTime PickupEnd { get; set; }

    [Required]
    public int PackageTypeId { get; set; }
}
