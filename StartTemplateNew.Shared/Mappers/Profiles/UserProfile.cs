using AutoMapper;
using StartTemplateNew.DAL.Entities.Identity;
using StartTemplateNew.Shared.Models.Dto.Identity;
using StartTemplateNew.Shared.Models.Dto.Requests;

namespace StartTemplateNew.Shared.Mappers.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<CreateUpdateUserRequest, User>()
                .ForSourceMember(dest => dest.RoleId, opt => opt.DoNotValidate())
                .ReverseMap();

            CreateMap<CreateUpdateUserFormRequest, User>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserRoles, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<UserEntity, User>().ReverseMap();
            CreateMap<UserEntity, CreateUpdateUserRequest>().ReverseMap();
            CreateMap<UserEntity, CreateUpdateUserWithTenantRequest>().ReverseMap();
            CreateMap<UserEntity, CreateUpdateUserFormRequest>().ReverseMap();
        }
    }
}
