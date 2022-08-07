using Dapper;
using System;
using System.Data;
using System.Collections.Generic;

using Tweet.Models;
using System.Data.SqlClient;
using Tweet.Context;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using Tweet.Dtos;
using Tweet.Utils;
using Catalog.Dtos;
using System.Security.Cryptography;
using System.Text;

namespace Tweet.Repository
{
    public class SqlServerUsersRepository : IUsersRepository


    {
        private readonly DapperContext context;
        IValidator<UserModel> _validator;

        public SqlServerUsersRepository(IValidator<UserModel> validator, DapperContext context)
        {
            _validator = validator;
            this.context = context;
        }


        public async Task CreateUserAsync(UserModel user)
        {
            //estabilish a connection
            //validate all the data first
            //check if the password is okay or not. if not okay getback a null or throw an exception
            //insert into a db and return created;

            await _validator.ValidateAndThrowAsync(user);


            using (var connection = context.CreateConnection())
            {


                var parameters = new
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Password = user.Password,
                    Role = user.Role
                };

                string sql = @"INSERT INTO [Users](Id, Name, Email, Password, Role)
                    VALUES(@Id, @Name, @Email, @Password, @Role);";
                var newUser = await connection.ExecuteAsync(sql, parameters);
            }
        }

        public async Task DeleteUserAsync(Guid id)
        {
            //check the log in - if logged or not
            //check the role if you are authorized or not
            //validate id
            //check if thata item existis
            //estabilish connection
            //excute the Command
            using (var connections = context.CreateConnection())
            {
                string sql = @"DELETE FROM Users WHERE Id = @id";
                var param = new { Id = id };
                await connections.ExecuteAsync(sql, param);
            }

        }

        public async Task<UserModel> GetUserAsync(Guid id)
        {

            using (var connection = context.CreateConnection())
            {
                //gota exposed
                var parameters = new { Id = id };
                string sql = @"SELECT * FROM [Users] WHERE Id=@id";
                UserModel user = await connection.QuerySingleAsync<UserModel>(sql, parameters);
                return user;
            }
        }

        public async Task<IEnumerable<UserModel>> GetUsersAsync(string connString)
        {
            using (var connection = new SqlConnection(connString))
            {
                string conn = GlobalConfig.GetConnString();
                //make the QueryHaapeens
                string sql = @"SELECT [Id],[Name],[Email],[Password],[CreatedAt],[Role]
                    FROM [Users];
                ";

                var users = await connection.QueryAsync<UserModel>(sql);
                return users;
            }

        }

        public async Task<UserModel> GetUserByEmailAsync(string email)
        {
            using (var connection = context.CreateConnection())
            {

                try
                {
                    string sql = @"SELECT * FROM Users WHERE email=@Email;";
                    var p = new
                    {
                        Email = email
                    };

                    var loggedUser = await connection.QuerySingleOrDefaultAsync<UserModel>(sql, p);

                    return loggedUser;
                }
                catch (System.Exception)
                {

                    throw;
                }

            }
        }

        public async Task<UserModel> LoginAsync(LoginDto user)
        {

            using (var connection = context.CreateConnection())
            {

                //finduser by mail - if valid

                return new UserModel { };

            }
        }

        public async Task SignUpAsync(UserModel user)
        {


            var hashedPassword = PasswordManager.HashPassword(user.Password);

            using (var conn = context.CreateConnection())
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        var sql = "spInsert_User";

                        var p = new DynamicParameters();
                        p.Add("Name", user.Name);
                        p.Add("Email", user.Email);
                        p.Add("Password", hashedPassword);
                        p.Add("CreatedAt", DateTime.UtcNow);
                        p.Add("Role", user.Role);


                        await conn.ExecuteAsync(sql, p, transaction, commandType: CommandType.StoredProcedure);




                        transaction.Commit();
                    }
                    catch (SqlException SqlEx)
                    {

                        transaction.Rollback();
                        throw SqlEx;
                    }

                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        var a = ex.Message;

                    }
                    finally
                    {
                        conn.Close();
                    }

                }
            }
        }

        public async Task UpdateUserAsync(UserModel model)
        {
            //validate at very begginig
            //estabilhs connection
            using (var connnection = context.CreateConnection())
            {
                var parameters = new
                {
                    Id = model.Id,
                    Name = model.Name,
                    Email = model.Email,
                    Password = model.Password,
                    Role = model.Role,
                    UpdatedAt = model.UpdatedAt
                };
                string sql = @"UPDATE Users SET NAME=@name,EMAIL=@email,PASSWORD=@password,ROLE=@role
                    where Id = @id;";

                await connnection.ExecuteAsync(sql, parameters);
            }



        }
    }
}