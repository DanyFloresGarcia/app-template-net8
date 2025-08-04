
using Application.Auth.Params;
using Application.Data;
using Infrastructure.Common;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Providers;

public class CognitoCredentialsProvider : ICredentialsProvider
{
    private readonly ILogger<CognitoCredentialsProvider> _logger;
    private readonly LoginCredentials _loginCredentials;

    public CognitoCredentialsProvider(LoginCredentials loginCredentials, ILogger<CognitoCredentialsProvider> logger)
    {
        _loginCredentials = loginCredentials;
        _logger = logger;
    }
    public LoginParam GetLoginParam()
    {
        var clientId = _loginCredentials.ClientId;
        var clientSecret = _loginCredentials.ClientSecret;
        
        _logger.LogInformation($"COGNITO_CLIENT_ID {clientId}\n");
        
        if (string.IsNullOrEmpty(clientId))
        {
            _logger.LogError("COGNITO_CLIENT_ID environment variable is not set.");
            throw new Exception("COGNITO_CLIENT_ID environment variable is not set.");
        }
        if (string.IsNullOrEmpty(clientSecret))
        {
            _logger.LogError("COGNITO_CLIENT_SECRET environment variable is not set.");
            throw new Exception("COGNITO_CLIENT_SECRET environment variable is not set.");
        }

        return new LoginParam(clientId, clientSecret);
    }
}
