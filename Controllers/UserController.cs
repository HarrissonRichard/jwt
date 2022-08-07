using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Tweet.Models;
using Tweet.Repository;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Linq;
using Tweet.Dtos;
using Catalog.Dtos;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;

namespace Tweet.Controllers
{

    [ApiController]
    [Route("users")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUsersRepository repository;

        public UserController(IUsersRepository repository)
        {
            //this.configuration = iConfig;
            this.repository = repository;
        }


        [HttpGet]
        public async Task<ActionResult<UserDto>> GetUsersAsync()
        {
            string conn = GlobalConfig.GetConnString();

            //get the connection string
            //estabilish connection with the db firts
            //make the query and retriev it all
            //get all users from my DB and return it
            var users = (await repository.GetUsersAsync(conn)).Select(user => user.AsDto());

            return Ok(users);

        }
        //ActionResult - allow us to return more than one type like not found or the actual item..
        [HttpGet("{id}")]
        [Authorize(Roles = "developer")]
        public async Task<ActionResult<UserDto>> GetUserAsync(Guid id)
        {
            var user = await repository.GetUserAsync(id);
            if (user is null)
            {
                return NotFound();
            }
            return user.AsDto();
        }

        [HttpPost]
        public async Task<ActionResult<UserDto>> CreateUserAsync(CreateUserDto userDto)
        {

            try
            {

                UserModel newUser = new()
                {
                    Id = Guid.NewGuid(),
                    Name = userDto.Name,
                    Email = userDto.Email,
                    Password = userDto.Password,
                    Role = userDto.Role
                };

                await repository.CreateUserAsync(newUser);
                // the createUser ,method reflects the eaction of creating it
                return CreatedAtAction(nameof(CreateUserAsync), new { id = newUser.Id }, newUser.AsDto());
            }
            catch (ValidationException ex)
            {
                List<string> messages = new List<string>();
                foreach (ValidationFailure failure in ex.Errors)
                {
                    messages.Add(failure.ErrorMessage);
                }

                return BadRequest(new
                {
                    status = "Fail",
                    message = messages,
                    stackTrace = ex.StackTrace
                });
            }

        }
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdatedItemAsync(UpdateUserDto userDto)
        {

            //UserModel updatedUser = userExisted with {Name = newUser.Name ....}
            //with-expression-SAYS
            // I AM TAKING A COPY OF THISEXISTING VALUE WITH THIS THE FOLLOWING TYPE MODFIED VALUES MODIFIED


            //repository.UpdateUser(model);  
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUserAsync(Guid id)
        {   //find the user if existis delete
            //if not badRequest Thenor NotFound  

            await repository.DeleteUserAsync(id);
            return NoContent();
        }
    }
}