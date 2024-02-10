using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Notes.API.Models;
using Notes.API.Repositories;

namespace Notes.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {

        private IConfiguration configuration;
        INoteRepository noteRepo;
        private string? connectionString;
        public AdminController(INoteRepository noteRepo, IConfiguration configuration)
        {
            this.configuration = configuration;
            this.noteRepo = noteRepo;
            connectionString = configuration["AppSettings:ConnectionStrings:PostgresDBConn"];
        }

        [HttpGet]
        [Route("Get-Notes")]
        public async Task<IActionResult> GetNotes()
        {
            try
            {
                if (connectionString != null)
                {
                    return Ok(await noteRepo.GetNotesAsync(connectionString));
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
        [Route("Get-Note/{id}")]
        public async Task<IActionResult> GetNote([FromRoute] Guid id)
        {
            try
            {
                if (connectionString != null)
                {
                    return Ok(await noteRepo.GetNoteAsync(connectionString, id));
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
        [Route("Add-Note")]
        public async Task<IActionResult> AddNote([FromBody] Note note)
        {
            try
            {
                if (connectionString != null)
                {
                    return Ok(await noteRepo.AddNoteAsync(connectionString, note));
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
        [Route("Update-Note")]
        public async Task<IActionResult> UpdateNote([FromBody] Note note)
        {
            try
            {
                if (connectionString != null)
                {
                    return Ok(await noteRepo.UpdateNoteAsync(connectionString, note));
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
        [Route("Delete-Note/{id}")]
        public async Task<IActionResult> DeleteNote([FromRoute] Guid id)
        {
            try
            {
                if (connectionString != null)
                {
                    return Ok(await noteRepo.DeleteNoteAsync(connectionString, id));
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
