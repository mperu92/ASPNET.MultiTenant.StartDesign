using AutoMapper;
using StartTemplateNew.DAL.Entities;
using StartTemplateNew.Shared.Models.Dto.Products;
using StartTemplateNew.Shared.Models.Dto.Requests;

namespace StartTemplateNew.Shared.Mappers.Profiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<CreateUpdateProductRequest, Product>().ReverseMap();
            CreateMap<ProductEntity, Product>()
                .ReverseMap();
           CreateMap<ProductEntity, CreateUpdateProductRequest>().ReverseMap();
        }
    }
}
