using User.Api.Common;
using User.Api.Features.User.GetAll;
using User.Api.Features.User.GetById;

namespace User.Api.Features.User;

[ExcludeFromCodeCoverage]
public sealed class EndPoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder endpointRouteBuilder)
    {
        var group = endpointRouteBuilder.MapGroup("/users")
            .WithTags("Users");

        group.MapGet(string.Empty, GetUsersAsync);
        group.MapGet("/{id:guid}", GetByIdAsync);
    }

    public static async Task<IResult> GetUsersAsync(ISender _sender,
        ILogger<EndPoints> logger,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetAllQuery(), cancellationToken);

        if (result.HasFailed)
        {
            return Results.BadRequest(new Response<Guid>(Guid.Empty, result.Error));
        }

        logger.LogInformation("Users retrieved with success - count: {Count}", result.Data!.Count());

        return Results.Ok(new Response<IEnumerable<Response>>(result.Data!.MapToResponse()));
    }

    public static async Task<IResult> GetByIdAsync([FromRoute] Guid id, ISender _sender,
        ILogger<EndPoints> logger,
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