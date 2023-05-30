using StdsSocialMediaBackend.MediaController.WebApi;
using StdsSocialMediaBackend.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.RegisterInfrastructureServices(builder.Configuration);
builder.Services.RegisterMediaApiServices(builder.Configuration);

var app = builder.Build();

app.UseCors(
    //options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().AllowAnyMethod()
    );

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}

app.Run();
