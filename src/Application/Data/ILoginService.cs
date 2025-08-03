using Application.Auth.Dtos;
namespace Aplication.Data;
public interface ILoginService
{
    Task<LoginResponse> LoginAsync(string userName, string password);
    Task LogoutAsync(string userName);
    Task RefreshTokenAsync(string userName, string refreshToken); 
}