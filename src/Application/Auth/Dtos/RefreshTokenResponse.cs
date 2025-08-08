namespace Application.Auth.Dtos;

public record RefreshTokenResponse(string IdToken, string AccessToken, int? ExpiresIn);