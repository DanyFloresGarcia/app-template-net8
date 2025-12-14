using Application.Auth.Dtos;
using ErrorOr;
namespace Application.Auth;
public interface ILoginService
{
    Task<ErrorOr<LoginResponse>> LoginAsync(string userName, string password);
    Task<ErrorOr<LogoutResponse>> LogoutAsync(string refreshToken);
    Task<ErrorOr<RefreshTokenResponse>> RefreshTokenAsync(string refreshToken);
}