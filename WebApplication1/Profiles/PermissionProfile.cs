using AutoMapper;
using WebApplication1.DTOs;
using WebApplication1.Models;
using WebApplication1.ViewModels;

namespace WebApplication1.Profiles
{
    public class PermissionProfile : Profile
    {
        public PermissionProfile()
        {
            CreateMap<Permission, PermissionViewModel>().ReverseMap();
            CreateMap<CreatePermissionDto, Permission>().ReverseMap();
            CreateMap<UpdatePermissionDto, Permission>().ReverseMap();
        }
    }
}
