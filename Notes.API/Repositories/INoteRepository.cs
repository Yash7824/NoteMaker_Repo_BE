using Notes.API.DTOs;
using Notes.API.Models;

namespace Notes.API.Repositories
{
    public interface INoteRepository
    {
        Task<List<UserNoteDto>> GetNotesAsync(string dbConn);
        Task<UserNoteDto> GetNoteAsync(string dbConn, Guid id);
        Task<NoteAddedDto> AddNoteAsync(string dbConn, Note note);
        Task<NoteUpdatedDto> UpdateNoteAsync(string dbConn, Note note);
        Task<NoteDeletedDto> DeleteNoteAsync(string dbConn, Guid id);


        Task<List<UserNoteDto>> GetUserNotesAsync(string dbConn, Guid? id);
        Task<UserNoteDto> GetUserNoteAsync(string dbConn, Guid? userId, Guid noteId);
        Task<NoteAddedDto> AddUserNoteAsync(string dbConn, Note note, Guid? userId);
        Task<NoteUpdatedDto> UpdateUserNoteAsync(string dbConn, Note note, Guid? userId);
        Task<NoteDeletedDto> DeleteUserNoteAsync(string dbConn, Guid noteId, Guid? userId);
    }
}
