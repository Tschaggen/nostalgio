using Microsoft.IdentityModel.Tokens;
using StdsSocialMediaBackend.Domain.Model.Auth;
using StdsSocialMediaBackend.Domain.Requests.Auth;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace StdsSocialMediaBackend.Infrastructure.Jwt
{
    public class JwtTokenHandler
    {
        //ToDo: über options...
        public const string JWT_SECURITY_KEY = "ChuckStinktChuckStinktChuckStinktChuckStinkt";
        private const int JWT_TOKEN_VALIDIY_MINS = 60;
        private readonly List<AuthUser> _testUser;

        public JwtTokenHandler()
        {
            _testUser = new List<AuthUser>
            {
                new AuthUser { Id = Guid.NewGuid(), Username = "Test", Password = "123", Role = Role.User }
            };
        }

        public AuthRes? GenerateJwtToken(AuthUser user)
        {

            var expires = DateTime.Now.AddMinutes(JWT_TOKEN_VALIDIY_MINS);
            var key = Encoding.ASCII.GetBytes(JWT_SECURITY_KEY);
            var claimsIdentity = new ClaimsIdentity(new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Name, user.Username),
                new Claim("id", user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            });

            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature
            );

            var securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                Expires = expires,
                SigningCredentials = signingCredentials
            };

            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);
            var token = jwtSecurityTokenHandler.WriteToken(jwtSecurityToken);

            return new AuthRes
            {
                ExpiresIn = (int)expires.Subtract(DateTime.Now).TotalSeconds,
                JwtToken = token
            };
        }
    }
}
