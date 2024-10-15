using WebApplication1.DTOs;
using WebApplication1.ViewModels;

namespace WebApplication1.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<AuthenticationViewModel> AuthenticateAsync(AuthenticateDto authenticateDto, CancellationToken cancel = default);
    }
}
