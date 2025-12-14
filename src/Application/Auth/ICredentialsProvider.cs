using Application.Auth.Params;

namespace Application.Auth;
public interface ICredentialsProvider
{
    LoginParam GetLoginParam();
}