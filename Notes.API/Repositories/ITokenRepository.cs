using Notes.API.DTOs;
using Notes.API.Models;

namespace Notes.API.Repositories
{
    public interface ITokenRepository
    {
        string? GenerateToken(User user);

        Task<User?> Authenticate(UserLoginDto userLogin);
    }
}
