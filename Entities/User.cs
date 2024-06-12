namespace WebApplication1.Entities;

public class User
{
    public int Id { get; set; }
    
    public required string Username { get; set; }
    
    public required string Password { get; set; }
    
    public string? RefreshToken { get; set; }
    
    public DateTime RefreshTokenExpire { get; set; }
    public int RoleId { get; set; }
    
    public Role Role { get; set; }
}