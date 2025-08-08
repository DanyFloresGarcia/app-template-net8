using Application.Auth.Dtos;
using ErrorOr;
using MediatR;

namespace Application.Auth.Logout;

public record LogoutCommand(
    string RefreshToken
) : IRequest<ErrorOr<LogoutResponse>>;