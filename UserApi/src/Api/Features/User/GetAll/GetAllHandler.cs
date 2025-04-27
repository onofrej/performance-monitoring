using User.Api.Common;

namespace User.Api.Features.User.GetAll;

[ExcludeFromCodeCoverage]
internal sealed class GetAllHandler(IDataAccess dataAccess) : IRequestHandler<GetAllQuery, Result<IEnumerable<Entity>>>
{
    public async Task<Result<IEnumerable<Entity>>> Handle(GetAllQuery request, CancellationToken cancellationToken)
    {
        return new Result<IEnumerable<Entity>>(await dataAccess.GetAllAsync(cancellationToken));
    }
}