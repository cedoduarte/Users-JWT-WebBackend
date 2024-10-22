using System.ComponentModel.DataAnnotations;
using WebApplication1.Validators;

namespace WebApplication1.DTOs
{
    [PasswordsMatch]
    public class UpdateUserDto : CreateUserDto
    {
        [Required]
        public int? Id { get; set; }
    }
}
