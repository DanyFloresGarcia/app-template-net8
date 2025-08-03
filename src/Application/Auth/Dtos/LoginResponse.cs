namespace Application.Auth.Dtos;

public record LoginResponse(string IdToken, string AccessToken, string RefreshToken); 