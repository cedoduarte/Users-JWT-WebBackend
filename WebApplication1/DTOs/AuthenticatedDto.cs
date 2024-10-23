using WebApplication1.ViewModels;

namespace WebApplication1.DTOs
{
    public class AuthenticatedDto
    {
        public string? Token { get; set; }
        public AuthenticationViewModel? Authentication { get; set; }
    }
}
