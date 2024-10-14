using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DTOs
{
    public class UpdatePermissionDto : CreatePermissionDto
    {
        [Required]
        public int Id { get; set; }
    }
}
