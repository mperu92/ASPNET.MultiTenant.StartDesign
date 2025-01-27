using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StartTemplateNew.DAL.Entities;
using StartTemplateNew.DAL.Entities.Tenant;
using StartTemplateNew.DAL.Repositories.Core;
using StartTemplateNew.DAL.Repositories.Models;
using StartTemplateNew.DAL.UnitOfWork.Core;
using StartTemplateNew.Shared.Enums;
using StartTemplateNew.Shared.Helpers.Extensions;
using StartTemplateNew.Shared.Models.Dto;
using StartTemplateNew.Shared.Models.Dto.Requests;
using StartTemplateNew.Shared.Models.Dto.Services;
using StartTemplateNew.Shared.Services.Models;
using System.Linq.Expressions;

namespace StartTemplateNew.Shared.Services.Core.Impl
{
    public class ServiceService : IServiceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        private readonly IServiceRepository _serviceRepo;
        private readonly ITenantRespository _tenantRepo;
        private readonly ITenantServiceRepository _tenantServiceRepo;

        public ServiceService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _serviceRepo = unitOfWork.GetClaimedRepoImpl<IServiceRepository>();
            _tenantRepo = unitOfWork.GetClaimedRepoImpl<ITenantRespository>();
            _tenantServiceRepo = unitOfWork.GetTenantedRepoImpl<ITenantServiceRepository>();
        }

        public async Task<ServiceResponse<EntityStateInfo>> CreateUpdateServiceAsync(CreateUpdateServiceRequest request, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                ServiceEntity entity = _mapper.Map<ServiceEntity>(request);

                EntityStateInfo stateInfo;
                if (request.Id == Guid.Empty)
                {
                    stateInfo = await CreateServiceAsync(entity, cancellationToken).ConfigureAwait(false);
                }
                else
                {
                    stateInfo = await UpdateServiceAsync(entity, cancellationToken).ConfigureAwait(false);
                }

                await _unitOfWork.CommitAsync(cancellationToken).ConfigureAwait(false);

                return ServiceResponse<EntityStateInfo>.Success(stateInfo);
            }
            catch (Exception ex)
            {
                return ServiceResponse<EntityStateInfo>.Error($"Error managing service.\n{ex.GetFullMessage()}");
            }
        }

        public async Task<ServiceResponse<EntityStateInfo>> DeleteServiceAsync(Guid serviceId, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            try
            {
                ServiceEntity? service = await _serviceRepo.GetByIdAsync(serviceId, cancellationToken).ConfigureAwait(false);
                if (service is null)
                {
                    return ServiceResponse<EntityStateInfo>.Error("Service not found.");
                }

                service.IsDeleted = true;

                await _serviceRepo.UpdateAsync(service, cancellationToken).ConfigureAwait(false);
                await _unitOfWork.CommitAsync(cancellationToken).ConfigureAwait(false);

                return ServiceResponse<EntityStateInfo>.Success(new EntityStateInfo(null, "Service deleted successfully.", EntityStatus.Deleted));
            }
            catch (Exception ex)
            {
                return ServiceResponse<EntityStateInfo>.Error($"Error deleting service.\n{ex.GetFullMessage()}");
            }
        }

        public async Task<ServiceResponse<ICollection<Service>>> GetServicesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                ICollection<Expression<Func<ServiceEntity, object>>> includes;
                if (_tenantServiceRepo.IsLoggedInWithTenant)
                {
                    includes =
                    [
                        x => x.Products.Where(x => x.TenantProducts.Any(a => a.TenantId == _tenantServiceRepo.TenantId)),
                    ];
                }
                else
                {
                    includes =
                    [
                        x => x.Products,
                    ];
                }

                QueryTotalCountPair<ServiceEntity> queryTotalCount =
                    await _tenantServiceRepo.GetAllWithCountAsync(pageSize: 1000, includes: includes, cancellationToken: cancellationToken).ConfigureAwait(false);

                ICollection<Service> serviceDtos =
                    _mapper.Map<ICollection<Service>>(await queryTotalCount.Query.ToListAsync(cancellationToken).ConfigureAwait(false));

                return ServiceResponse<ICollection<Service>>.Success(serviceDtos, "Services successfully loaded");
            }
            catch (Exception ex)
            {
                return ServiceResponse<ICollection<Service>>.Error($"Error getting services.\n{ex.GetFullMessage()}");
            }
        }

        public async Task<ServiceResponse<EntityStateInfo>> SetTenantServiceAsync(SetTenantServiceRequest request, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (!await _tenantRepo.AnyAsync(a => a.Id == request.TenantId, cancellationToken).ConfigureAwait(false))
                return ServiceResponse<EntityStateInfo>.Error($"Tenant with id '{request.TenantId}' not found.");
            if (!await _serviceRepo.AnyAsync(a => a.Id == request.ServiceId, cancellationToken).ConfigureAwait(false))
                return ServiceResponse<EntityStateInfo>.Error($"Service with id '{request.ServiceId}' not found.");

            try
            {
                TenantServiceEntity? tenantService = await _tenantServiceRepo
                    .GetByFilterAsync(x => x.ServiceId == request.ServiceId && x.TenantId == request.TenantId, cancellationToken: cancellationToken)
                    .ConfigureAwait(false);

                EntityStatus entityStatus;
                if (tenantService is null)
                {
                    entityStatus = EntityStatus.Added;

                    tenantService = new TenantServiceEntity
                    {
                        ServiceId = request.ServiceId,
                        TenantId = request.TenantId,
                        ActivationDate = request.ActivationDate,
                        ExpirationDate = request.ExpirationDate,
                    };

                    await _tenantServiceRepo.AddAsync(tenantService, cancellationToken).ConfigureAwait(false);
                }
                else
                {
                    entityStatus = EntityStatus.Updated;

                    tenantService.ActivationDate = request.ActivationDate;
                    tenantService.ExpirationDate = request.ExpirationDate;

                    await _tenantServiceRepo.UpdateAsync(tenantService, cancellationToken).ConfigureAwait(false);
                }

                await _unitOfWork.CommitAsync(cancellationToken).ConfigureAwait(false);
                return ServiceResponse<EntityStateInfo>.Success(new EntityStateInfo(null, "Tenant service updated successfully.", entityStatus));
            }
            catch (Exception ex)
            {
                return ServiceResponse<EntityStateInfo>.Error($"Error setting tenant service.\n{ex.GetFullMessage()}");
            }
        }

        private async Task<EntityStateInfo> CreateServiceAsync(ServiceEntity service, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(service);
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                await _serviceRepo.AddAsync(service, cancellationToken).ConfigureAwait(false);
                if (_serviceRepo.IsLoggedInWithTenant)
                {
                    TenantServiceEntity tenantService = new(service);
                    await _tenantServiceRepo.AddAsync(tenantService, cancellationToken).ConfigureAwait(false);
                }

                return new EntityStateInfo(service.Id.ToString(), "Service created successfully.", EntityStatus.Added);
            }
            catch (Exception ex)
            {
                return new EntityStateInfo(message: $"Error creating service.\n{ex.GetFullMessage()}");
            }
        }

        private async Task<EntityStateInfo> UpdateServiceAsync(ServiceEntity service, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(service);
            cancellationToken.ThrowIfCancellationRequested();
            try
            {
                await _serviceRepo.UpdateAsync(service, cancellationToken).ConfigureAwait(false);
                if (_serviceRepo.IsLoggedInWithTenant)
                {
                    TenantServiceEntity? tenantProduct = await _tenantServiceRepo
                        .GetByFilterAsync(x => x.ServiceId == service.Id && x.TenantId == _tenantServiceRepo.TenantId, cancellationToken: cancellationToken)
                        .ConfigureAwait(false);

                    if (tenantProduct is null)
                    {
                        tenantProduct = new TenantServiceEntity(service);
                        await _tenantServiceRepo.AddAsync(tenantProduct, cancellationToken).ConfigureAwait(false);
                    }
                    else
                    {
                        tenantProduct.Service = service;
                        await _tenantServiceRepo.UpdateAsync(tenantProduct, cancellationToken).ConfigureAwait(false);
                    }
                }

                return new EntityStateInfo(service.Id.ToString(), "Service updated successfully.", EntityStatus.Added);
            }
            catch (Exception ex)
            {
                return new EntityStateInfo(message: $"Error updating service.\n{ex.GetFullMessage()}");
            }
        }
    }
}
