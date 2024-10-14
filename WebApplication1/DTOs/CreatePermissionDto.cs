using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DTOs
{
    public class CreatePermissionDto
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters")]
        public string? Name { get; set; }
    }
}
