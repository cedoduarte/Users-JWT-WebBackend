namespace WebApplication1.Services.Interfaces
{
    public interface IAuthorizationService
    {
        Task<bool> HasPermissionAsync(string userIdString, string requiredPermission, CancellationToken cancel = default);
    }
}
