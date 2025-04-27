using User.Api.Common;

namespace User.Api.Features.User.GetById;

[ExcludeFromCodeCoverage]
public record GetByIdQuery(Guid Id) : IRequest<Result<Entity>>;