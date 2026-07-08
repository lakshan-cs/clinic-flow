using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicFlow.Models
{
    [Table("USERS")]
    public class User
    {
        [Column("ID")]
        public int Id { get; set; }
        [Column("USERNAME")]
        public string Username { get; set; }
        [Column("EMAIL")]
        public string Email { get; set; }
        [Column("PASSWORD")]
        public string Password { get; set; }
        [Column("ROLE")]
        public string Role { get; set; } = "ADMIN";

    }
}
