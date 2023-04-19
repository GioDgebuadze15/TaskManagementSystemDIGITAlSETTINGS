namespace TMS.Data.Models;

public record SignInResponse(int StatusCode, string? Error, string? Token);