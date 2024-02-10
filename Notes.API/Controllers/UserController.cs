using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Notes.API.Models;
using Notes.API.Repositories;
using System.Security.Claims;

namespace Notes.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IConfiguration configuration;
        IUserRepository userRepository;
        private string? connectionString;
        public UserController(IUserRepository userRepository, IConfiguration configuration)
        {
            this.configuration = configuration;
            this.userRepository = userRepository;
            connectionString = configuration["AppSettings:ConnectionStrings:PostgresDBConn"];
        }

        [HttpGet]
        [Route("Get-Users")]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                if(connectionString != null)
                {
                    return Ok(await userRepository.GetUsersAsync(connectionString));
                }
                else
                {
                    return BadRequest("Unable to fetch Connection String");
                }
            }catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet]
        [Authorize]
        [Route("Which-User")]
        public async Task<IActionResult> WhichUser()
        {
            var currentUser = await GetCurrentUserAsync();
            return Ok($"Hello {currentUser?.Username}");
        }

        public async Task<User?> GetCurrentUserAsync()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity != null)
            {
                var userClaims = identity.Claims;
                string? nameIdentifierValue = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                Guid Id = nameIdentifierValue != null ? Guid.Parse(nameIdentifierValue) : Guid.Empty;

                var user = connectionString != null ? await userRepository.GetUserAsync(connectionString, Id) : null;
                return user;
            }

            return null;
        }

    }
}
