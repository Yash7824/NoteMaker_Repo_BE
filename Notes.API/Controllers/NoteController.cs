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
    public class NoteController : ControllerBase
    {
        private IConfiguration configuration;
        INoteRepository noteRepo;
        IUserRepository userRepo;
        private string? connectionString;
        public NoteController(INoteRepository noteRepo, IConfiguration configuration, IUserRepository userRepo)
        {
            this.configuration = configuration;
            this.noteRepo = noteRepo;
            this.userRepo = userRepo;
            connectionString = configuration["AppSettings:ConnectionStrings:PostgresDBConn"];
        }

        public async Task<User?> GetCurrentUserAsync()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity != null)
            {
                var userClaims = identity.Claims;
                string? nameIdentifierValue = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                Guid Id = nameIdentifierValue != null ? Guid.Parse(nameIdentifierValue) : Guid.Empty;

                var user = connectionString != null ? await userRepo.GetUserAsync(connectionString, Id) : null;
                return user;
            }

            return null;
        }

        


        [HttpGet]
        [Route("Get-User-Notes")]
        [Authorize]
        public async Task<IActionResult> GetUserNotes()
        {
            try
            {
                if (connectionString != null)
                {
                    var currentUser = await GetCurrentUserAsync();
                    return Ok(await noteRepo.GetUserNotesAsync(connectionString, currentUser?.User_Id));
                }
                else
                {
                    return BadRequest("Unable to fetch the Connection String");
                }


            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        
        [HttpGet]
        [Route("Get-User-Note/{noteId}")]
        [Authorize]
        public async Task<IActionResult> GetUserNote([FromRoute] Guid noteId)
        {
            try
            {
                if (connectionString != null)
                {
                    var currentUser = await GetCurrentUserAsync();
                    return Ok(await noteRepo.GetUserNoteAsync(connectionString, currentUser?.User_Id, noteId));
                }
                else
                {
                    return BadRequest("Unable to fetch the Connection String");
                }

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("Add-User-Note")]
        [Authorize]
        public async Task<IActionResult> AddUserNote([FromBody] Note note)
        {
            try
            {
                if (connectionString != null)
                {
                    var currentUser = await GetCurrentUserAsync();
                    return Ok(await noteRepo.AddUserNoteAsync(connectionString, note, currentUser?.User_Id));
                }
                else
                {
                    return BadRequest("Unable to fetch Connection String");
                }

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPut]
        [Route("Update-User-Note")]
        [Authorize]
        public async Task<IActionResult> UpdateUserNote([FromBody] Note note)
        {
            try
            {
                if (connectionString != null)
                {
                    var currentUser = await GetCurrentUserAsync();
                    return Ok(await noteRepo.UpdateUserNoteAsync(connectionString, note, currentUser?.User_Id));
                }
                else
                {
                    return BadRequest("Unable to fetch the Connection String");
                }

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpDelete]
        [Route("Delete-User-Note/{noteId}")]
        [Authorize]
        public async Task<IActionResult> DeleteUserNote([FromRoute] Guid noteId)
        {
            try
            {
                if (connectionString != null)
                {
                    var currentUser = await GetCurrentUserAsync();
                    return Ok(await noteRepo.DeleteUserNoteAsync(connectionString, noteId, currentUser?.User_Id));
                }
                else
                {
                    return BadRequest("Unable to fetch the Connection String");
                }

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}
