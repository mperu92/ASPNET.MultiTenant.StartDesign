using AutoMapper;
using StartTemplateNew.DAL.Entities.Identity;
using StartTemplateNew.Shared.Models.Dto.Identity;
using StartTemplateNew.Shared.Models.Dto.Requests;

namespace StartTemplateNew.Shared.Mappers.Profiles
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<CreateRoleRequest, Role>().ReverseMap();
            CreateMap<RoleEntity, Role>().ReverseMap();
            CreateMap<RoleEntity, CreateRoleRequest>().ReverseMap();
        }
    }
}
