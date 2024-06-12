using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.DTOs;
using WebApplication1.Entities;
using WebApplication1.Helpers;
using WebApplication1.Services;

namespace Tutorial12.API.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController(ApplicationContext context, IAuthenticationService authService) : ControllerBase
{
    private readonly PasswordHasher<User> _passwordHasher = new();
    [Route("register")]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserDto createUser, CancellationToken cancellationToken)
    {
        if (await context.Users.AnyAsync(u => u.Username == createUser.Username, cancellationToken))
        {
            Console.WriteLine("User already exists.");
            return BadRequest("User already exists.");
        }

        var role = await context.Roles.FirstOrDefaultAsync(r => r.Name == "User", cancellationToken);
        if (role == null)
        {
            Console.WriteLine("Role 'User' not found.");
            return NotFound("Role 'User' not found.");
        }

        var mappedUser = createUser.Map();
        mappedUser.Password = _passwordHasher.HashPassword(mappedUser, mappedUser.Password);
        mappedUser.RoleId = role.Id;

        context.Users.Add(mappedUser);
        Console.WriteLine($"User {mappedUser.Username} added to context.");
        await context.SaveChangesAsync(cancellationToken);
        Console.WriteLine($"User {mappedUser.Username} saved to database.");

        return CreatedAtAction(nameof(CreateUser), new { id = mappedUser.Id }, mappedUser);
    }


    [Route("login")]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<AuthDto>> LoginUser([FromBody] LoginUserDto loginUser,
        CancellationToken cancellationToken)
    {
        var databaseUser = await context.Users.Include(u => u.Role).FirstOrDefaultAsync(
            u => u.Username == loginUser.Username,
            cancellationToken: cancellationToken);
        if (databaseUser is null)
        {
            return Unauthorized();
        }

        var verificationResult =
            _passwordHasher.VerifyHashedPassword(databaseUser, databaseUser.Password, loginUser.Password);
        if (verificationResult == PasswordVerificationResult.Failed)
        {
            return Unauthorized();
        }

        var authResponse = new AuthDto
        {
            AccessToken = authService.GenerateAccessToken(databaseUser),
            RefreshToken = authService.GenerateRefreshToken()
        };

        databaseUser.RefreshToken = authResponse.RefreshToken;
        databaseUser.RefreshTokenExpire = DateTime.Now.AddDays(1);

        context.Entry(databaseUser).State = EntityState.Modified;
        context.Users.Update(databaseUser);
        await context.SaveChangesAsync(cancellationToken);

        return Ok(authResponse);
    }

    [Route("refresh")]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<AuthDto>> RefreshToken([FromBody] AuthDto auth, CancellationToken cancellationToken)
    {
        var isTokenValid = await authService.ValidateExpiredAccessTokenAsync(auth.AccessToken);
        if (isTokenValid is false)
        {
            return Forbid();
        }

        var currentUser = await context.Users.Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.RefreshToken == auth.RefreshToken, cancellationToken: cancellationToken);
        if (currentUser is null || currentUser.RefreshTokenExpire < DateTime.Now)
        {
            return Forbid();
        }

        var authResponse = new AuthDto
        {
            AccessToken = authService.GenerateAccessToken(currentUser),
            RefreshToken = authService.GenerateRefreshToken()
        };

        currentUser.RefreshToken = authResponse.RefreshToken;
        currentUser.RefreshTokenExpire = DateTime.Now.AddDays(1);

        context.Entry(currentUser).State = EntityState.Modified;
        context.Users.Update(currentUser);
        await context.SaveChangesAsync(cancellationToken);

        return Ok(authResponse);
    }
}