namespace WebApplication1.Services.Interfaces
{
    public interface IAuthorizationService
    {
        Task<bool> HasPermissionAsync(string? role, string permission, CancellationToken cancel = default);
    }
}
