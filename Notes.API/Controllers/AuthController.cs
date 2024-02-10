using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Notes.API.DTOs;
using Notes.API.Repositories;

namespace Notes.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IConfiguration configuration;
        ITokenRepository tokenRepository;
        public AuthController(IConfiguration configuration, ITokenRepository tokenRepository)
        {
            this.configuration = configuration;
            this.tokenRepository = tokenRepository;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Create-Token")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto userLogin)
        {
            var user = await tokenRepository.Authenticate(userLogin);

            if(user != null)
            {
                var token = tokenRepository.GenerateToken(user);
                return Ok(token);
            }

            return NotFound("User not found");
        }
    }
}
