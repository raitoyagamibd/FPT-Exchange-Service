using AutoMapper;
using Data.Entities;
using Data.Models.View;

namespace Data.Mapping
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {
            CreateMap<User, UserViewModel>();
            CreateMap<Role, RoleViewModel>();
        }
    }
}
