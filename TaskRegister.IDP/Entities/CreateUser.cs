using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TaskRegister.IDP.Entities
{
    public class CreateUser
    {
        [Required]
        [MaxLength(200)]
        public string UserName { get; set; }
        [MaxLength(200)]
        public string Password { get; set; }
        [Required]
        [MaxLength(250)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(250)]
        public string LastName { get; set; }
    }
}
