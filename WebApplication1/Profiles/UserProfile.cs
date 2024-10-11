using AutoMapper;
using WebApplication1.DTOs;
using WebApplication1.Models;
using WebApplication1.Utilities;
using WebApplication1.ViewModels;

namespace WebApplication1.Profiles
{
    public class UserProfile :  Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserViewModel>().ReverseMap();
            CreateMap<CreateUserDto, User>()
                .ForMember(a => a.PasswordHash, b => b.MapFrom(c => Util.GetSha256Hash(c.Password!)))
                .ReverseMap();
            CreateMap<UpdateUserDto, User>()
                .ForMember(a => a.PasswordHash, b => b.MapFrom(c => Util.GetSha256Hash(c.Password!)))
                .ReverseMap();
        }
    }
}