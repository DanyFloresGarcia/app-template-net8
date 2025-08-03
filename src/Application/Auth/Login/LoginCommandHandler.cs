using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Aplication.Data;
using Domain.Primitives;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using ErrorOr;
using Application.Auth.Dtos;

namespace Application.Auth.Login;
public class LoginCommandHandler : IRequestHandler<LoginCommand, ErrorOr<LoginResponse>>
{
    private readonly ILoginService _loginService;
    private readonly ILogger<LoginCommandHandler> _logger;

    public LoginCommandHandler(ILoginService loginService, ILogger<LoginCommandHandler> logger)
    {
        _loginService = loginService ?? throw new ArgumentNullException(nameof(loginService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<ErrorOr<LoginResponse>> Handle(LoginCommand command, CancellationToken cancellationToken)
    {
        using (LogContext.PushProperty("Identifier", string.Concat("Login: " + string.Concat(command.UserName, " ", command.Password))))
        {
            _logger.LogInformation("Iniciando Login para el usuario: {UserName}", command.UserName);

            try
            {
                var response = await _loginService.LoginAsync(command.UserName, command.Password);
                if (response is null){
                    _logger.LogError("Login failed for user: {UserName}", command.UserName);
                    return Error.Failure(
                        code: "Login.Failure",
                        description: "Invalid username or password."
                    );
                }

                _logger.LogInformation("Login successful for user: {UserName}", command.UserName);
                return response;
            }
            catch (Exception ex)
            {
                string errorMessage = $"Error logging in user: {ex.Message}";
                _logger.LogError(errorMessage);

                return Error.Failure(
                    code: "User.Login.Failure",
                    description: errorMessage
                );
            }
        }
    }
}
