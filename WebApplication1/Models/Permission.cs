using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Permission
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string? Name { get; set; }

        public bool IsDeleted { get; set; } = false;
        public DateTime? Created { get; set; } = DateTime.UtcNow;
        public DateTime? Updated { get; set; } = DateTime.UtcNow;
        public DateTime? Deleted { get; set; } = DateTime.UtcNow;
    }
}
