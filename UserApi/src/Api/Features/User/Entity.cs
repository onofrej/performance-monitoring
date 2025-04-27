namespace User.Api.Features.User;

[ExcludeFromCodeCoverage]
public sealed class Entity
{
    public Guid Id { get; set; }
    public string? Email { get; set; }
    public string? Name { get; set; }
}