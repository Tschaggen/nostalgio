using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StdsSocialMediaBackend.Domain.Helper;
using StdsSocialMediaBackend.Domain.Model.Auth;
using StdsSocialMediaBackend.Domain.Model.User;
using StdsSocialMediaBackend.Domain.Requests.Auth;
using StdsSocialMediaBackend.Infrastructure.Jwt;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;
using StdsSocialMediaBackend.Infrastructure.Persistence;

namespace StdsSocialMediaBackend.AuthService.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly JwtTokenHandler _jwtTokenHandler;
        private readonly AuthDbContext _authDbContext;
        private readonly ILogger<AuthController> _logger;
        private readonly HttpClient _httpClient;

        public AuthController(
            JwtTokenHandler jwtTokenHandler,
            AuthDbContext authDbContext,
            ILogger<AuthController> logger,
            HttpClient httpClient)
        {
            _jwtTokenHandler = jwtTokenHandler;
            _authDbContext = authDbContext;
            _logger = logger;
            _httpClient = httpClient;

        }

        [HttpPost("Login")]
        public async Task<ActionResult<AuthRes?>> Authenticate(AuthReq authReq)
        {
            if (string.IsNullOrWhiteSpace(authReq.Username) || string.IsNullOrWhiteSpace(authReq.Password))
            {
                _logger.LogWarning($"Login failed at {DateTime.Now}: name or paswword empty");
                return Unauthorized(null);
            }

            var user = await _authDbContext.Users.FirstOrDefaultAsync(x => x.Username == authReq.Username);

            if (user == null)
            {
                _logger.LogWarning($"Login failed at {DateTime.Now}: User {authReq.Username} not found");
                return Unauthorized(null);
            }

            if (user.Password != PasswordHelper.HashPw(authReq.Password))
            {
                _logger.LogWarning($"Login failed at {DateTime.Now}: User {user.Username} entered a wrong password");
                return Unauthorized(null);
            }

            var authRes = _jwtTokenHandler.GenerateJwtToken(user);

            if (authRes == null) return Unauthorized();

            _logger.LogInformation($"{user.Username} successfully logged in at {DateTime.Now}");
            return Ok(authRes);
        }

        [HttpPost("Register")]
        public async Task<ActionResult> Register(RegisterReq req)
        {
            Guid userId= new();

            if (string.IsNullOrWhiteSpace(req.User.UserName) || string.IsNullOrWhiteSpace(req.Password))
            {
                _logger.LogWarning($"Registration failed at {DateTime.Now}: name or paswword empty");
                return BadRequest();
            }

            try
            {
                using StringContent userJson = new(
                       JsonSerializer.Serialize(req.User),
                        Encoding.UTF8,
                        "application/json"
                );
                var res = await _httpClient.PostAsync("http://localhost:5000/api/User", userJson);

                if(!res.IsSuccessStatusCode) 
                {
                    return StatusCode(500, res.ReasonPhrase);
                }

                var str = await res.Content.ReadAsStringAsync();
                userId = JsonSerializer.Deserialize<Guid>(str);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            //ToDo: Create User in User Service, bei Misserfolg kein Eintrag in Auth
            //Allow UserService.User.Add() nur von dieser IP (IP-Protection statt Login für interne Calls?!)

            try
            {
                _authDbContext.Users.Add(new AuthUser
                {
                    Id = userId,
                    Username = req.User.UserName,
                    Password = PasswordHelper.HashPw(req.Password),
                    Role = Role.User
                });

                await _authDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            _logger.LogInformation($"Registration successful at {DateTime.Now}: {req.User.UserName} is registered");
            return Ok();
        }

        [HttpGet("GetUsers")]
        [Authorize(Roles = "Administrator")]
        //[ServiceFilter(typeof(ClientIpCheckActionFilter))]
        public async Task<ActionResult<List<string>>?> GetUsers()
        {
            var users = await _authDbContext.Users.ToListAsync();
            if (users == null)
            {
                return NotFound();
            }
            _logger.LogInformation("UserList successfully submitted");
            return Ok(users);
        }
    }
}
