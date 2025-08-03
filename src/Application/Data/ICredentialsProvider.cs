using Application.Auth.Dtos;

namespace Application.Data;
public interface ICredentialsProvider
{
    LoginParam GetLoginParam();
}