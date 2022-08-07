using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Tweet.Models;

namespace Tweet.Services
{
    public class TokenService
    {

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

            // var claims = new List<Claim>
            // {
            //     new Claim(ClaimTypes.Name, user.Name),
            //     new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            //     new Claim(ClaimTypes.Role, user.Role),
            //     new Claim(JwtRegisteredClaimNames.Nbf, new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString()),
            //     new Claim(JwtRegisteredClaimNames.Exp, new DateTimeOffset(DateTime.Now.AddDays(1)).ToUnixTimeSeconds().ToString())
            // };

            // var token = new JwtSecurityToken(
            //     new JwtHeader(
            //         new SigningCredentials(new SymmetricSecurityKey(),
            //         SecurityAlgorithms.HmacSha256)),
            //         new JwtPayload(claims));


            // return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}