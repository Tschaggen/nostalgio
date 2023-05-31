using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using StdsSocialMediaBackend.Domain.Helper;
using StdsSocialMediaBackend.Domain.Model.Media;
using StdsSocialMediaBackend.Domain.Model.User;
using StdsSocialMediaBackend.Domain.Requests.Media;
using StdsSocialMediaBackend.Infrastructure.Persistence;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace StdsSocialMediaBackend.MediaController.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly PostDbContext _postDbContext;
        private readonly HttpClient _httpClient;

        public PostController(PostDbContext postDbContext, HttpClient httpClient)
        {
            _postDbContext = postDbContext;
            _httpClient = httpClient;
        }

        [HttpGet("[action]")]
        [Authorize]
        public async Task<ActionResult<List<GetPostRes>>> GetTimeline()
        {
            string? authHeader = Request.Headers["Authorization"];

            if (authHeader == null )
            {
                return BadRequest("Error inside Auth-Header");
            }

            string? userId = UserFromAuthHeader.GetUserId(authHeader);

            if (userId == null)
            {
                return BadRequest("Error inside Auth-Header or user not found");
            }
            //var tokenHandler = new JwtSecurityTokenHandler();
            //var token = tokenHandler.ReadJwtToken(authHeader.Substring("Bearer ".Length));
            //string? userId = token.Claims.FirstOrDefault(c => c.Type == "id")?.Value;

            string reqCont = JsonConvert.SerializeObject(userId);

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("http://localhost:5000/api/Follow/GetFollowingIds"),
                Content = new StringContent(reqCont, Encoding.UTF8, MediaTypeNames.Application.Json /* or "application/json" in older versions */),
            };

            //var userRes = await _httpClient.GetAsync("http://localhost:5000/api/User/GetFollowingIds");
            var userRes = await _httpClient.SendAsync(request);
            var str = await userRes.Content.ReadAsStringAsync();
            //Console.WriteLine(str);
            List<Guid>? following = System.Text.Json.JsonSerializer.Deserialize<List<Guid>>(str);

            if(following == null || following.Count == 0)
            {
                return new List<GetPostRes> ();
            }

            var res = new List<GetPostRes>();
            var postsDb = await _postDbContext
                                    .Posts
                                    .Where(x => following.Contains(x.UserId) || x.UserId == Guid.Parse(userId))
                                    .OrderBy(x => x.PostedAt)
                                    .Take(10)
                                    .Include(x => x.Likes)
                                    .Include(x => x.Comments)
                                    .ToListAsync();


            foreach (var postDb in postsDb)
            {
                //Byte[] b = System.IO.File.ReadAllBytes($"C:\\StdsTest\\Test.jpg");
                Byte[] b = System.IO.File.ReadAllBytes($"C:\\StdsTest\\{postDb.OriginalImage}.jpg");
                //var img = File(b, "image/jpg", "post.jpg");
                int likes = 0;
                if (postDb.Likes != null)
                {
                    likes = postDb.Likes.Count();
                }

                var postRes = new GetPostRes
                {
                    PostId = postDb.Id,
                    PostetAt = postDb.PostedAt,
                    PostetByUserId = postDb.UserId,
                    PostetByUsername = postDb.Username,
                    Description = postDb.Description,
                    Likes = likes,
                    Comments = postDb.Comments,
                    Image = File(b, "image/jpg", "post.jpg")
                };

                res.Add(postRes);
            }

            //Test
            //foreach (var i in Enumerable.Range(0, 9))
            //{
            //    res.Add(new GetPostRes
            //    {
            //        PostId = Guid.NewGuid(),
            //        PostetAt = DateTime.Now,
            //        PostetByUserId = Guid.NewGuid(),
            //        PostetByUsername = "Test",
            //        Description = "Test",
            //        Likes = 1,
            //        Comments = new(),
            //        Image = File(b, "image/jpg", "post.jpg")
            //    });
            //}
            
            return Ok(res);
        }

        //[HttpGet("{id}")]
        //[Authorize]
        //public ActionResult<GetPostRes> GetPost([FromBody] Guid postId)
        //{
        //    //prüfe, dass User auch follower über jwt
        //    return Ok();
        //}

        [HttpGet("[action]")]
        //[Authorize]
        //interne Anfrage
        public async Task<ActionResult<List<Post>>> GetPostsByUser([FromBody] Guid userId)
        {
            var posts = await _postDbContext.Posts
                .Where(x => x.UserId == userId)
                .FirstOrDefaultAsync();

            if(posts == null)
            {
                return NotFound();
            }
            //prüfe, dass User auch follower über jwt
            return Ok(posts);
        }

        //verifiziere dass Img auch ein Bild ist
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> AddPost(AddPostReq post)
        {
            try
            {
                string? authHeader = Request.Headers["Authorization"];

                if (authHeader == null)
                {
                    return BadRequest("Error inside Auth-Header");
                }

                string? userId = UserFromAuthHeader.GetUserId(authHeader);
                string? userName = UserFromAuthHeader.GetUserName(authHeader);

                if (userId == null || userName == null)
                {
                    return BadRequest("Error inside Auth-Header or user not found");
                }

                //ToDo: gerneriere GUID, speichere Bild damit + speichere in DB

                var imgGuid = Guid.NewGuid();

                if (post.Image != null) {
                    string base64Image = post.Image.Replace("data:image/jpeg;base64,", string.Empty);

                    // Convert the base64 string to bytes
                    byte[] imageBytes = Convert.FromBase64String(base64Image);

                    System.IO.File.WriteAllBytes($"C:\\StdsTest\\{imgGuid.ToString()}.jpg", imageBytes);
                }

                _postDbContext.Posts.Add(new Post
                {
                    UserId = Guid.Parse(userId),
                    Username = userName,
                    Description = post.Description,
                    OriginalImage = imgGuid
                });

                await _postDbContext.SaveChangesAsync();
                return Ok();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            //user-id aus jwt beziehen nicht aus body
        }

        [HttpPost("[action]")]
        [Authorize]
        public async Task<ActionResult> LikePost([FromBody] Guid postId)
        {
            // ToDO: prüfe if Follower...
            try
            {
                string? authHeader = Request.Headers["Authorization"];

                if (authHeader == null)
                {
                    return BadRequest("Error inside Auth-Header");
                }

                string? userId = UserFromAuthHeader.GetUserId(authHeader);
                string? userName = UserFromAuthHeader.GetUserName(authHeader);

                if (userId == null || userName == null)
                {
                    return BadRequest("Error inside Auth-Header or user not found");
                }

                if(await _postDbContext.Likes.Where(x => x.PostId == postId && x.UserId == Guid.Parse(userId)).AnyAsync())
                {
                    return BadRequest("Like Bereits vorhanden");
                }

                _postDbContext.Likes.Add(new Like
                {
                    PostId = postId,
                    UserId = Guid.Parse(userId)
                });
                await _postDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
            return Ok();
        }

        [HttpPost("[action]")]
        [Authorize]
        public async Task<ActionResult> CommentPost([FromBody] CommentReq req)
        {
            try
            {
                //_postDbContext.Comments.Add(new Comment
                //{
                //    PostId = req.PostId,
                //    UserId = req.UserId,
                //    Text = req.Text,
                //    Created = DateTime.Now
                //});
                string? authHeader = Request.Headers["Authorization"];

                if (authHeader == null)
                {
                    return BadRequest("Error inside Auth-Header");
                }

                string? userId = UserFromAuthHeader.GetUserId(authHeader);
                string? userName  = UserFromAuthHeader.GetUserName(authHeader);

                if (userId == null || userName == null)
                {
                    return BadRequest("Error inside Auth-Header or user not found");
                }

                var postTask = _postDbContext.Posts.Where(x => x.Id == req.PostId).FirstOrDefaultAsync();
                //var userTask = _httpClient.GetAsync($"http://localhost/api/users/{userId}");

                var post = await postTask;
                if (post == null)
                {
                    return BadRequest("Post not found");
                }
                if (post.Comments == null)
                {
                    post.Comments = new();
                }

                if(req.Text == null || req.Text.IsNullOrEmpty())
                {
                    return BadRequest("Kein Text übergeben");
                }

                //var userRes = await userTask;
                //var str = await userRes.Content.ReadAsStringAsync();
                ////Console.WriteLine(str);
                //User? user = System.Text.Json.JsonSerializer.Deserialize<User>(str);

                //if(user == null)
                //{
                //    return BadRequest("User not found");
                //}

                post.Comments.Add(new Comment
                {
                    UserId = Guid.Parse(userId),
                    Username = userName,
                    Text = req.Text,
                    Created = DateTime.Now
                });
                await _postDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok();
        }

        //ToDo Delete + Update Post : nur wenn Post.UserId == jwt.UserId
        // delete Like
        //delete Comment
    }
}
