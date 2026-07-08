using System.ComponentModel.DataAnnotations;

namespace EcoMeal.Site.Models;

public class PackageAddModel
{
    [Required(ErrorMessage = "Name is required")]
    [StringLength(50)]
    public required string Name { get; set; }
    [Required(ErrorMessage = "Description is required")]
    public required string Description { get; set; }
    [Required]
    [Range(0,1000)]
    public double Price { get; set; }
    [Required]
    public DateTime PickupStart { get; set; }
    [Required]
    public DateTime PickupEnd { get; set; }
    [Required]
    public int PackageTypeId { get; set; }


}