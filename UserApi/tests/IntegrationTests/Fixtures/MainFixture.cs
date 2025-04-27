using User.Api.IntegrationTests.Factories;
using User.Api.IntegrationTests.Fixtures.PostgreSql;

namespace User.Api.IntegrationTests.Fixtures;

public sealed class MainFixture : IDisposable
{
    private readonly IConfiguration _configuration;
    private readonly PostgreSqlFixture _postgreSqlFixture;
    private readonly CustomWebApplicationFactory _customWebApplicationFactory;
    private readonly HttpClient _httpClient;

    public MainFixture()
    {
        _configuration = new ConfigurationBuilder()
          .SetBasePath(AppContext.BaseDirectory)
          .AddJsonFile("integrationtests-settings.json", optional: false)
          .Build();

        InitializeEnvironmentVariables();

        _postgreSqlFixture = new PostgreSqlFixture(_configuration);
        _customWebApplicationFactory = new CustomWebApplicationFactory();
        _httpClient = _customWebApplicationFactory.CreateClient();
    }

    internal HttpClient HttpClient => _httpClient;
    internal IConfiguration Configuration => _configuration;
    internal PostgreSqlFixture PostgreSqlFixture => _postgreSqlFixture;

    public void Dispose()
    {
        _postgreSqlFixture.Dispose();

        GC.SuppressFinalize(this);
    }

    private static void InitializeEnvironmentVariables()
    {
        Environment.SetEnvironmentVariable("HTTP_PROXY", "");
        Environment.SetEnvironmentVariable("HTTPS_PROXY", "");
        Environment.SetEnvironmentVariable("AWS_ACCESS_KEY_ID", "test");
        Environment.SetEnvironmentVariable("AWS_SECRET_ACCESS_KEY", "test");
    }
}