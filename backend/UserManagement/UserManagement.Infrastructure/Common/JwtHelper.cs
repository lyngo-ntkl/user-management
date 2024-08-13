using System.Security.Claims;
using UserManagement.Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

namespace UserManagement.Infrastructure.Common
{
    public class JwtHelper
    {
        public static string GenerateAccessToken(User user, string securityKey)
        {
            Claim[] claims =
            {
                new Claim(ClaimTypes.Sid, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims);

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                SigningCredentials = credentials,
                IssuedAt = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddDays(7)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);

            string accessToken = tokenHandler.WriteToken(token);
            return accessToken;
        }

        public static IEnumerable<Claim> GetClaims(string jwt)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.ReadJwtToken(jwt);
            return token.Claims;
        }
    }
}
