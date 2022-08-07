using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Catalog.Dtos;
using Tweet.Dtos;
using Tweet.Models;

namespace Tweet.Repository
{
    public interface IUsersRepository
    {

        Task<IEnumerable<UserModel>> GetUsersAsync();
        Task<UserModel> GetUserAsync(Guid Id);
        Task CreateUserAsync(UserModel user);
        Task UpdateUserAsync(UserModel model);
        Task DeleteUserAsync(Guid id);
        Task SignUpAsync(UserModel user);
        Task<UserModel> GetUserByEmailAsync(string email);
    }
}