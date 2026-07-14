using System.Security.Claims;
using Azure.Core;
using EcoMeal.Backend.Entities;
using EcoMeal.Backend.Infrastructure;
using EcoMeal.Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EcoMeal.Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OrderController:ControllerBase
{
    private readonly EcoMealDbContext _context;
    public OrderController(EcoMealDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<ActionResult<OrderGetDTO>> PlaceOrder ([FromBody] OrderCreateDTO Request)
    {
        var userId = GetCurrentUserId();
        var package = await _context.Packages
        .Include(p=>p.Business)
        .Include(p=>p.Orders)
        .FirstOrDefaultAsync(p=>p.Id==Request.PackageId);

        if(package is null)
        {
            return NotFound("Pachetul nu a fost gasit");
        }

        if (package.Orders.Any())
        {
            return BadRequest("Pachetul nu mai este disponibil");
        }

        var order = new Order
        {
            UserId = userId,
            PackageId = package.Id,
            Status = "Plasata",
            OrderDate = DateTime.UtcNow
        };

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        return Ok(new OrderGetDTO
        {
            Id = order.Id,
            Date = order.OrderDate,
            Status = order.Status,
            PackageName = package.Name,
            Price = (double)package.Price,
            BusinessId = package.BusinessId,
            BusinessName = package.Business.Name,
            UserName = order.User.Name,
            UserContact = order.User.Contact
        });
    }

    [HttpGet("my")]
    public async Task<ActionResult<IEnumerable<OrderGetDTO>>> GetMyOrders()
    {
        var userId = GetCurrentUserId();
        var orders = await _context.Orders
        .Where(o=>o.UserId == userId)
        .OrderByDescending(o=>o.OrderDate)
        .Select(o=> new OrderGetDTO
        {
            Id = o.Id,
            Date = o.OrderDate,
            Status = o.Status,
            Price = (double)o.Package.Price,
            BusinessId = o.Package.BusinessId,
            BusinessName = o.Package.Business.Name,
            PackageName = o.Package.Name
        }).ToListAsync();

        return Ok(orders);
    }
    private int GetCurrentUserId()
    {
        var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return int.Parse(userIdValue!);
    }
}
