using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Planify_BackEnd.Models;
using Planify_BackEnd.Entities;
using Google.Apis.Auth;
using Planify_BackEnd.DTOs.Login;
using Planify_BackEnd.Services.Auths;
using Planify_BackEnd.DTOs;

public class AuthService : IAuthService
{
    private readonly IConfiguration _configuration;
    private readonly IUserRepository _userRepository;

    public AuthService(IConfiguration configuration, IUserRepository userRepository)
    {
        _configuration = configuration;
        _userRepository = userRepository;
    }

    public async Task<ResponseDTO> GoogleLoginAsync(GoogleLoginRequestDTO request)
    {
        var googleUserInfo = await ValidateGoogleTokenAsync(request.GoogleToken);
        if (googleUserInfo == null || string.IsNullOrEmpty(googleUserInfo.Email))
        {
            return new ResponseDTO(400, "Invalid Google token or email not provided.", null);
        }

        var user = await _userRepository.GetUserByEmailAsync(googleUserInfo.Email);
        if (user == null || !string.Equals(request.CampusName, user.Campus.CampusName, StringComparison.OrdinalIgnoreCase))
        {
            return new ResponseDTO(401, "Email not found in the system. Please register or contact an admin.", null);
        }

        var jwtToken = GenerateJwtToken(user);
        var refreshToken = GenerateRefreshToken();

        return new ResponseDTO(200, "Login successful!", new AuthResponseDTO
        {
            UserId = user.Id,
            FullName = $"{user.FirstName} {user.LastName}",
            Email = user.Email,
            Campus = user.Campus.CampusName,
            Role = user.UserRoles?.FirstOrDefault()?.Role?.RoleName,
            AccessToken = jwtToken,
            RefreshToken = refreshToken
        });
    }

    private async Task<GoogleUserInfo> ValidateGoogleTokenAsync(string googleToken)
    {
        try
        {
            var payload = await GoogleJsonWebSignature.ValidateAsync(googleToken);
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
            new Claim(ClaimTypes.Role, user.UserRoles?.FirstOrDefault()?.Role?.RoleName),
            new Claim("campusId", user.CampusId.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:AccessTokenExpirationMinutes"])),
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

    public async Task<AuthResponseDTO> RefreshToken(string refreshToken, string accessToken)
    {
        if (string.IsNullOrEmpty(refreshToken))
        {
            throw new Exception("Invalid refresh token.");
        }

        var handler = new JwtSecurityTokenHandler();
        var token = handler.ReadJwtToken(accessToken);

        var expClaim = token.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Exp)?.Value;
        if (expClaim == null)
        {
            throw new Exception("Invalid access token.");
        }

        var expiryDate = DateTimeOffset.FromUnixTimeSeconds(long.Parse(expClaim)).UtcDateTime;

        if (expiryDate > DateTime.Now)
        {
            throw new Exception("Access token is still valid.");
        }

        var userId = token.Claims.First(c => c.Type == JwtRegisteredClaimNames.Sub).Value;

        var user = await _userRepository.GetUserByIdAsync(Guid.Parse(userId));
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
            Role = user.UserRoles?.FirstOrDefault()?.Role?.RoleName,
            AccessToken = newJwtToken,
            RefreshToken = newRefreshToken
        };
    }

}