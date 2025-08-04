using Application.Auth.Params;

namespace Application.Data;
public interface ICredentialsProvider
{
    LoginParam GetLoginParam();
}