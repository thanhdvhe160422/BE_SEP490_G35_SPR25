using Microsoft.AspNetCore.Mvc;
using Planify_BackEnd.DTOs.Login;
using Planify_BackEnd.Services.Auth;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("google-login")]
    public IActionResult GoogleLogin([FromBody] GoogleLoginRequestDTO request)
    {
        try
        {
            var response = _authService.GoogleLogin(request);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("refresh-token")]
    public IActionResult RefreshToken([FromBody] RefreshTokenRequestDTO request)
    {
        try
        {
            var response = _authService.RefreshToken(request.RefreshToken, request.AccessToken);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}