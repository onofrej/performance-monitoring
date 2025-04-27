using User.Api.Common;
using User.Api.Features.User.GetAll;
using User.Api.Features.User.GetById;

namespace User.Api.Features.User;

[ExcludeFromCodeCoverage]
public sealed class EndPoints(ILogger<EndPoints> logger) : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/users")
            .WithTags("Users");

        group.MapGet(string.Empty, GetUsersAsync);
        group.MapGet("/{id:guid}", GetByIdAsync);
    }

    public async Task<IResult> GetUsersAsync(ISender _sender,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetAllQuery(), cancellationToken);

        if (result.HasFailed)
        {
            return Results.BadRequest(new Response<Guid>(Guid.Empty, result.Error));
        }

        logger.LogInformation("Users retreived with success - count: {Count}", result.Data!.Count());

        return Results.Ok(new Response<IEnumerable<Response>>(result.Data!.MapToResponse()));
    }

    public async Task<IResult> GetByIdAsync([FromRoute] Guid id, ISender _sender,
        CancellationToken cancellationToken)
    {
        var query = new GetByIdQuery(id);

        var result = await _sender.Send(query, cancellationToken);

        if (result.HasFailed)
        {
            return Results.BadRequest(new Response<Guid>(Guid.Empty, result.Error));
        }

        logger.LogInformation("Profile by id retrieved with success: {Id}", id);

        return Results.Ok(new Response<Response>(result.Data!.MapToResponse()));
    }
}