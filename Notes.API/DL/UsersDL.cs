using Notes.API.DTOs;
using Notes.API.Models;
using Notes.API.Repositories;
using Npgsql;
using System.Data;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.AspNetCore.Identity;

namespace Notes.API.DL
{
    public class UsersDL : IUserRepository
    {
        private IConfiguration configuration;
        private string? connectionString;
        public UsersDL(IConfiguration configuration)
        {
            this.configuration = configuration;
            connectionString = configuration["AppSettings:ConnectionStrings:PostgresDBConn"];
        }
        public async Task<List<User>> GetUsersAsync(string dbConn)
        {
            List<User> users = new List<User>();

            NpgsqlConnection conn = new NpgsqlConnection(dbConn);
            await conn.OpenAsync();
            string query = "Select * from \"Users\";";
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
            using(NpgsqlDataReader reader = await cmd.ExecuteReaderAsync())
            {
                while(await  reader.ReadAsync())
                {
                    User user = new User
                    {
                        User_Id = (Guid)reader["User_Id"],
                        Username = reader["Username"].ToString(),
                        Email = reader["Email"].ToString(),
                        Password = reader["Password"].ToString(),
                        Role = reader["Role"].ToString()
                    };

                    users.Add(user);
                }
            }
            return users;
        }

        public async Task<User> GetUserAsync(string dbConn, Guid id)
        {
            User user = new User();

            NpgsqlConnection conn = new NpgsqlConnection(dbConn);
            await conn.OpenAsync();
            string query = $"Select * from \"Users\" where \"User_Id\" = '{id}';";
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            using(NpgsqlDataReader reader = await cmd.ExecuteReaderAsync())
            {
                while(await reader.ReadAsync())
                {
                    user.User_Id = (Guid)reader["User_Id"];
                    user.Username = reader["Username"].ToString();
                    user.Email = reader["Email"].ToString();
                    user.Password = reader["Password"].ToString();
                    user.Role = reader["Role"].ToString();
                }
            }
            return user;
        }
        public async Task<UserAddedDto> AddUserAsync(string dbConn, User user)
        {
            int rowsAdded;
            NpgsqlConnection conn = new NpgsqlConnection(dbConn);
            await conn.OpenAsync();
            string query = $"Insert into \"Users\" values ('{user.User_Id}', '{user.Username}', '{user.Email}', '{user.Password}', '{user.Role}');";
            using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
            {
                rowsAdded = await cmd.ExecuteNonQueryAsync(); 
            };

            UserAddedDto userResponse = new UserAddedDto
            {
                IsAdded = true,
                RowsAdded = rowsAdded
            };

            return userResponse;
        }

        public async Task<UserUpdatedDto> UpdateUserAsync(string dbConn, User user)
        {
            int rowsUpdated;
            NpgsqlConnection conn = new NpgsqlConnection(dbConn);
            await conn.OpenAsync();
            string query = $"Update \"Users\" Set \"Username\" = '{user.Username}', \"Email\" = '{user.Email}', \"Password\" = '{user.Password}', \"Role\" = '{user.Role}' where \"User_Id\" = '{user.User_Id}';";
            using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
            {
                rowsUpdated = await cmd.ExecuteNonQueryAsync();
            };

            UserUpdatedDto userResponse = new UserUpdatedDto
            {
                IsUpdated = true,
                RowsUpdated = rowsUpdated
            };
            return userResponse;
        }

        public async Task<UserDeletedDto> DeleteUserAsync(string dbConn, Guid id)
        {
            int rowsDeleted;
            NpgsqlConnection conn = new NpgsqlConnection(dbConn);
            await conn.OpenAsync();
            string query = $"Delete from \"Users\" where \"User_Id\" = '{id}';";
            using(NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
            {
                rowsDeleted = await cmd.ExecuteNonQueryAsync();
            }

            UserDeletedDto userResponse = new UserDeletedDto
            {
                IsDeleted = true,
                RowsDeleted = rowsDeleted
            };
            return userResponse;
        }

  
    }
}
