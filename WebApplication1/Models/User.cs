using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(256)]
        public string? Username { get; set; }

        [Required]
        public string? PasswordHash { get; set; }

        [Required]
        [StringLength(256)]
        public string? FirstName { get; set; }

        [Required]
        [StringLength(256)]
        public string? LastName { get; set; }

        [Required]
        [StringLength(256)]
        [EmailAddress]
        public string? Email { get; set; }

        public bool IsDeleted { get; set; } = false;

        public DateTime? Created { get; set; } = DateTime.UtcNow;
        public DateTime? Updated { get; set; } = DateTime.UtcNow;
        public DateTime? Deleted { get; set; } = DateTime.UtcNow;
    }
}
