using Microsoft.IdentityModel.Tokens;
using Notes.API.DL;
using Notes.API.DTOs;
using Notes.API.Models;
using Notes.API.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Net.WebSockets;
using System.Security.Claims;
using System.Text;

namespace Notes.API.BL
{
    public class LoginAuthBL : ITokenRepository
    {
        private IConfiguration configuration;
        private string? connectionString;
        private IUserRepository userRepository;
        public LoginAuthBL(IConfiguration configuration, IUserRepository userRepository) 
        { 
            this.configuration = configuration;
            connectionString = configuration["AppSettings:ConnectionStrings:PostgresDBConn"];
            this.userRepository = userRepository;
        }
        public async Task<User?> Authenticate(UserLoginDto userLogin)
        {
            List<User> users = new List<User>();

            if(connectionString != null)
            {
                users = await userRepository.GetUsersAsync(connectionString);
                var currentUser = users.FirstOrDefault(x => x.Username == userLogin.Username && x.Password == userLogin.Password);

                if (currentUser != null)
                {
                    return currentUser;
                }
            }

            return null;

        }

        public string? GenerateToken(User user)
        {
            var key = configuration["AppSettings:Jwt:Key"];
            var issuer = configuration["AppSettings:Jwt:Issuer"];
            var audience = configuration["AppSettings:Jwt:Audience"];

            if(key != null && user.Username != null && user.Email != null)
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.User_Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Email, user.Email),
                };

                var token = new JwtSecurityToken(issuer,
                    audience,
                    claims,
                    expires: DateTime.Now.AddMinutes(15),
                    signingCredentials: credentials);

                return new JwtSecurityTokenHandler().WriteToken(token);

            }

            return null;
            
        }
    }
}
