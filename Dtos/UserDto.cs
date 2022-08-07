using System;
using System.ComponentModel.DataAnnotations;
using Tweet.Models;

namespace Tweet.Dtos
{
    public record UserDto
    {
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public string Role { get; set; }
        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}