using Tweet.Models;

namespace Catalog.Dtos
{
    public record SignUpDto
    {
        public string Name { get; init; }
        public string Email { get; init; }
        public string Password { get; init; }

        public string Role { get; init; }
    }

}