using ApiAngularApp.Models.Domain;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ApiAngularApp.Models;

namespace ApiAngularApp.Respositories.Implementation
{
    public class tokenServices
    {


        public string GenerateToken(Userjwt users)
        {
            var handler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(AuthSettings.PrivateKey);
            var credentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = GenerateClaims(users),
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = credentials,
            };

            var token = handler.CreateToken(tokenDescriptor);
            return handler.WriteToken(token);
        }

        private static ClaimsIdentity GenerateClaims(Userjwt user)
        {
            var claims = new ClaimsIdentity();
            claims.AddClaim(new Claim(ClaimTypes.Name, user.Email));

            foreach (var role in user.Roles)
                claims.AddClaim(new Claim(ClaimTypes.Role, role));

            return claims;
        }
    }
   

   
}
