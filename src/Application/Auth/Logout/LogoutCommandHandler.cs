using MediatR;
using Application.Data;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using ErrorOr;
using Application.Auth.Dtos;

namespace Application.Auth.Logout;
public class LogoutCommandHandler : IRequestHandler<LogoutCommand, ErrorOr<LogoutResponse>>
{
    private readonly ILoginService _loginService;
    private readonly ILogger<LogoutCommandHandler> _logger;

    public LogoutCommandHandler(ILoginService loginService, ILogger<LogoutCommandHandler> logger)
    {
        _loginService = loginService ?? throw new ArgumentNullException(nameof(loginService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<ErrorOr<LogoutResponse>> Handle(LogoutCommand command, CancellationToken cancellationToken)
    {
        using (LogContext.PushProperty("Identifier", string.Concat("Logout: " + command.RefreshToken)))
        {
            _logger.LogInformation($"Iniciando Logout para el RefreshToken: {command.RefreshToken}\n");

            try
            {
                var response = await _loginService.LogoutAsync(command.RefreshToken);
                if (response is null){
                    _logger.LogError("Logout failed for RefreshToken: {RefreshToken}", command.RefreshToken);
                    return Error.Failure(
                        code: "Logout.Failure",
                        description: "Invalid RefreshToken."
                    );
                }

                _logger.LogInformation($"Logout successful for RefreshToken: {command.RefreshToken}\n");
                return response;
            }
            catch (Exception ex)
            {
                string errorMessage = $"Error logging out user: {ex.Message}";
                _logger.LogError(errorMessage);

                return Error.Failure(
                    code: "User.Logout.Failure",
                    description: errorMessage
                );
            }
        }
    }
}
