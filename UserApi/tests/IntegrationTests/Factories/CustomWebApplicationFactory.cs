using Microsoft.Extensions.Hosting;

namespace User.Api.IntegrationTests.Factories;

internal sealed class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.UseEnvironment("IntegrationTests");

        return base.CreateHost(builder);
    }
}