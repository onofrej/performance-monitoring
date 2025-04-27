using User.Api.Common;

namespace User.Api.Features.User.GetById;

[ExcludeFromCodeCoverage]
internal sealed class GetByIdHandler(IDataAccess userData, IValidator<GetByIdQuery> validator) : IRequestHandler<GetByIdQuery, Result<Entity>>
{
    public async Task<Result<Entity>> Handle(GetByIdQuery request, CancellationToken cancellationToken)
    {
        var validationResult = validator.Validate(request);

        if (!validationResult.IsValid)
        {
            return new Result<Entity>(default, Errors.ReturnInvalidEntriesError(validationResult.ToString()));
        }

        var userEntity = await userData.GetByIdAsync(request.Id, cancellationToken);

        if (userEntity is null)
        {
            return new Result<Entity>(default, Errors.ReturnUserNotFoundError());
        }

        return new Result<Entity>(userEntity);
    }
}