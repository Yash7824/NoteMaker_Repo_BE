using System.ComponentModel.DataAnnotations;

namespace Notes.API.Models
{
    public class Note
    {
        [Key]
        public Guid Note_Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Tag { get; set; }
        public DateTime? AddedOn { get; set; } = DateTime.UtcNow;
        public Guid? User_Id { get; set; }


    }
}
