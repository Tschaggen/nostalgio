using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using StdsSocialMediaBackend.Domain.Helper;
using StdsSocialMediaBackend.Domain.Model.Media;
using StdsSocialMediaBackend.Domain.Model.User;
using StdsSocialMediaBackend.Domain.Requests.User;
using StdsSocialMediaBackend.Infrastructure.Persistence;
using System.Net.Mime;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace StdsSocialMediaBackend.UserService.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserDbContext _userDbContext;
        private readonly HttpClient _httpClient;
        public UserController(UserDbContext userDbContext, HttpClient httpClient)
        {
            _userDbContext = userDbContext;
            _httpClient = httpClient;
        }

        [HttpPost]
        [ServiceFilter(typeof(ClientIpCheckActionFilter))]
        public async Task<ActionResult<Guid>> AddUser(User user)
        {
            try
            {
                _userDbContext.Users.Add(user);
                await _userDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
            return Ok(user.Id);
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<List<User>>> GetAll()
        {
            try
            {
                return await _userDbContext.Users.Include(x => x.Adress).ToListAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<User>> Get([FromBody]Guid userId)
        {
            try
            {
                var user = await _userDbContext.Users.Where(x => x.Id == userId).Include(x => x.Adress).FirstOrDefaultAsync();

                if (user == null)
                {
                    return BadRequest("User not found");
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("[action]")]
        [Authorize]
        public async Task<ActionResult> GetProfile()
        {
            string? authHeader = Request.Headers["Authorization"];

            if (authHeader == null)
            {
                return BadRequest("Error inside Auth-Header");
            }

            string? userId = UserFromAuthHeader.GetUserId(authHeader);

            if (userId == null)
            {
                return BadRequest("Error inside Auth-Header or user not found");
            }

            var userTask = _userDbContext.Users
                .Where(x => x.Id == Guid.Parse(userId))
                .Include(x => x.Adress)
                .FirstOrDefaultAsync();

            string reqCont = JsonConvert.SerializeObject(userId);

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("http://localhost/api/Post/GetPostsByUser"),
                Content = new StringContent(reqCont, Encoding.UTF8, MediaTypeNames.Application.Json /* or "application/json" in older versions */),
            };

            var postRes = await _httpClient.SendAsync(request);
            var str = await postRes.Content.ReadAsStringAsync();
            //Console.WriteLine(str);
            List<Post>? posts = System.Text.Json.JsonSerializer.Deserialize<List<Post>>(str);
            var user = await userTask;

            if(user == null)
            {
                return NotFound("User nicht gefunden");
            }

            var res = new GetProfileRes
            {
                User = user,
                Posts = posts
            };

            return Ok();
        }

        [HttpGet("[action]")]
        [Authorize]
        public async Task<ActionResult<List<KeyValuePair<Guid, string>>>> GetUsersForFollow()
        {
            var users = await _userDbContext.Users.ToListAsync();
            if(users == null)
            {
                return NotFound("No Users in DB");
            }
            var res = new List<KeyValuePair<Guid, string>>();

            foreach ( var user in users )
            {
                res.Add(new KeyValuePair<Guid, string>(user.Id, user.UserName));
            }

            return Ok(res);
        }

        //ToDo Delete + Update Profile : nur wenn User.Id == jwt.UserId
    }
}
