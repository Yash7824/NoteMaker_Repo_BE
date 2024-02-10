using Notes.API.DTOs;
using Notes.API.Models;

namespace Notes.API.Repositories
{
    public interface IUserRepository
    {
        Task<List<User>> GetUsersAsync(string dbConn);
        Task<User> GetUserAsync(string dbConn, Guid id);
        Task<UserAddedDto> AddUserAsync(string dbConn, User user);
        Task<UserUpdatedDto> UpdateUserAsync(string dbConn, User user);
        Task<UserDeletedDto> DeleteUserAsync(string dbConn, Guid id);


    }
}
