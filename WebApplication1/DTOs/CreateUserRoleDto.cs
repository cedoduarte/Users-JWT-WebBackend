using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DTOs
{
    public class CreateUserRoleDto
    {
        [Required(ErrorMessage = "User ID is required")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Role ID is required")]
        public int RoleId { get; set; }
    }
}
