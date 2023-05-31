using StdsSocialMediaBackend.MediaController.WebApi;
using StdsSocialMediaBackend.Infrastructure;
using StdsSocialMediaBackend.Infrastructure.Persistence;
using StdsSocialMediaBackend.Domain.Model.Media;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.RegisterInfrastructureServices(builder.Configuration);
builder.Services.RegisterMediaApiServices(builder.Configuration);

var app = builder.Build();

//app.UseCors(
//    //options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().AllowAnyMethod()
//    );

AddPostData(app);

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

static void AddPostData(WebApplication app)
{
    var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetService<PostDbContext>();

    var p1 = new Post
    {
        Id = Guid.NewGuid(),
        UserId = Guid.Parse("f7004b60-fb3a-11ed-929d-7d2144380188"),
        Description = "BlaBla",
        PostedAt = DateTime.Now,
        Username = "TestUser",
        Comments = new(),
        Likes = new()
    };
    p1.Comments.Add(new Comment
    {
        Id = Guid.NewGuid(),
        Text = "Hallo",
        Username = "Admin",
        UserId = Guid.Parse("f7007270-fb3a-11ed-929d-7d2144380188")
    });
    var p2 = new Post
    {
        Id = Guid.NewGuid(),
        UserId = Guid.Parse("f7007270-fb3a-11ed-929d-7d2144380188"),
        Description = "AdminPost",
        PostedAt = DateTime.Now,
        Username = "Admin",
        Comments = new(),
        Likes = new()
    };
    //p1.Likes.Add(new Like
    //{
    //    Id = Guid.NewGuid(),
    //    UserId = Guid.Parse("f7007270-fb3a-11ed-929d-7d2144380188")
    //});

    db.Posts.Add(p1);
    db.Posts.Add(p2);
    db.SaveChanges();
}