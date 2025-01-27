using AutoMapper;
using StartTemplateNew.DAL.Entities.Identity;
using StartTemplateNew.Shared.Models.Dto.Identity;

namespace StartTemplateNew.Shared.Mappers.Profiles
{
    public class UserRoleProfile : Profile
    {
        public UserRoleProfile()
        {
            CreateMap<UserRoleEntity, UserRole>().ReverseMap();
        }
    }
}
