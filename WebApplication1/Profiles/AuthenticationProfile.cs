using AutoMapper;
using WebApplication1.Models;
using WebApplication1.ViewModels;

namespace WebApplication1.Profiles
{
    public class AuthenticationProfile : Profile
    {
        public AuthenticationProfile()
        {
            CreateMap<Authentication, AuthenticationViewModel>()
                .ForMember(x => x.Username, y => y.MapFrom(z => z.User!.Username))
                .ForMember(x => x.Email, y => y.MapFrom(z => z.User!.Email));
        }
    }
}
