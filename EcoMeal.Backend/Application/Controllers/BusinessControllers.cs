using Microsoft.AspNetCore.Mvc;
using EcoMeal.Backend.Entities;
using EcoMeal.Backend.Infrastructure;


using EcoMeal.Backend.Application.Models;
using Microsoft.EntityFrameworkCore;

namespace EcoMeal.Backend.Application;

[ApiController]
[Route("api/[controller]")]
public class BusinessController : ControllerBase
{
    private readonly EcoMealDbContext _context;
    public BusinessController(EcoMealDbContext context)
    {
        _context = context;
    }
    [HttpGet]
    public async Task<ActionResult<IEnumerable<BusinessDTO>>> GetAll()
    {
        var businessesDTOs = await _context.Business
            .Include(b => b.BusinessType)
            .Select(b => new BusinessDTO
            {
                Id = b.Id,
                Name = b.Name,
                Address = b.Address,
                Description = b.Description,
                Contact = b.Contact,
                BusinessTypeName = b.BusinessType.Name
            })
            .ToListAsync();
        return Ok(businessesDTOs);
    }

[HttpDelete("{id}")]
    public async Task<ActionResult> Delete (int id)
    {
        var business = await _context.Business.FindAsync(id);
        if (business == null)
        {
            return NotFound();
        }

        _context.Business.Remove(business);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BusinessDetailsDTO>> GetOneById(int id)
    {
        var business = await _context.Business
            .Include(b => b.Packages)
            .ThenInclude(p => p.PackageType)
            .Select(b => new BusinessDetailsDTO
            {
                Id = b.Id,
                Name = b.Name,
                Address = b.Address,
                Description = b.Description,
                Contact = b.Contact,
                BusinessTypeName = b.BusinessType.Name,
            })
            .FirstOrDefaultAsync(b => b.Id == id);
        if (business is null)
        {
            return NotFound();
        }

        return Ok(business);
    }

    [HttpPost]
    [Route("{id}/addPackage")]
    public async Task<IActionResult> AddPackageToBusiness(int id,[FromBody] PackageAddDTO package)
    {
         _context.Packages.Add(new Package
        {
            Name = package.Name,
            Description = package.Description,
            Price = (decimal)package.Price,
            PickupStart = package.PickupStart,
            PickupEnd = package.PickupEnd,
            PackageTypeId = package.PackageTypeId,
            BusinessId = id,
        });
        await _context.SaveChangesAsync();
        return Created();
    }
}