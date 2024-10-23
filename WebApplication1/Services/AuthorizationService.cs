using WebApplication1.Services.Interfaces;

namespace WebApplication1.Services
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly ILogger<AuthorizationService> _logger;
        private readonly IRoleService _roleService;
        private readonly IRolePermissionService _rolePermissionService;

        public AuthorizationService(
            ILogger<AuthorizationService> logger,
            IRoleService roleService,
            IRolePermissionService rolePermissionService)
        {
            _logger = logger;
            _roleService = roleService;
            _rolePermissionService = rolePermissionService;
        }

        public async Task<bool> HasPermissionAsync(string? role, string permission, CancellationToken cancel)
        {
            if (role is null)
            {
                _logger.LogWarning("User role is not found in the claims");
                return false;
            }
            var foundRole = await _roleService.FindOneAsync(role!, cancel);
            var foundPermissions = await _rolePermissionService.GetPermissions(foundRole.Id, cancel);
            var foundPermission = foundPermissions.Where(x => string.Equals(x.Name, permission)).FirstOrDefault();
            return foundPermission is not null;
        }
    }
}
