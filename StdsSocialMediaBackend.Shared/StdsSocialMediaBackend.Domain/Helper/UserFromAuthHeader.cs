using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StdsSocialMediaBackend.Domain.Helper
{
    public static class UserFromAuthHeader
    {
        static public string? GetUserId(string authHeader)
        {
            var token = GetToken(authHeader);
            return token.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
        }

        static public string? GetUserName(string authHeader)
        {
            var token = GetToken(authHeader);
            return token.Claims.FirstOrDefault(c => c.Type == "name")?.Value;
        }

        private static JwtSecurityToken GetToken(string authHeader)
        {
            if (string.IsNullOrEmpty(authHeader) && !authHeader.StartsWith("Bearer "))
            {
                throw new Exception();
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.ReadJwtToken(authHeader.Substring("Bearer ".Length));

            if (token == null)
            {
                throw new ArgumentNullException(nameof(token));
            }

            return token;
        }
    }
}
