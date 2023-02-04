using System.ComponentModel.DataAnnotations;

namespace TaskRegister.API.Entities.TaskRegister
{
    public class ProductionTimeSlip
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Subject { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public Guid ProjectId { get; set; }

        [MaxLength(400)]
        [Required]
        public string AssignedTo { get; set; }

        [MaxLength(500)]
        [Required]
        public string TaskDescription { get; set; }

        [MaxLength(500)]
        public string? Comment { get; set; }

        public int Time { get; set; }

        [Required]
        public DateTime AssignedDate { get; set; }
        
        [Required]
        public DateTime DateLog { get; set;}
    }
}
