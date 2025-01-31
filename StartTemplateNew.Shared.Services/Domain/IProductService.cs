using StartTemplateNew.Shared.Models.Dto.Requests;
using StartTemplateNew.Shared.Models.Dto;
using StartTemplateNew.Shared.Services.Models;
using StartTemplateNew.Shared.Models.Dto.Products;
using StartTemplateNew.Shared.Services.Core;

namespace StartTemplateNew.Shared.Services.Domain
{
    public interface IProductService : IService
    {
        Task<ServiceResponse<ICollection<Product>>> GetProductsAsync(GetProductsRequest request, CancellationToken cancellationToken = default);
        Task<ServiceResponse<EntityStateInfo>> CreateUpdateProductAsync(CreateUpdateProductRequest request, CancellationToken cancellationToken = default);
        Task<ServiceResponse<EntityStateInfo>> DeleteProductAsync(Guid id, CancellationToken cancellationToken = default);
        Task<ServiceResponse<EntityStateInfo>> SetTenantProductAsync(SetTenantProductRequest request, CancellationToken cancellationToken = default);
    }
}
