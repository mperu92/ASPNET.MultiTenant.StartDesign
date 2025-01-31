using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StartTemplateNew.DAL.Entities;
using StartTemplateNew.DAL.Entities.Identity;
using StartTemplateNew.DAL.Entities.Tenant;
using StartTemplateNew.DAL.Repositories.Core;
using StartTemplateNew.DAL.Repositories.Core.Base;
using StartTemplateNew.DAL.Repositories.Core.Impl;
using StartTemplateNew.DAL.Repositories.Helpers;
using StartTemplateNew.DAL.Repositories.Helpers.Tenant;
using StartTemplateNew.DAL.Repositories.Models;
using StartTemplateNew.DAL.UnitOfWork.Core;
using StartTemplateNew.Shared.Enums;
using StartTemplateNew.Shared.Helpers;
using StartTemplateNew.Shared.Helpers.Extensions;
using StartTemplateNew.Shared.Models.Dto;
using StartTemplateNew.Shared.Models.Dto.Base.Requests;
using StartTemplateNew.Shared.Models.Dto.Products;
using StartTemplateNew.Shared.Models.Dto.Requests;
using StartTemplateNew.Shared.Services.Models;
using System.Linq.Expressions;

namespace StartTemplateNew.Shared.Services.Domain.Impl
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        private readonly IProductRepository _productRepo;
        private readonly IServiceRepository _serviceRepo;
        private readonly ITenantRespository _tenantRepo;

        private readonly ITenantedRepository<TenantProductEntity, int, TenantEntity, Guid, UserEntity, Guid> _tenantProductRepo;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _productRepo = _unitOfWork.GetClaimedRepoImpl<IProductRepository>();
            _tenantRepo = _unitOfWork.GetClaimedRepoImpl<ITenantRespository>();
            _serviceRepo = _unitOfWork.GetClaimedRepoImpl<IServiceRepository>();
            _tenantProductRepo = _unitOfWork.GetTenantedRepository<TenantProductEntity, int, TenantEntity, Guid, UserEntity, Guid>();
        }

        public async Task<ServiceResponse<ICollection<Product>>> GetProductsAsync(GetProductsRequest request, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            try
            {
                Expression<Func<ProductEntity, bool>>? filter = null;
                if (_productRepo.IsLoggedInWithTenant)
                    TenantHandler.MergeManyToManyFilterWithTenant(ref filter, _tenantProductRepo.TenantId, _tenantProductRepo.ClaimUser, x => x.TenantProducts.Any(x => x.TenantId == _tenantProductRepo.TenantId));

                if (request.HasSearchFilters)
                {
                    foreach (RequestSearchField field in request.SearchFields)
                    {
                        Expression<Func<ProductEntity, bool>> filterExpression = FilteringHelper<ProductEntity>.CreateExpressionFromSearchField(field);
                        filter = filter is null ? filterExpression : filter.AndAlso(filterExpression);
                    }
                }

                ICollection<Expression<Func<ProductEntity, object>>> includes =
                [
                    x => x.Service
                ];

                QueryTotalCountPair<ProductEntity> queryResp =
                    await _productRepo.GetAllWithCountAsync(filter, includes: includes, cancellationToken: cancellationToken).ConfigureAwait(false);

                IQueryable<ProductEntity> prdsQuery = queryResp.Query;
                ICollection<ProductEntity> prdsList = await prdsQuery.ToListAsync(cancellationToken).ConfigureAwait(false);

                return ServiceResponse<ICollection<Product>>.Success(_mapper.Map<ICollection<Product>>(prdsList), request.Pagination.SetTotalCount(queryResp.TotalCount));
            }
            catch (Exception ex)
            {
                return ServiceResponse<ICollection<Product>>.Error($"Error getting products.\n{ex.GetFullMessage()}");
            }
        }

        public async Task<ServiceResponse<EntityStateInfo>> CreateUpdateProductAsync(CreateUpdateProductRequest request, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            try
            {
                ProductEntity entity = _mapper.Map<ProductEntity>(request);

                ServiceEntity? serviceEntity = await _serviceRepo
                    .GetByFilterAsync(x => x.Id == entity.ServiceId, cancellationToken: cancellationToken)
                    .ConfigureAwait(false);
                if (serviceEntity is null)
                    return ServiceResponse<EntityStateInfo>.Error("Service not found.");

                entity.NiceUrl = entity.Name.ToNiceUrl();

                EntityStateInfo stateInfo;
                if (request.Id == Guid.Empty)
                    stateInfo = await CreateProductAsync(entity, cancellationToken).ConfigureAwait(false);
                else
                    stateInfo = await UpdateProductAsync(entity, cancellationToken).ConfigureAwait(false);

                await _unitOfWork.CommitAsync(cancellationToken).ConfigureAwait(false);
                return ServiceResponse<EntityStateInfo>.Success(stateInfo);
            }
            catch (Exception ex)
            {
                return ServiceResponse<EntityStateInfo>.Error($"Error managing product.\n{ex.GetFullMessage()}");
            }
        }

        public async Task<ServiceResponse<EntityStateInfo>> DeleteProductAsync(Guid id, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            try
            {
                ProductEntity? product = await _productRepo.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);
                if (product is null)
                    return ServiceResponse<EntityStateInfo>.Error("Product not found.");

                product.IsDeleted = true;

                await _productRepo.UpdateAsync(product, cancellationToken).ConfigureAwait(false);
                await _unitOfWork.CommitAsync(cancellationToken).ConfigureAwait(false);

                return ServiceResponse<EntityStateInfo>.Success(new EntityStateInfo(id.ToString(), "Product deleted successfully.", EntityStatus.Deleted));
            }
            catch (Exception ex)
            {
                return ServiceResponse<EntityStateInfo>.Error($"Error deleting product.\n{ex.GetFullMessage()}");
            }
        }

        public async Task<ServiceResponse<EntityStateInfo>> SetTenantProductAsync(SetTenantProductRequest request, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            try
            {
                TenantEntity? tenant =
                    await _tenantRepo.GetByFilterAsync(x => x.Id == request.TenantId, cancellationToken: cancellationToken).ConfigureAwait(false);
                if (tenant is null)
                    return ServiceResponse<EntityStateInfo>.Error("Tenant not found.");
                ProductEntity? product =
                    await _productRepo.GetByFilterAsync(x => x.Id == request.ProductId, cancellationToken: cancellationToken).ConfigureAwait(false);
                if (product is null)
                    return ServiceResponse<EntityStateInfo>.Error("Product not found.");

                TenantProductEntity? tenantProduct = await _tenantProductRepo
                    .GetByFilterAsync(x => x.ProductId == request.ProductId && x.TenantId == request.TenantId, cancellationToken: cancellationToken)
                    .ConfigureAwait(false);

                EntityStatus entityStatus;
                if (tenantProduct is null)
                {
                    entityStatus = EntityStatus.Added;

                    tenantProduct = new TenantProductEntity
                    {
                        Tenant = tenant,
                        Product = product,
                        ActivationDate = request.ActivationDate,
                        ExpirationDate = request.ExpirationDate,
                    };

                    await _tenantProductRepo.AddAsync(tenantProduct, cancellationToken).ConfigureAwait(false);
                }
                else
                {
                    entityStatus = EntityStatus.Updated;

                    tenantProduct.ActivationDate = request.ActivationDate;
                    tenantProduct.ExpirationDate = request.ExpirationDate;

                    await _tenantProductRepo.UpdateAsync(tenantProduct, cancellationToken).ConfigureAwait(false);
                }

                await _unitOfWork.CommitAsync(cancellationToken).ConfigureAwait(false);
                return ServiceResponse<EntityStateInfo>.Success(new EntityStateInfo(null, "Tenant product set successfully.", entityStatus));
            }
            catch (Exception ex)
            {
                return ServiceResponse<EntityStateInfo>.Error($"Error setting tenant product.\n{ex.GetFullMessage()}");
            }
        }

        private async Task<EntityStateInfo> CreateProductAsync(ProductEntity prod, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(prod);
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                await _productRepo.AddAsync(prod, cancellationToken).ConfigureAwait(false);
                if (_productRepo.IsLoggedInWithTenant)
                {
                    TenantProductEntity tenantProduct = new(prod);
                    await _tenantProductRepo.AddAsync(tenantProduct, cancellationToken).ConfigureAwait(false);
                }

                return new EntityStateInfo(null, "Product created successfully.", EntityStatus.Added);
            }
            catch (Exception ex)
            {
                return new EntityStateInfo(message: $"Error creating product.\n{ex.GetFullMessage()}");
            }
        }

        private async Task<EntityStateInfo> UpdateProductAsync(ProductEntity prod, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(prod);
            cancellationToken.ThrowIfCancellationRequested();
            try
            {
                await _productRepo.UpdateAsync(prod, cancellationToken).ConfigureAwait(false);
                if (_productRepo.IsLoggedInWithTenant)
                {
                    TenantProductEntity? tenantProduct = await _tenantProductRepo
                        .GetByFilterAsync(x => x.ProductId == prod.Id && x.TenantId == _tenantProductRepo.TenantId, cancellationToken: cancellationToken)
                        .ConfigureAwait(false);

                    if (tenantProduct is null)
                    {
                        tenantProduct = new TenantProductEntity(prod);
                        await _tenantProductRepo.AddAsync(tenantProduct, cancellationToken).ConfigureAwait(false);
                    }
                    else
                    {
                        tenantProduct.Product = prod;
                        await _tenantProductRepo.UpdateAsync(tenantProduct, cancellationToken).ConfigureAwait(false);
                    }
                }
                return new EntityStateInfo(prod.Id.ToString(), "Product updated successfully.", EntityStatus.Updated);
            }
            catch (Exception ex)
            {
                return new EntityStateInfo(message: $"Error updating product.\n{ex.GetFullMessage()}");
            }
        }
    }
}
