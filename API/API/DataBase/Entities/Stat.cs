using System.ComponentModel.DataAnnotations;

namespace API.DataBase.Entities
{
    public class Stat
    {
        [Key]
        public string Name { get; set; }
        [Required]
        public int Points { get; set; }
    }
}
