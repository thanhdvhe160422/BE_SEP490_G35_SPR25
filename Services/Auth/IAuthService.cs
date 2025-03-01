using Planify_BackEnd.DTOs.Login;
using Planify_BackEnd.Entities;

namespace Planify_BackEnd.Services.Auth
{
    public interface IAuthService
    {
        AuthResponseDTO GoogleLogin(GoogleLoginRequestDTO request);
        AuthResponseDTO RefreshToken(string refreshToken, string accessToken);
    }
}
