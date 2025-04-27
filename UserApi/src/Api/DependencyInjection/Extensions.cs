using User.Api.Features.User;

namespace User.Api.DependencyInjection;

[ExcludeFromCodeCoverage]
internal static class ServiceCollectionExtensions
{
    internal static IServiceCollection InitializeApplicationDependencies(this IServiceCollection services)
    {
        services.InitializeAwsServices()
            .InitializeDatabase()
            .InitializeLog()
            .InitializeMediatr()
            .InitializeSwagger();

        return services;
    }

    private static IServiceCollection InitializeAwsServices(this IServiceCollection services)
    {
        //services.AddAWSService<IAmazonSecretsManager>();
        //services.AddAWSService<IAmazonSQS>();

        return services;
    }

    private static IServiceCollection InitializeSwagger(this IServiceCollection services)
    {
        services.AddCarter();

        services.AddSwaggerGen(options =>
        {
            options.CustomSchemaIds(schemaIdSelector => schemaIdSelector.FullName);
        });

        return services;
    }

    private static IServiceCollection InitializeMediatr(this IServiceCollection services)
    {
        services.AddMediatR(configuration =>
            configuration.RegisterServicesFromAssembly(typeof(Program).Assembly));

        services.AddValidatorsFromAssembly(typeof(Program).Assembly);

        return services;
    }

    private static IServiceCollection InitializeLog(this IServiceCollection services)
    {
        services.AddLogging();

        return services;
    }

    private static IServiceCollection InitializeDatabase(this IServiceCollection services)
    {
        services.AddSingleton<IDataAccess, DataAccess>();

        services.AddSingleton(_ =>
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            return new NpgsqlDataSourceBuilder("").Build();
        });

        return services;
    }
}

[ExcludeFromCodeCoverage]
internal static class WebApplicationExtensions
{
    internal static IApplicationBuilder UseApplicationDependencies(this WebApplication application)
    {
        application.MapCarter();
        application.UseSwagger();
        application.UseSwaggerUI(setupAction =>
        {
            setupAction.SwaggerEndpoint("/swagger/v1/swagger.json", "User.API");
        });

        return application;
    }
}