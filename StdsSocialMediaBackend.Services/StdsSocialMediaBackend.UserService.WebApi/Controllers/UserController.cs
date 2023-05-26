using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StdsSocialMediaBackend.Domain.Helper;
using StdsSocialMediaBackend.Domain.Model.User;
using StdsSocialMediaBackend.Infrastructure.Persistence;

namespace StdsSocialMediaBackend.UserService.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserDbContext _userDbContext;
        public UserController(UserDbContext userDbContext)
        {
            _userDbContext = userDbContext;
        }

        [HttpPost]
        [ServiceFilter(typeof(ClientIpCheckActionFilter))]
        public async Task<ActionResult> RegistereUser(User user)
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
            return Ok();
        }

        [HttpGet("[action]")]
        //[Authorize(Roles = "Administrator")]
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
    }
}
