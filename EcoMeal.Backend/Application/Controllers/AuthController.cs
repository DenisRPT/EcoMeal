using EcoMeal.Backend.Entities;
using EcoMeal.API.Application.Models.Auth;
using EcoMeal.API.Application.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EcoMeal.API.Application.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<User> _userManager;

    public AuthController(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var user = new User
        {
            UserName = request.Email,
            Email = request.Email,
            Name = request.Name,
            Contact = request.Contact
        };

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
            return BadRequest(new { Errors = result.Errors.Select(e => e.Description) });

        await _userManager.AddToRoleAsync(user, UserRoles.User);

        return Ok(new { Message = "User registered successfully" });
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> GetMe()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return NotFound();

        var roles = await _userManager.GetRolesAsync(user);

        return Ok(new UserMeResponse
        {
            Email = user.Email,
            Name = user.Name,
            Contact = user.Contact,
            Roles = roles
        });
    }

    [HttpPut("me")]
    [Authorize]
    public async Task<IActionResult> UpdateMe([FromBody] UpdateMeRequest request)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return NotFound();

        var normalizedEmail = request.Email?.Trim();
        if (string.IsNullOrWhiteSpace(normalizedEmail))
            return BadRequest(new { Errors = new[] { "Email-ul este obligatoriu." } });

        var existingUser = await _userManager.FindByEmailAsync(normalizedEmail);
        if (existingUser != null && existingUser.Id != user.Id)
            return BadRequest(new { Errors = new[] { "Există deja un cont cu acest email." } });

        user.Email = normalizedEmail;
        user.UserName = normalizedEmail;
        user.Name = request.Name?.Trim();
        user.Contact = request.Contact?.Trim();

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
            return BadRequest(new { Errors = result.Errors.Select(e => e.Description) });

        var roles = await _userManager.GetRolesAsync(user);

        return Ok(new UserMeResponse
        {
            Email = user.Email,
            Name = user.Name,
            Contact = user.Contact,
            Roles = roles
        });
    }
}
