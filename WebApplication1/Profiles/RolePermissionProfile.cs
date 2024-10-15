using AutoMapper;
using WebApplication1.DTOs;
using WebApplication1.Models;
using WebApplication1.ViewModels;

namespace WebApplication1.Profiles
{
    public class RolePermissionProfile : Profile
    {
        public RolePermissionProfile()
        {
            CreateMap<CreateRolePermissionDto, RolePermission>().ReverseMap();

            CreateMap<RolePermission, RolePermissionViewModel>()
                .ForMember(x => x.RoleName, y => y.MapFrom(z => z.Role!.Name!))
                .ForMember(x => x.PermissionName, y => y.MapFrom(z => z.Permission!.Name!));
        }
    }
}
