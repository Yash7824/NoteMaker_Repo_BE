namespace Notes.API.DTOs
{
    public class UserNoteDto
    {
        public Guid Note_Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Tag { get; set; }
        public DateTime? AddedOn { get; set; } = DateTime.UtcNow;
        public Guid? User_Id { get; set; }
        public string? Username { get; set; }
    }
}
