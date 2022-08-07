using Tweet.Models;

namespace Catalog.Dtos
{
    public record UpdateUserDto
    {
        public string Name { get; init; }
        public string Email { get; init; }
        public string Password { get; init; }

        public string Role { get; init; }
    }

}