namespace EcoMeal.Backend.Entities;

public class Business
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Address { get; set; }
    public string? Description { get; set; }
    public required string Contact { get; set; }
    public int BusinessTypeId { get; set; }
    public BusinessType BusinessType { get; set; } = null!;
    public ICollection<Package> Packages { get; set; } = new List<Package>();
    public string? ImagePath { get; set; }
}