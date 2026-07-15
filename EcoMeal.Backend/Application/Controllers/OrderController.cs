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
            .Include(p => p.Business)
            .FirstOrDefaultAsync(p => p.Id == Request.PackageId);

        if (package is null)
        {
            return NotFound("Pachetul nu a fost gasit");
        }

        if (package.IsReserved || package.PickupEnd <= DateTime.UtcNow)
        {
            return BadRequest("Pachetul nu mai este disponibil");
        }

        // reserve the package
        package.IsReserved = true;

        var order = new Order
        {
            UserId = userId,
            PackageId = package.Id,
            Status = OrderStatus.Rezervat,
            OrderDate = DateTime.UtcNow
        };

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        var user = await _context.Users.FindAsync(userId);

        return Ok(new OrderGetDTO
        {
            Id = order.Id,
            Date = order.OrderDate,
            Status = order.Status.ToString(),
            PackageName = package.Name,
            Price = (double)package.Price,
            BusinessId = package.BusinessId,
            BusinessName = package.Business.Name,
            UserName = user?.Name,
            UserContact = user?.Contact
        });
    }

    [HttpGet("my")]
    public async Task<ActionResult<IEnumerable<OrderGetDTO>>> GetMyOrders()
    {
        var userId = GetCurrentUserId();
        var orders = await _context.Orders
            .Where(o => o.UserId == userId)
            .OrderByDescending(o => o.OrderDate)
            .Select(o => new OrderGetDTO
            {
                Id = o.Id,
                Date = o.OrderDate,
                Status = o.Status.ToString(),
                Price = (double)o.Package!.Price,
                BusinessId = o.Package!.BusinessId,
                BusinessName = o.Package!.Business!.Name,
                PackageName = o.Package!.Name
            }).ToListAsync();

        return Ok(orders);
    }
    
    [HttpGet("admin")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IEnumerable<OrderGetDTO>>> GetAllOrdersForAdmin()
    {
        var orders = await _context.Orders
            .Include(o => o.User)
            .Include(o => o.Package!)
                .ThenInclude(p => p.Business)
            .OrderByDescending(o => o.OrderDate)
            .Select(o => new OrderGetDTO
            {
                Id = o.Id,
                Date = o.OrderDate,
                Status = o.Status.ToString(),
                Price = (double)o.Package!.Price,
                BusinessId = o.Package!.BusinessId,
                BusinessName = o.Package!.Business!.Name,
                PackageName = o.Package!.Name,
                UserName = o.User!.Name,
                UserContact = o.User!.Contact
            }).ToListAsync();

        return Ok(orders);
    }
    private int GetCurrentUserId()
    {
        var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return int.Parse(userIdValue!);
    }
}
