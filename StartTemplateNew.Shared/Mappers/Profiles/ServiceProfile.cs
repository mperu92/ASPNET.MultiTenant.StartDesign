using AutoMapper;
using StartTemplateNew.DAL.Entities;
using StartTemplateNew.Shared.Models.Dto.Requests;
using StartTemplateNew.Shared.Models.Dto.Services;

namespace StartTemplateNew.Shared.Mappers.Profiles
{
    public class ServiceProfile : Profile
    {
        public ServiceProfile()
        {
            CreateMap<CreateUpdateServiceRequest, Service>().ReverseMap();
            CreateMap<ServiceEntity, Service>().ReverseMap();
            CreateMap<ServiceEntity, CreateUpdateServiceRequest>().ReverseMap();
        }
    }
}
