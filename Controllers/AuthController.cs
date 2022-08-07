using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Catalog.Dtos;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Tweet.Context;
using Tweet.Dtos;
using Tweet.Models;
using Tweet.Repository;
using Tweet.Services;
using Tweet.Settings;
using Tweet.Utils;

namespace Tweet.Controllers
{
    [ApiController]

    public class AuthControllers : ControllerBase
    {
        private readonly IUsersRepository repository;
        private readonly JwtSettings settings;
        private readonly IValidator<UserModel> validator;

        public AuthControllers(IUsersRepository repository, JwtSettings settings, IValidator<UserModel> validator)
        {
            this.repository = repository;
            this.settings = settings;
            this.validator = validator;

        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<UserDto>> LoginAsync(LoginDto userDto)
        {
            try
            {
                //validate input user info..check if it
                var userData = new UserModel
                {
                    Email = userDto.Email,
                    Password = userDto.Password
                };
                await validator.ValidateAsync(userData, options =>
                {
                    options.IncludeRuleSets("Login");
                    options.ThrowOnFailures();
                });


                //input data are valid till here.


                //now check if thisdude reallly existis in db
                var user = await repository.GetUserByEmailAsync(userData.Email);
                if (user is null)
                {
                    return BadRequest(new
                    {
                        status = "fail",
                        message = "Email ou senha invalidos, tente novamente"
                    });
                }

                //check passwords

                if (!PasswordManager.ComparePasswords(userData.Password, user.Password))
                {
                    return BadRequest(new
                    {
                        status = "fail",
                        message = "Email ou senha invalidos"
                    });
                }

                //match
                //return user and token
                var token = TokenService.GenerateToken(user);
                user.Password = null;


                return Ok(new
                {
                    user,
                    token
                });

            }
            catch (System.Exception ex)
            {

                return BadRequest(new { Status = "Fail", Message = ex.Message, Stack = ex.StackTrace });
            }

        }

        [HttpPost]
        [Route("signup")]
        public async Task<ActionResult<UserDto>> SignUpAsync(SignUpDto signUpDto)
        {

            try
            {

                var newUser = new UserModel
                {
                    Name = signUpDto.Name,
                    Email = signUpDto.Email,
                    Password = signUpDto.Password,
                    Role = signUpDto.Role
                };

                await validator.ValidateAsync(newUser, opt =>
                {
                    opt.ThrowOnFailures();

                });

                //till here objects are valid
                await repository.SignUpAsync(newUser);

                return new CreatedResult(nameof(SignUpAsync), newUser);


            }
            catch (ValidationException vex)
            {

                return BadRequest(new { ex = vex.Errors });//handle validations errors here
            }
            catch (SqlException ex)
            {
                //handle exceptions like user with this id found
                return BadRequest(new { ex = ex.Message });
            }
            catch (Exception gex)
            {
                return BadRequest(new { ex = gex.Data });
            }
            //validate user input data here

            //check if there is no user with this email on here...
            //check user by its email or caputre uniqueness exception and send it all
            //get the token 
            //hash password
            //put on db
            //create tojken
            //null password
            //send back user with token in there....you digg

        }
    }
}