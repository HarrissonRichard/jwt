using Tweet.Dtos;
using Tweet.Models;

namespace Tweet
{
    //for extension methods the class must be public
    public static class Extensions
    {
        //is gonna operate in the current item, the current User can have a method called AsDto that return its UserDto version
        public static UserDto AsDto(this UserModel user)
        {

          return  new UserDto {

                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Password = user.Password,
                Role = user.Role,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt
            };
          
        }
    }
}