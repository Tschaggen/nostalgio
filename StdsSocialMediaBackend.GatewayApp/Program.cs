using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Values;
using StdsSocialMediaBackend.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("ocelot.json", false, true)
    .AddEnvironmentVariables();
builder.Services.AddOcelot();
builder.Services.AddCors();
builder.Services.RegisterInfrastructureServices(builder.Configuration);

var app = builder.Build();

await app.UseOcelot();

app.UseCors(
    options => options.AllowAnyOrigin.AllowAnyMethod().AllowAnyHeader().AllowAnyMethod()
    );


app.UseAuthentication();
app.UseAuthorization();

app.Run();
