using System.ComponentModel.DataAnnotations;

namespace TaskRegister.API.Entities.Projects
{
    public class BaseProjects
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Subject { get; set; }

        [MaxLength(300)]
        [Required]
        public string Project { get; set; }

        [Required]
        public bool Active { get; set; }
    }
}
