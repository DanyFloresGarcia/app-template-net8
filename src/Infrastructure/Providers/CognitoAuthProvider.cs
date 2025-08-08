using Application.Auth.Dtos;
using Application.Data;
using Microsoft.Extensions.Logging;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Application.Auth.Params;

namespace Infrastructure.Providers;

public class CognitoAuthProvider(ICredentialsProvider credentialsProvider, ILogger<CognitoAuthProvider> logger) : ILoginService
{
    private readonly ICredentialsProvider _credentialsProvider = credentialsProvider ?? throw new ArgumentNullException(nameof(credentialsProvider));
    private readonly ILogger<CognitoAuthProvider> _logger = logger ?? throw new ArgumentNullException(nameof(logger));


    public async Task<LoginResponse> LoginAsync(string userName, string password)
    {
        // Crear cliente de Cognito
        var cognito = new AmazonCognitoIdentityProviderClient();

        LoginParam loginParam = _credentialsProvider.GetLoginParam();

        string clientId = loginParam.ClientId;
        string clientSecret = loginParam.ClientSecret;

        if (string.IsNullOrEmpty(clientId))
        {
            throw new Exception("COGNITO_CLIENT_ID environment variable is not set.");
        }
        if (string.IsNullOrEmpty(clientSecret))
        {
            throw new Exception("COGNITO_CLIENT_SECRET environment variable is not set.");
        }
        // Preparar la solicitud de autenticación
        var authRequest = new InitiateAuthRequest
        {
            AuthFlow = AuthFlowType.USER_PASSWORD_AUTH,
            ClientId = clientId,
            AuthParameters = new Dictionary<string, string>
                {
                    { "USERNAME", userName },
                    { "PASSWORD", password },
                    { "SECRET_HASH", CalculateSecretHash(userName, clientId, clientSecret) }
                }
        };

        // Llamar a Cognito
        var authResponse = await cognito.InitiateAuthAsync(authRequest);

        return new LoginResponse(authResponse.AuthenticationResult.IdToken,
                                 authResponse.AuthenticationResult.AccessToken,
                                 authResponse.AuthenticationResult.RefreshToken,
                                 authResponse.AuthenticationResult.ExpiresIn);
    }
    public async Task<LogoutResponse> LogoutAsync(string refreshToken)
    {
        var cognito = new AmazonCognitoIdentityProviderClient();

        LoginParam loginParam = _credentialsProvider.GetLoginParam();
        string clientId = loginParam.ClientId;
        string clientSecret = loginParam.ClientSecret;

        if (string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(clientSecret))
        {
            throw new Exception("COGNITO_CLIENT_ID or CLIENT_SECRET is not set.");
        }

        var revokeRequest = new RevokeTokenRequest
        {
            ClientId = clientId,
            ClientSecret = clientSecret,
            Token = refreshToken
        };

        await cognito.RevokeTokenAsync(revokeRequest);

        return new LogoutResponse(true, "Token revoked successfully");
    }

    public async Task<RefreshTokenResponse> RefreshTokenAsync(string refreshToken)
    {
        var cognito = new AmazonCognitoIdentityProviderClient();

        LoginParam loginParam = _credentialsProvider.GetLoginParam();
        string clientId = loginParam.ClientId;
        string clientSecret = loginParam.ClientSecret;

        if (string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(clientSecret))
        {
            throw new Exception("COGNITO_CLIENT_ID or CLIENT_SECRET is not set.");
        }

        var refreshRequest = new InitiateAuthRequest
        {
            AuthFlow = AuthFlowType.REFRESH_TOKEN_AUTH,
            ClientId = clientId,
            AuthParameters = new Dictionary<string, string>
                {
                { "REFRESH_TOKEN", refreshToken },
                { "SECRET_HASH", CalculateSecretHash("", clientId, clientSecret) }
            }
        };

        var response = await cognito.InitiateAuthAsync(refreshRequest);

        return new RefreshTokenResponse(
            response.AuthenticationResult.IdToken,
            response.AuthenticationResult.AccessToken,
            response.AuthenticationResult.ExpiresIn
        );
    }

    private static string CalculateSecretHash(string username, string clientId, string clientSecret)
    {
        var keyBytes = System.Text.Encoding.UTF8.GetBytes(clientSecret);
        var messageBytes = System.Text.Encoding.UTF8.GetBytes(username + clientId);

        using var hmac = new System.Security.Cryptography.HMACSHA256(keyBytes);
        var hash = hmac.ComputeHash(messageBytes);
        return Convert.ToBase64String(hash);
    }
}