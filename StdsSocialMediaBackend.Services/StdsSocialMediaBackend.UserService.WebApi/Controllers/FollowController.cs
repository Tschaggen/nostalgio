using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StdsSocialMediaBackend.Domain.Helper;
using StdsSocialMediaBackend.Infrastructure.Persistence;

namespace StdsSocialMediaBackend.UserService.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FollowController : ControllerBase
    {
        private readonly UserDbContext _userDbContext;
        public FollowController(UserDbContext userDbContext)
        {
            _userDbContext = userDbContext;
        }

        [HttpGet("[action]")]
        //[ServiceFilter(typeof(ClientIpCheckActionFilter))]
        public async Task<ActionResult<List<Guid>>> GetFollowingIds ([FromBody]Guid userId)
        {
            Console.WriteLine(userId);
            try
            {
                return Ok(await _userDbContext.Follows
                            .Where(x => x.FollowerId == userId)
                            .Select(x => x.FollowingId)
                            .ToListAsync());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
            
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<bool>> IsFollower (Guid userId, Guid follwingId)
        {
            return Ok();
        }
    }
}
