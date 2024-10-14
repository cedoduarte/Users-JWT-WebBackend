using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DTOs
{
    public class UpdateRoleDto : CreateRoleDto
    {
        [Required]
        public int Id { get; set; }
    }
}
