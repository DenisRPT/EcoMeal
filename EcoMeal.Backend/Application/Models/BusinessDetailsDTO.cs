using EcoMeal.Backend.Application.Models;
namespace EcoMeal.Backend.Application.Models;

public class BusinessDetailsDTO : BusinessDTO
{
    public List<PackageGetDTO> Packages { get; set; } = new();
} 