using CodeFlix.Catalog.Api.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllersAndDocumentation()
    .AddApplicationServices()
    .AddRepositories()
    .AddAllConnections();

var app = builder.Build();

app.UseDocumentation();
app.MapControllers();
app.UseHttpsRedirection();

await app.RunAsync();

public partial class ProgramMarker;