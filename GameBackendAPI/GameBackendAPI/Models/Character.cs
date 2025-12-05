using System.ComponentModel.DataAnnotations;

namespace GameBackendAPI.Models
{
    public class Character
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        public int Level { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}
