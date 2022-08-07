using Microsoft.Extensions.Configuration;

namespace Tweet.Settings
{
    public class JwtSettings
    {
        public string JWT_EXPIRES { get; set; }
        public string JWT_SECRET { get; set; }

        private readonly IConfiguration configuration;

        public JwtSettings()
        {

        }
        public JwtSettings(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public JwtSettings GetSettings()
        {
            var settings = configuration.GetSection(nameof(JwtSettings)).Get<JwtSettings>();
            return settings;
        }
    }

}