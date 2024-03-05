using System.ComponentModel.DataAnnotations;

namespace DataLayer.Models
{
    public class User_db
    {
        [Key]
        public Guid Id { get; set; }= Guid.NewGuid();
        public string Name { get; set; }
        public string Email { get; set; }

        public string ProfileImg { get; set; }

        public string Password { get; set; }

    }
}
