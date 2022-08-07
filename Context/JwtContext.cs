using Microsoft.Extensions.Configuration;
using Tweet.Settings;

namespace Tweet.Context
{
    public class JwtContext
    {
        public string JWT_SECRET { get; set; }
        private readonly IConfiguration configuration;
        public JwtContext(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public JwtSettings GetJwtSettings()
        {
            var settings = configuration.GetSection(nameof(JwtSettings)).Get<JwtSettings>();
            return settings;
        }
    }
}