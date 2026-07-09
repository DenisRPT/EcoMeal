namespace EcoMeal.Backend.Application.Models;

public class PackageGetDTO
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public double Price { get; set; }
    public DateTime PickupStart { get; set; }
    public DateTime PickupEnd { get; set; }
    public int PackageTypeId { get; set; }

}