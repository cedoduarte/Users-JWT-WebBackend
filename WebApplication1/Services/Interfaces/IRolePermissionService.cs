﻿using WebApplication1.DTOs;
using WebApplication1.ViewModels;

namespace WebApplication1.Services.Interfaces
{
    public interface IRolePermissionService
    {
        Task<RolePermissionViewModel> CreateAsync(CreateRolePermissionDto createRolePermissionDto, CancellationToken cancel = default);
        Task<IEnumerable<RolePermissionViewModel>> FindAllAsync(CancellationToken cancel = default);
        Task<RolePermissionViewModel> UpdateAsync(UpdateRolePermissionDto updateRolePermissionDto, CancellationToken cancel = default);
    }
}