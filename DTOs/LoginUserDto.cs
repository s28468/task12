using System.ComponentModel.DataAnnotations;
using WebApplication1.Entities;

namespace WebApplication1.DTOs;

public class LoginUserDto
{
    [Required]
    public required string Username { get; set; }
    [Required]
    public required string Password { get; set; }
}