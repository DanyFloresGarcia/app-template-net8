using Application.Auth.Dtos;
using Application.Auth;
using Microsoft.Extensions.Logging;
using ErrorOr;

namespace Infrastructure.Providers;

public class DisabledAuthProvider(ILogger<DisabledAuthProvider> logger) : ILoginService
{
    private readonly ILogger<DisabledAuthProvider> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public Task<ErrorOr<LoginResponse>> LoginAsync(string userName, string password)
    {
        _logger.LogWarning("Login deshabilitado en este host");
        return Task.FromResult<ErrorOr<LoginResponse>>(
         Error.Failure(
             code: "Auth.Login.Disabled",
             description: "Login deshabilitado en este host"
         ));
    }

    public Task<ErrorOr<LogoutResponse>> LogoutAsync(string refreshToken)
    {
        _logger.LogWarning("Logout deshabilitado en este host");
        return Task.FromResult<ErrorOr<LogoutResponse>>(
         Error.Failure(
             code: "Auth.Logout.Disabled",
             description: "Logout deshabilitado en este host"
         ));
    }

    public Task<ErrorOr<RefreshTokenResponse>> RefreshTokenAsync(string refreshToken)
    {
        _logger.LogWarning("RefreshToken deshabilitado en este host");
        return Task.FromResult<ErrorOr<RefreshTokenResponse>>(
         Error.Failure(
             code: "Auth.RefreshToken.Disabled",
             description: "RefreshToken deshabilitado en este host"
         ));
    }
}