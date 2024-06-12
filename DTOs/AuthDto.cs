namespace WebApplication1.DTOs;

public class AuthDto
{
    public required string AccessToken { get; set; }
    
    public required string RefreshToken { get; set; }
}