using System;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Tokens;
using Tweet.Models;

namespace Tweet.Services
{
    public class TokenService
    {

        public static CookieAuthConfig GenerateClaimsPrincipal(UserModel user)
        {
            var claimsIdentity = new ClaimsIdentity(new Claim[]
                 {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.PrimarySid, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role),

                 }, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties

            {
                IssuedUtc = DateTimeOffset.UtcNow
            };
            var config = new CookieAuthConfig
            {
                Scheme = CookieAuthenticationDefaults.AuthenticationScheme,
                PrincipalClaim = new ClaimsPrincipal(claimsIdentity),
                AuthProperties = authProperties
            };

            return config;
        }

        public static string GenerateToken(UserModel user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes("suppersecret comming right from the dotnet secrets");
            var tokenDescriptor = new SecurityTokenDescriptor
            {

                Subject = new ClaimsIdentity(new Claim[]
                {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role),

                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var tkn = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(tkn);

        }

    }
}