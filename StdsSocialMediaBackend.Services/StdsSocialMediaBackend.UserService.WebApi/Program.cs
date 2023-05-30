using StdsSocialMediaBackend.Domain.Helper;
using StdsSocialMediaBackend.Domain.Model.Auth;
using StdsSocialMediaBackend.Domain.Model.User;
using StdsSocialMediaBackend.Infrastructure;
using StdsSocialMediaBackend.Infrastructure.Persistence;
using StdsSocialMediaBackend.UserService.WebApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.RegisterInfrastructureServices(builder.Configuration);
builder.Services.RegisterUserApiServices(builder.Configuration);

var app = builder.Build();

AddUserData(app);

//app.UseCors(
//    //options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().AllowAnyMethod()
//    );

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
    var db = scope.ServiceProvider.GetService<UserDbContext>();

    var user1Id = Guid.Parse("f7004b60-fb3a-11ed-929d-7d2144380188");
    var adr1Id = Guid.NewGuid();
    var user1 = new User
    {
        Id = user1Id,
        UserName = "TestUser",
        Adress = new Adress
        {
            Id = Guid.NewGuid(),
            ZipCode = "01234",
            City = "Musterhausen",
            Street = "Musterstraﬂe",
            HouseNumber = "12",
            Country = "DE"
        },
        Email = "test@test.com"
    };
    var user2Id = Guid.Parse("f7007270-fb3a-11ed-929d-7d2144380188");
    var user2 = new User
    {
        Id = user2Id,
        UserName = "Admin",
        Adress = new Adress
        {
            Id = Guid.NewGuid(),
            ZipCode = "01234",
            City = "Musterhausen",
            Street = "Musterstraﬂe",
            HouseNumber = "12",
            Country = "DE"
        },
        Email = "admin@test.de"
    };
    var user3Id = Guid.Parse("f7007271-fb3a-11ed-929d-7d2144380188");
    var user3 = new User
    {
        Id = user3Id,
        UserName = "Ron",
        Adress = new Adress
        {
            Id = Guid.NewGuid(),
            ZipCode = "01234",
            City = "Musterhausen",
            Street = "Musterstraﬂe",
            HouseNumber = "1",
            Country = "DE"
        },
        Email = "ron@bielecki.de"
    };

    db.Users.Add(user1);
    db.Users.Add(user2);
    db.Users.Add(user3);

    var f12 = new Follow
    {
        Id = Guid.NewGuid(),
        FollowerId = Guid.Parse("f7004b60-fb3a-11ed-929d-7d2144380188"),
        FollowingId = Guid.Parse("f7007270-fb3a-11ed-929d-7d2144380188")
    };
    var f13 = new Follow
    {
        Id = Guid.NewGuid(),
        FollowerId = Guid.Parse("f7004b60-fb3a-11ed-929d-7d2144380188"),
        FollowingId = Guid.Parse("f7007271-fb3a-11ed-929d-7d2144380188")
    };
    var f31 = new Follow
    {
        Id = Guid.NewGuid(),
        FollowerId = Guid.Parse("f7007271-fb3a-11ed-929d-7d2144380188"),
        FollowingId = Guid.Parse("f7004b60-fb3a-11ed-929d-7d2144380188")
    };
    var f23 = new Follow
    {
        Id = Guid.NewGuid(),
        FollowerId = Guid.Parse("f7007270-fb3a-11ed-929d-7d2144380188"),
        FollowingId = Guid.Parse("f7007271-fb3a-11ed-929d-7d2144380188")
    };

    db.Follows.Add(f12);
    db.Follows.Add(f13);
    db.Follows.Add(f31);
    db.Follows.Add(f23);

    db.SaveChanges();
}
