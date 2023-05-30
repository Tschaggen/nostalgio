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

            if (string.IsNullOrEmpty(authHeader) && !authHeader.StartsWith("Bearer "))
            {
                return null;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.ReadJwtToken(authHeader.Substring("Bearer ".Length));
            return token.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
        }
    }
}
