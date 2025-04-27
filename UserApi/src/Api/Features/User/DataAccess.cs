namespace User.Api.Features.User;

public interface IDataAccess
{
    Task<IEnumerable<Entity>> GetAllAsync(CancellationToken cancellationToken);

    Task<Entity?> GetByIdAsync(Guid Id, CancellationToken cancellationToken);
}

[ExcludeFromCodeCoverage]
internal sealed class DataAccess(NpgsqlDataSource npgsqlDataSource) : IDataAccess
{
    public async Task<Entity?> GetByIdAsync(Guid Id, CancellationToken cancellationToken)
    {
        await using var connection = await npgsqlDataSource.OpenConnectionAsync(cancellationToken);
        const string query = @"SELECT * FROM ""user"" WHERE id = @Id";
        return await connection.QueryFirstOrDefaultAsync<Entity>(query, new { Id });
    }

    public async Task<IEnumerable<Entity>> GetAllAsync(CancellationToken cancellationToken)
    {
        await using var connection = await npgsqlDataSource.OpenConnectionAsync(cancellationToken);
        const string query = @"SELECT * FROM ""user""";
        return await connection.QueryAsync<Entity>(query, cancellationToken);
    }
}