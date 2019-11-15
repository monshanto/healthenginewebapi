using HealthEngineAPI.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Configuration;
using System.Security.Cryptography;

namespace HealthEngineAPI.Globals
{
    static public class GlobalMethods
    {
        public static string GetUserID(this ClaimsPrincipal principal)
        {
            var identity = principal.Identity as ClaimsIdentity;
            if (!identity.IsAuthenticated)
            {
                return null;
            }
            Claim identityClaim = identity.Claims.FirstOrDefault(x => x.Type == "UserId");
            return identityClaim.Value.ToString();
        }

        public static string GenerateAccessToken(ApplicationUsers appUser, string role, ApplicationSettings _settings)
        {
            IdentityOptions _options = new IdentityOptions();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("UserId",appUser.Id.ToString()),
                    new Claim("Email",appUser.Email),
                    new Claim(_options.ClaimsIdentity.RoleClaimType,role)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.JWT_SecretKey)), SecurityAlgorithms.HmacSha256Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(securityToken);
        }

        public static string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using(var range = RandomNumberGenerator.Create())
            {
                range.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
        public static int GenerateOTP()
        {
            return new Random().Next(1000, 9999);
        }
    }
}
