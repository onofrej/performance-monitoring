namespace User.Api.IntegrationTests.Fixtures.PostgreSql;

internal sealed class PostgreSqlFixture(IConfiguration _configuration) : IDisposable
{
    private readonly string _connectionString = _configuration.GetSection("PostgreSQL:ConnectionString").Value!;

    public async Task<T?> GetByIdAsync<T>(string query, object queryParams, CancellationToken cancellationToken)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);
        return await connection.QueryFirstOrDefaultAsync<T?>(query, queryParams);
    }

    public async Task<int> CreateAsync<T>(T entity, string query, CancellationToken cancellationToken)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);
        return await connection.ExecuteAsync(query, entity);
    }

    internal async Task TruncateTableAsync(string tableName, CancellationToken cancellationToken)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);
        await connection.ExecuteAsync($"TRUNCATE TABLE {tableName};");
    }

    public void Dispose()
    {
    }
}