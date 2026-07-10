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
                BusinessTypeName = b.BusinessType.Name,
                BusinessTypeId = b.BusinessTypeId
            })
            .ToListAsync();
        return Ok(businessesDTOs);
    }

    [HttpPost]
    public async Task<IActionResult> CreateBusiness([FromBody] BusinessAddDTO business)
    {
        var newBusiness = new Business
        {
            Name = business.Name,
            Address = business.Address,
            Description = business.Description,
            Contact = business.Contact,
            BusinessTypeId = business.BusinessTypeId
        };

        _context.Business.Add(newBusiness);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetOneById), new { id = newBusiness.Id }, null);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBusiness(int id, [FromBody] BusinessUpdateDTO business)
    {
        var existingBusiness = await _context.Business.FindAsync(id);
        if (existingBusiness is null)
        {
            return NotFound();
        }

        existingBusiness.Name = business.Name;
        existingBusiness.Address = business.Address;
        existingBusiness.Description = business.Description;
        existingBusiness.Contact = business.Contact;
        existingBusiness.BusinessTypeId = business.BusinessTypeId;

        await _context.SaveChangesAsync();
        return NoContent();
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
            .Include(b => b.BusinessType)
            .Select(b => new BusinessDetailsDTO
            {
                Id = b.Id,
                Name = b.Name,
                Address = b.Address,
                Description = b.Description,
                Contact = b.Contact,
                BusinessTypeName = b.BusinessType.Name,
                BusinessTypeId = b.BusinessTypeId,
                Packages = b.Packages.Select(p => new PackageGetDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description ?? string.Empty,
                    Price = (double)p.Price,
                    PickupStart = p.PickupStart,
                    PickupEnd = p.PickupEnd,
                    PackageTypeId = p.PackageTypeId
                }).ToList()
            })
            .FirstOrDefaultAsync(b => b.Id == id);
        if (business is null)
        {
            return NotFound();
        }

        return Ok(business);
    }

    [HttpGet("businessTypes")]
    public async Task<ActionResult<IEnumerable<BusinessTypeDTO>>> GetBusinessTypes()
    {
        var businessTypes = await _context.BusinessTypes
            .Select(bt => new BusinessTypeDTO
            {
                Id = bt.Id,
                Name = bt.Name
            })
            .ToListAsync();

        return Ok(businessTypes);
    }

    [HttpGet("packageTypes")]
    public async Task<ActionResult<IEnumerable<PackageTypeDTO>>> GetPackageTypes()
    {
        var packageTypes = await _context.PackageTypes
            .Select(pt => new PackageTypeDTO
            {
                Id = pt.Id,
                Name = pt.Name
            })
            .ToListAsync();

        return Ok(packageTypes);
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

    [HttpPut("{businessId}/packages/{packageId}")]
    public async Task<IActionResult> UpdatePackage(int businessId, int packageId, [FromBody] PackageUpdateDTO package)
    {
        var existingPackage = await _context.Packages
            .FirstOrDefaultAsync(p => p.Id == packageId && p.BusinessId == businessId);

        if (existingPackage is null)
        {
            return NotFound();
        }

        existingPackage.Name = package.Name;
        existingPackage.Description = package.Description;
        existingPackage.Price = (decimal)package.Price;
        existingPackage.PickupStart = package.PickupStart;
        existingPackage.PickupEnd = package.PickupEnd;
        existingPackage.PackageTypeId = package.PackageTypeId;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{businessId}/packages/{packageId}")]
    public async Task<IActionResult> DeletePackage(int businessId, int packageId)
    {
        var existingPackage = await _context.Packages
            .FirstOrDefaultAsync(p => p.Id == packageId && p.BusinessId == businessId);

        if (existingPackage is null)
        {
            return NotFound();
        }

        _context.Packages.Remove(existingPackage);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}