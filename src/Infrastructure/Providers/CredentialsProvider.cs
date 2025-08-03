
using Application.Auth.Dtos;
using Application.Data;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Providers;

public class CredentialsProvider : ICredentialsProvider
{
    private readonly ILogger<CredentialsProvider> _logger;

    public CredentialsProvider(ILogger<CredentialsProvider> logger)
    {
        _logger = logger;
    }
    public LoginParam GetLoginParam()
    {
        return new LoginParam("1ejqj12dj47c6dlj79eko0o990XXX", "14tfv3jps0gcerusdhdo4tqr3c6drmj25due3j4bc0tlf4tvsop2");
    }
}
