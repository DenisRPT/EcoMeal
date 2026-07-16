using Microsoft.AspNetCore.Http;

namespace EcoMeal.Backend.Application.Models;

public class BusinessUpdateDTO
{
    public required string Name { get; set; }
    public required string Address { get; set; }
    public string? Description { get; set; }
    public required string Contact { get; set; }
    public int BusinessTypeId { get; set; }
    public IFormFile? ImageFile { get; set; }
}
