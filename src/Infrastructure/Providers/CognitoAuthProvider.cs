using Microsoft.EntityFrameworkCore;

using Application.Auth.Dtos;
using Aplication.Data;
using Microsoft.Extensions.Logging;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Application.Data;

namespace Infrastructure.Providers;

public class CognitoAuthProvider(ICredentialsProvider credentialsProvider, ILogger<CognitoAuthProvider> logger) : ILoginService
{
    private readonly ICredentialsProvider _credentialsProvider = credentialsProvider ?? throw new ArgumentNullException(nameof(credentialsProvider));
    private readonly ILogger<CognitoAuthProvider> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    

    public async Task<LoginResponse> LoginAsync(string userName, string password)
    {
            // Crear cliente de Cognito
            var cognito = new AmazonCognitoIdentityProviderClient();

            string clientId = _credentialsProvider.GetLoginParam().ClientId;
            string clientSecret = _credentialsProvider.GetLoginParam().ClientSecret;

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
                AuthFlow = AuthFlowType.USER_PASSWORD_AUTH, // o USER_SRP_AUTH si configuras SRP
                ClientId = clientId,
                AuthParameters = new Dictionary<string, string>
                {
                    { "USERNAME", userName },
                    { "PASSWORD", password },
                    { "SECRET_HASH", CalculateSecretHash(userName, clientId, clientSecret) }
                }
            };

            Console.WriteLine("COGNITO_CLIENT_ID: " + authRequest.ClientId);
            
            // Llamar a Cognito
            var authResponse = await cognito.InitiateAuthAsync(authRequest);

            return new LoginResponse(authResponse.AuthenticationResult.IdToken, authResponse.AuthenticationResult.AccessToken, authResponse.AuthenticationResult.RefreshToken);
    }
    public Task LogoutAsync(string userName)
    {
        // Implement login logic here
        throw new NotImplementedException();
    }

    public Task RefreshTokenAsync(string userName, string refreshToken)
    {
        // Implement login logic here
        throw new NotImplementedException();
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