using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DTOs
{
    public class AuthenticateDto
    {
        [Required(ErrorMessage = "Username is required")]
        [StringLength(256, ErrorMessage = "Username cannot be longer than 256 characters")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(256, MinimumLength = 8, ErrorMessage = "Password must be at leat 8 characters long")]
        public string? Password { get; set; }
    }
}
