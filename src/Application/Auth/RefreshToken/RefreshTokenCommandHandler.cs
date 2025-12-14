using MediatR;
using Application.Data;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using ErrorOr;
using Application.Auth.Dtos;

namespace Application.Auth.RefreshToken;
public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, ErrorOr<RefreshTokenResponse>>
{
    private readonly ILoginService _loginService;
    private readonly ILogger<RefreshTokenCommandHandler> _logger;

    public RefreshTokenCommandHandler(ILoginService loginService, ILogger<RefreshTokenCommandHandler> logger)
    {
        _loginService = loginService ?? throw new ArgumentNullException(nameof(loginService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<ErrorOr<RefreshTokenResponse>> Handle(RefreshTokenCommand command, CancellationToken cancellationToken)
    {
        using (LogContext.PushProperty("Identifier", string.Concat("RefreshToken: " + command.RefreshToken)))
        {
            _logger.LogInformation($"Iniciando RefreshToken para el RefreshToken: {command.RefreshToken}\n");

            try
            {
                var response = await _loginService.RefreshTokenAsync(command.RefreshToken);
                if (response.IsError){
                    _logger.LogError("RefreshToken failed for RefreshToken: {RefreshToken}", command.RefreshToken);
                    return Error.Failure(
                        code: "RefreshToken.Failure",
                        description: "Invalid RefreshToken."
                    );
                }

                _logger.LogInformation($"RefreshToken successful for RefreshToken: {command.RefreshToken}\n");
                return response;
            }
            catch (Exception ex)
            {
                string errorMessage = $"Error refreshing token: {ex.Message}";
                _logger.LogError(errorMessage);

                return Error.Failure(
                    code: "User.RefreshToken.Failure",
                    description: errorMessage
                );
            }
        }
    }
}
