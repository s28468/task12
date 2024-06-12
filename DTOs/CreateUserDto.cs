using System.ComponentModel.DataAnnotations;
using WebApplication1.Entities;

namespace WebApplication1.DTOs;

public class CreateUserDto
{
    [Required]
    [Length(8, 50)]
    public required string Username { get; set; }
    
    [Required]
    [Length(8, 32)]
    public required string Password { get; set; }

    public User Map()
    {
        return new User
        {
            Username = Username,
            Password = Password
        };
    }
}