namespace User.Api.Common;

[ExcludeFromCodeCoverage]
public record Response<T>(T? Data = default, object? Errors = default);