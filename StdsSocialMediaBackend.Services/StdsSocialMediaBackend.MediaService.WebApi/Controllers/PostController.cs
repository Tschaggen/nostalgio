using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StdsSocialMediaBackend.Domain.Model.Media;
using StdsSocialMediaBackend.Domain.Model.User;
using StdsSocialMediaBackend.Domain.Requests.Media;
using StdsSocialMediaBackend.Infrastructure.Persistence;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;

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
        //[Authorize]
        public async Task<ActionResult<List<GetPostRes>>> GetTimeline()
        {
            var tokenFromHeader = HttpContext.Request.Headers["Authorization"];
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.ReadJwtToken(tokenFromHeader);
            var claims = token.Claims;
            string? userId = token.Claims.FirstOrDefault(c => c.Type == "id")?.Value;

            if(userId == null)
            {
                return BadRequest("The given user from jwt not found");
            }
            
            var userRes = await _httpClient.GetAsync("");
            var str = await userRes.Content.ReadAsStringAsync();
            List<Guid>? following = JsonSerializer.Deserialize<List<Guid>>(str);

            if(following == null)
            {
                return BadRequest($"User {userId} follows nobody or an error occured");
            }

            var res = new List<GetPostRes>();
            var postsDb = await _postDbContext.Posts.Where(x => following.Contains(x.UserId)).OrderBy(x => x.PostedAt).Take(10).ToListAsync();

            Byte[] b = System.IO.File.ReadAllBytes($"C:\\StdsTest\\Test.jpg");

            foreach (var postDb in postsDb)
            {
                //Byte[] b = System.IO.File.ReadAllBytes($"C:\\StdsTest\\Test.jpg");
                var img = File(b, "image/jpg", "post.jpg");
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
            foreach (var i in Enumerable.Range(0, 9))
            {
                res.Add(new GetPostRes
                {
                    PostId = Guid.NewGuid(),
                    PostetAt = DateTime.Now,
                    PostetByUserId = Guid.NewGuid(),
                    PostetByUsername = "Test",
                    Description = "Test",
                    Likes = 1,
                    Comments = new(),
                    Image = File(b, "image/jpg", "post.jpg")
                });
            }
            
            return Ok(res);
        }

        [HttpGet("[action]")]
        public ActionResult<GetPostRes> GetPost([FromBody] Guid postId)
        {
            //prüfe, dass User auch follower über jwt
            return Ok();
        }
        [HttpPost("[action]")]
        public ActionResult<GetPostRes> AddPost([FromBody] AddPostReq post)
        {
            //user-id aus jwt beziehen nicht aus body
            return Ok();
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> LikePost([FromBody] LikeReq req)
        {
            try
            {
                _postDbContext.Likes.Add(new Like
                {
                    PostId = req.PostId,
                    UserId = req.UserId
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
        public async Task<ActionResult> CommentPost([FromBody] CommentReq req)
        {
            try
            { 
                _postDbContext.Comments.Add(new Comment
                {
                    PostId = req.PostId,
                    UserId = req.UserId,
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
    }
}
