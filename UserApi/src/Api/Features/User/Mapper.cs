namespace User.Api.Features.User;

public static class Mapper
{
    public static Response MapToResponse(this Entity userEntity)
    {
        return new Response(userEntity.Id,
            userEntity.Email,
            userEntity.Name);
    }

    public static IEnumerable<Response> MapToResponse(this IEnumerable<Entity> userEntities)
    {
        foreach (var userEntity in userEntities)
        {
            yield return new Response(userEntity.Id,
                userEntity.Email,
                userEntity.Name);
        }
    }
}