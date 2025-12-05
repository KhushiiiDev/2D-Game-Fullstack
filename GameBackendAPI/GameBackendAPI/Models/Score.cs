using System.ComponentModel.DataAnnotations;

namespace GameBackendAPI.Models
{
    public class Score
    {
        public int Id { get; set; }

        [Required]
        public int Value { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}
