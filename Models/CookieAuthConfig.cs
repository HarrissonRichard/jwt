using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace Tweet.Models
{
    public class CookieAuthConfig
    {

        public string Scheme { get; set; }
        public ClaimsPrincipal PrincipalClaim { get; set; }
        public AuthenticationProperties AuthProperties { get; set; }
    }
}