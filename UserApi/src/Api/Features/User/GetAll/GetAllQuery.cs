using User.Api.Common;

namespace User.Api.Features.User.GetAll;

[ExcludeFromCodeCoverage]
public record GetAllQuery() : IRequest<Result<IEnumerable<Entity>>>;