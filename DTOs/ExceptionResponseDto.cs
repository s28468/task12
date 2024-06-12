using System.Net;

namespace WebApplication1.DTOs;

public record ExceptionResponseDto(HttpStatusCode StatusCode, string Description);