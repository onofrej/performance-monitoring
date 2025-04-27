namespace User.Api.Features.User;

[ExcludeFromCodeCoverage]
public sealed record Response(
    Guid Id,
    string? Email,
    string? Name);