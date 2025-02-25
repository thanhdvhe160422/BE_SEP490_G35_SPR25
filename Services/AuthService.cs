using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Planify_BackEnd.Models;
using Planify_BackEnd.Entities;
using Google.Apis.Auth;
using Planify_BackEnd.DTOs.Login;

public class AuthService
{
    private readonly IConfiguration _configuration;
    private readonly IUserRepository _userRepository;

    public AuthService(IConfiguration configuration, IUserRepository userRepository)
    {
        _configuration = configuration;
        _userRepository = userRepository;
    }

    public AuthResponseDTO GoogleLogin(GoogleLoginRequestDTO request)
    {
        var googleUserInfo = ValidateGoogleToken(request.GoogleToken);
        if (googleUserInfo == null || string.IsNullOrEmpty(googleUserInfo.Email))
        {
            throw new Exception("Invalid Google token or email not provided.");
        }

        var user = _userRepository.GetUserByEmail(googleUserInfo.Email);
        if (user == null || request.CampusName != user.Campus.CampusName)
        {
            throw new Exception("Email not found in the system. Please register or contact an admin.");
        }

        var jwtToken = GenerateJwtToken(user);

        var refreshToken = GenerateRefreshToken();

        return new AuthResponseDTO
        {
            UserId = user.Id,
            FullName = user.FirstName + " " + user.LastName,
            Email = user.Email,
            Campus = user.Campus.CampusName,
            Role = user.RoleNavigation.RoleName,
            AccessToken = jwtToken,
            RefreshToken = refreshToken
        };
    }
    private GoogleUserInfo ValidateGoogleToken(string googleToken)
    {
        try
        {
            var payload = GoogleJsonWebSignature.ValidateAsync(googleToken).Result;

            return new GoogleUserInfo
            {
                Email = payload.Email,
                Name = payload.Name
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] Google Token Validation Failed: {ex.Message}");
            return null;
        }
    }


    private string GenerateJwtToken(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role.ToString() ?? "1"),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpiryMinutes"])),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    public AuthResponseDTO RefreshToken(string refreshToken, string accessToken)
    {
        if (string.IsNullOrEmpty(refreshToken))
        {
            throw new Exception("Invalid refresh token.");
        }

        var handler = new JwtSecurityTokenHandler();
        var token = handler.ReadJwtToken(accessToken);
        var userId = token.Claims.First(c => c.Type == JwtRegisteredClaimNames.Sub).Value;

        var user = _userRepository.GetUserById(Guid.Parse(userId));
        if (user == null)
        {
            throw new Exception("User not found.");
        }

        var newJwtToken = GenerateJwtToken(user);
        var newRefreshToken = GenerateRefreshToken();

        return new AuthResponseDTO
        {
            UserId = user.Id,
            FullName = user.FirstName + " " + user.LastName,
            Email = user.Email,
            Campus = user.Campus.CampusName,
            Role = user.RoleNavigation.RoleName,
            AccessToken = newJwtToken,
            RefreshToken = newRefreshToken
        };
    }
}