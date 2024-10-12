using AutoMapper;
using WebApplication1.DTOs;
using WebApplication1.Models;
using WebApplication1.ViewModels;

namespace WebApplication1.Profiles
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<Role, RoleViewModel>().ReverseMap();
            CreateMap<CreateRoleDto, Role>().ReverseMap();
            CreateMap<UpdateRoleDto, Role>().ReverseMap();
        }
    }
}
