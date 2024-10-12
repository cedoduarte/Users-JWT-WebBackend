using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DTOs
{
    public class CreateRoleDto
    {
        [Required]
        [StringLength(256, MinimumLength = 1, ErrorMessage = "Name must be at least 1 characters long and cannot be longer than 256 characters")]
        public string? Name { get; set; }
    }
}
