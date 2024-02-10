using Notes.API.DTOs;
using Notes.API.Models;
using Notes.API.Repositories;
using Npgsql;

namespace Notes.API.DL
{
    public class NotesDL : INoteRepository
    {

        public async Task<List<UserNoteDto>> GetNotesAsync(string dbConn)
        {
            List<UserNoteDto> notes = new List<UserNoteDto>();

            NpgsqlConnection conn = new NpgsqlConnection(dbConn);
            await conn.OpenAsync();
            string query = "Select * from \"Notes\" notes inner join \"Users\" users on notes.\"User_Id\" = users.\"User_Id\"";
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    UserNoteDto note = new UserNoteDto
                    {
                        Note_Id = (Guid)reader["Note_Id"],
                        Title = reader["Title"].ToString(),
                        Description = reader["Description"].ToString(),
                        Tag = reader["Tag"].ToString(),
                        AddedOn = Convert.ToDateTime(reader["AddedOn"]),
                        User_Id = (Guid)reader["User_Id"],
                        Username = reader["Username"].ToString()
                    };

                    notes.Add(note);
                }
            }

            return notes;
        }

        public async Task<UserNoteDto> GetNoteAsync(string dbConn, Guid id)
        {

            UserNoteDto note = new UserNoteDto();
            NpgsqlConnection conn = new NpgsqlConnection(dbConn);
            await conn.OpenAsync();
            string query = $"Select * from \"Notes\" notes inner join \"Users\" users on notes.\"User_Id\" = users.\"User_Id\" where notes.\"Note_Id\" = '{id}';";
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
            NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                note.Note_Id = (Guid)reader["Note_Id"];
                note.Title = reader["Title"].ToString();
                note.Description = reader["Description"].ToString();
                note.Tag = reader["Tag"].ToString();
                note.AddedOn = Convert.ToDateTime(reader["AddedOn"]);
                note.User_Id = (Guid)reader["User_Id"];
                note.Username = reader["Username"].ToString();
            }

            return note;
        }

        public async Task<NoteAddedDto> AddNoteAsync(string dbConn, Note note)
        {
            int rowsAdded;
            NpgsqlConnection conn = new NpgsqlConnection(dbConn);
            await conn.OpenAsync();
            string query = $"Insert into \"Notes\" values('{note.Note_Id}', '{note.Title}', '{note.Description}', '{note.Tag}', '{note.AddedOn}');";
            using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
            {
                rowsAdded = await cmd.ExecuteNonQueryAsync();
            };

            NoteAddedDto responseNote = new NoteAddedDto
            {
                IsAdded = true,
                RowsAdded = rowsAdded
            };

            return responseNote;
        }

        public async Task<NoteUpdatedDto> UpdateNoteAsync(string dbConn, Note note)
        {
            int rowsUpdated;
            NpgsqlConnection conn = new NpgsqlConnection(dbConn);
            await conn.OpenAsync();
            string query = $"Update \"Notes\" Set \"Title\" = '{note.Title}', \"Description\" = '{note.Description}', \"Tag\" = '{note.Tag}', \"AddedOn\" = '{note.AddedOn}' where \"Note_Id\" = '{note.Note_Id}';";
            using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
            {
                rowsUpdated = await cmd.ExecuteNonQueryAsync();
            };

            NoteUpdatedDto responseNote = new NoteUpdatedDto
            {
                IsUpdated = true,
                RowsUpdated = rowsUpdated
            };

            return responseNote;
        }

        public async Task<NoteDeletedDto> DeleteNoteAsync(string dbConn, Guid id)
        {
            int rowsDeleted;
            NpgsqlConnection conn = new NpgsqlConnection(dbConn);
            await conn.OpenAsync();
            string query = $"Delete from \"Notes\" where \"Note_Id\" = '{id}'";
            using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
            {
                rowsDeleted = await cmd.ExecuteNonQueryAsync();
            }

            NoteDeletedDto responseNote = new NoteDeletedDto
            {
                IsDeleted = true,
                RowsDeleted = rowsDeleted
            };

            return responseNote;
        }

        public async Task<List<UserNoteDto>> GetUserNotesAsync(string dbConn, Guid? id)
        {
            List<UserNoteDto> notes = new List<UserNoteDto>();
            NpgsqlConnection connection = new NpgsqlConnection(dbConn);
            await connection.OpenAsync();
            string query = $"Select * from \"Notes\" notes inner join \"Users\" users on notes.\"User_Id\" = users.\"User_Id\" where notes.\"User_Id\" = '{id}';";
            NpgsqlCommand cmd = new NpgsqlCommand(query, connection);
            using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    UserNoteDto note = new UserNoteDto
                    {
                        Note_Id = (Guid)reader["Note_Id"],
                        Title = reader["Title"].ToString(),
                        Description = reader["Description"].ToString(),
                        Tag = reader["Tag"].ToString(),
                        AddedOn = Convert.ToDateTime(reader["AddedOn"]),
                        User_Id = (Guid)reader["User_Id"],
                        Username = reader["Username"].ToString()
                    };

                    notes.Add(note);
                }
            }

            return notes;
        }

        public async Task<UserNoteDto> GetUserNoteAsync(string dbConn, Guid? userId, Guid noteId)
        {
            UserNoteDto note = new UserNoteDto();
            NpgsqlConnection connection = new NpgsqlConnection(dbConn);
            await connection.OpenAsync();
            string query = $"Select * from \"Notes\" notes inner join \"Users\" users on notes.\"User_Id\" = users.\"User_Id\" where notes.\"User_Id\" = '{userId}' and notes.\"Note_Id\" = '{noteId}';";
            NpgsqlCommand cmd = new NpgsqlCommand(query, connection);
            using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    note.Note_Id = (Guid)reader["Note_Id"];
                    note.Title = reader["Title"].ToString();
                    note.Description = reader["Description"].ToString();
                    note.Tag = reader["Tag"].ToString();
                    note.AddedOn = Convert.ToDateTime(reader["AddedOn"]);
                    note.User_Id = (Guid)reader["User_Id"];
                    note.Username = reader["Username"].ToString();
                }
            }

            return note;
        }

        public async Task<NoteAddedDto> AddUserNoteAsync(string dbConn, Note note, Guid? userId)
        {
            int rowsAdded;
            NpgsqlConnection conn = new NpgsqlConnection(dbConn);
            await conn.OpenAsync();
            string query = $"Insert into \"Notes\" values('{note.Note_Id}', '{note.Title}', '{note.Description}', '{note.Tag}', '{note.AddedOn}', '{userId}');";
            using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
            {
                rowsAdded = await cmd.ExecuteNonQueryAsync();
            };

            NoteAddedDto responseNote = new NoteAddedDto
            {
                IsAdded = true,
                RowsAdded = rowsAdded
            };

            return responseNote;
        }

        public async Task<NoteUpdatedDto> UpdateUserNoteAsync(string dbConn, Note note, Guid? userId)
        {
            int rowsUpdated;
            NpgsqlConnection conn = new NpgsqlConnection(dbConn);
            await conn.OpenAsync();
            string query = $"Update \"Notes\" Set \"Title\" = '{note.Title}', \"Description\" = '{note.Description}', \"Tag\" = '{note.Tag}', \"AddedOn\" = '{note.AddedOn}' where \"Note_Id\" = '{note.Note_Id}' and \"User_Id\" = '{userId}';";

            using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
            {
                rowsUpdated = await cmd.ExecuteNonQueryAsync();
            };

            NoteUpdatedDto responseNote = new NoteUpdatedDto
            {
                IsUpdated = true,
                RowsUpdated = rowsUpdated
            };

            return responseNote;
        }

        public async Task<NoteDeletedDto> DeleteUserNoteAsync(string dbConn, Guid noteId, Guid? userId)
        {
            int rowsDeleted;
            NpgsqlConnection conn = new NpgsqlConnection(dbConn);
            await conn.OpenAsync();
            string query = $"Delete from \"Notes\" where \"Note_Id\" = '{noteId}' and \"User_Id\" = '{userId}'";
            using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
            {
                rowsDeleted = await cmd.ExecuteNonQueryAsync();
            }

            NoteDeletedDto responseNote = new NoteDeletedDto
            {
                IsDeleted = true,
                RowsDeleted = rowsDeleted
            };

            return responseNote;
        }
    }
}
