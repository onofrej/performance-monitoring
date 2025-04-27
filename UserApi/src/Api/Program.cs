using User.Api.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.InitializeApplicationDependencies();

var app = builder.Build();

app.UseHttpsRedirection();
app.MapControllers();
app.UseAuthentication();
app.UseAuthorization();
app.UseApplicationDependencies();

app.Run();

[ExcludeFromCodeCoverage]
public partial class Program
{
    protected Program()
    { }
}