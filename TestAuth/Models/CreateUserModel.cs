using System.ComponentModel.DataAnnotations;

namespace TestAuth.Models
{
    public class CreateUserModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
