using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StdsSocialMediaBackend.Domain.Helper;
using StdsSocialMediaBackend.Domain.Model.User;
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

        //[HttpGet("[action]")]
        //public async Task<ActionResult<bool>> IsFollower ([FromBody]Guid userId, [FromBody]Guid follwingId)
        //{
        //    return Ok();
        //}

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Follow([FromBody]string followingUserName/*Guid followingId*/)
        {
            User? followingUser = await _userDbContext.Users.Where(x => x.UserName == followingUserName).FirstOrDefaultAsync();

            if(followingUser == null)
            {
                return NotFound("User not found");
            }

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

            if (await _userDbContext.Follows.Where(x => x.FollowerId == Guid.Parse(userId) && x.FollowingId == followingUser.Id).AnyAsync())
            {
                return BadRequest("Already following");
            }

            _userDbContext.Follows.Add(new Follow
            {
                FollowerId = Guid.Parse(userId),
                FollowingId = followingUser.Id
            });
            await _userDbContext.SaveChangesAsync();
            return Ok();
        }
    }
}
