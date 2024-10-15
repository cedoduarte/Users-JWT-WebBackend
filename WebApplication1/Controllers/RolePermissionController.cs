using Microsoft.AspNetCore.Mvc;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RolePermissionController : Controller
    {
        private readonly IRolePermissionService _rolePermissionService;

        public RolePermissionController(IRolePermissionService rolePermissionService)
        {
            _rolePermissionService = rolePermissionService;
        }
    }
}
