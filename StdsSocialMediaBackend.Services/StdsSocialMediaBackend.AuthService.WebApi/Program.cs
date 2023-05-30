using StdsSocialMediaBackend.AuthService.WebApi;
using StdsSocialMediaBackend.Domain.Helper;
using StdsSocialMediaBackend.Domain.Model.Auth;
using StdsSocialMediaBackend.Infrastructure;
using StdsSocialMediaBackend.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.RegisterInfrastructureServices(builder.Configuration);
builder.Services.RegisterAuthApiServices(builder.Configuration);

var app = builder.Build();

AddUserData(app);

//app.UseCors(
//    options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().AllowAnyMethod()
//);

app.UseCors("CorsPolicy");

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

static void AddUserData(WebApplication app)
{
    var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetService<AuthDbContext>();

    var user1 = new AuthUser
    {
        Id = Guid.Parse("f7004b60-fb3a-11ed-929d-7d2144380188"),
        Username = "TestUser",
        Password = PasswordHelper.HashPw("123"),
        Role = Role.User
    };
    var user2 = new AuthUser
    {
        Id = Guid.Parse("f7007270-fb3a-11ed-929d-7d2144380188"),
        Username = "Admin",
        Password = PasswordHelper.HashPw("345"),
        Role = Role.Administrator
    };
    var user3 = new AuthUser
    {
        Id = Guid.Parse("f7007271-fb3a-11ed-929d-7d2144380188"),
        Username = "Ron",
        Password = PasswordHelper.HashPw("hallo"),
        Role = Role.Influencer
    };

    db.Users.Add(user1);
    db.Users.Add(user2);
    db.Users.Add(user3);

    db.SaveChanges();
}