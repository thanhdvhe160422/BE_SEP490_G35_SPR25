using Microsoft.AspNetCore.Mvc;
using Planify_BackEnd.DTOs.Login;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
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