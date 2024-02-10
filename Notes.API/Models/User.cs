using System.ComponentModel.DataAnnotations;

namespace Notes.API.Models
{
    public class User
    {
        [Key]
        public Guid User_Id { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Role { get; set; }


    }
}
