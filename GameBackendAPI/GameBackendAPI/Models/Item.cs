using System.ComponentModel.DataAnnotations;

namespace GameBackendAPI.Models
{
    public class Item
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public string Description { get; set; }

        public int CharacterId { get; set; }
        public Character Character { get; set; }
    }
}
