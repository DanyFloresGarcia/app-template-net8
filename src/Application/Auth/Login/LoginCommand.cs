using Application.Auth.Dtos;
using ErrorOr;
using MediatR;

namespace Application.Auth.Login;

public record LoginCommand(
    string UserName,
    string Password
) : IRequest<ErrorOr<LoginResponse>>;