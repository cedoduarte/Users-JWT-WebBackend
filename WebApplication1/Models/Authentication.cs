using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Authentication
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public DateTime? Date { get; set; }

        public virtual User? User { get; set; }
    }
}
