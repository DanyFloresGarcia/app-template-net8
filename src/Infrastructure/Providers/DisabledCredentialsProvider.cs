
using Application.Auth.Params;
using Application.Auth;

namespace Infrastructure.Providers;
public sealed class DisabledCredentialsProvider : ICredentialsProvider
{
    public LoginParam GetLoginParam()
    {
        throw new InvalidOperationException(
            "Credenciales deshabilitadas en este host"
        );
    }
}
