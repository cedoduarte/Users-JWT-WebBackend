using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Role
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string? Name { get; set; } = string.Empty;

        public bool IsDeleted { get; set; } = false;
        public DateTime? Created { get; set; } = DateTime.UtcNow;
        public DateTime? Updated { get; set; } = DateTime.UtcNow;
        public DateTime? Deleted { get; set; } = DateTime.UtcNow;
    }
}
