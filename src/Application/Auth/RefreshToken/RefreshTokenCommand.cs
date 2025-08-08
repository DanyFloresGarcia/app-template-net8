using Application.Auth.Dtos;
using ErrorOr;
using MediatR;

namespace Application.Auth.RefreshToken;

public record RefreshTokenCommand(
    string RefreshToken
) : IRequest<ErrorOr<RefreshTokenResponse>>;