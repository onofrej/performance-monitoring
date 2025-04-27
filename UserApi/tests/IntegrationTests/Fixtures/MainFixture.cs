using User.Api.IntegrationTests.Factories;
using User.Api.IntegrationTests.Fixtures.PostgreSql;

namespace User.Api.IntegrationTests.Fixtures;

public sealed class MainFixture : IDisposable
{
    public MainFixture()
    {
        Configuration = new ConfigurationBuilder()
          .SetBasePath(AppContext.BaseDirectory)
          .AddJsonFile("integrationtests-settings.json", optional: false)
          .Build();

        PostgreSqlFixture = new PostgreSqlFixture(Configuration);
        HttpClient = new CustomWebApplicationFactory().CreateClient();
    }

    internal HttpClient HttpClient { get; }
    internal IConfiguration Configuration { get; }
    internal PostgreSqlFixture PostgreSqlFixture { get; }

    public void Dispose()
    {
        PostgreSqlFixture.Dispose();

        GC.SuppressFinalize(this);
    }
}