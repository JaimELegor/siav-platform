using Microsoft.AspNetCore.Mvc;
using SIAV.Application.DTOs.Auth;
using SIAV.Application.Interfaces;

namespace SIAV.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        try
        {
            var result = await authService.RegisterAsync(dto);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        try
        {
            var result = await authService.LoginAsync(dto);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return Unauthorized(new { mensaje = ex.Message });
        }
    }

    [HttpGet("debug-claims")]
    public IActionResult DebugClaims()
    {
        var authHeader = Request.Headers["Authorization"].ToString();
        var claims = User.Claims.Select(c => new { c.Type, c.Value });
        return Ok(new { authHeader, claims });
    }

}