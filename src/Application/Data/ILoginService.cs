using Application.Auth.Dtos;
namespace Application.Data;
public interface ILoginService
{
    Task<LoginResponse> LoginAsync(string userName, string password);
    Task<LogoutResponse> LogoutAsync(string refreshToken);
    Task<RefreshTokenResponse> RefreshTokenAsync(string refreshToken);
}