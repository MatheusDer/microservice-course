using System.ComponentModel.DataAnnotations;

namespace CommandService.Models
{
    public class Platform
    {
        public Platform()
        {
            Commands = new List<Command>();
        }

        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public int ExternalId { get; set; }

        [Required]
        public string? Name { get; set; }

        public ICollection<Command> Commands { get; set; }
    }
}