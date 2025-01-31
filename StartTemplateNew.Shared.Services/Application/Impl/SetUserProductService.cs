using AutoMapper;
using StartTemplateNew.DAL.Entities;
using StartTemplateNew.DAL.Entities.Identity;
using StartTemplateNew.Shared.Helpers.Extensions;
using StartTemplateNew.Shared.Models;
using StartTemplateNew.Shared.Models.Dto.Identity;
using StartTemplateNew.Shared.Models.Dto.Products;
using StartTemplateNew.Shared.Models.Dto.Requests;
using StartTemplateNew.Shared.Services.Domain;
using StartTemplateNew.Shared.Services.Extensions;
using StartTemplateNew.Shared.Services.Models;
using System.Linq.Expressions;

namespace StartTemplateNew.Shared.Services.Application.Impl
{
    public class SetUserProductService : ISetUserProductService
    {
        private readonly IUserService _userService;
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public SetUserProductService(IMapper mapper, IUserService userService, IProductService productService)
        {
            _mapper = mapper;
            _userService = userService;
            _productService = productService;
        }

        private sealed class SetUserContext
        {
            public User User { get; set; } = default!;
            public Product Product { get; set; } = default!;
            public UserEntity UserEntity { get; set; } = default!;
            public ProductEntity ProductEntity { get; set; } = default!;
        }

        public async Task<ServiceResponse> SetUserProductAsync(SetUserProductRequest request, CancellationToken cancellationToken)
        {
            try
            {
                SetUserContext context = new();

                return await RetrieveAndValidateUserAsync(request.UserId, cancellationToken)
                    .Meanwhile(user => context.User = user)
                    .Continue(_ => RetrieveAndValidateProductAsync(request.ProductId, cancellationToken).Meanwhile(prod => context.Product = prod))
                    .Continue(_ => MapToEntities(context))
                    .EndsWithNoValue(_ => SetUserProductAsync(context.UserEntity, context.ProductEntity, cancellationToken))
                    .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                return ServiceResponse.Error(ex.GetFullMessage());
            }
        }

        private async Task<ServiceResponse<User>> RetrieveAndValidateUserAsync(Guid userId, CancellationToken cancellationToken)
        {
            ICollection<Expression<Func<UserEntity, object>>> includes =
            [
                i => i.UserProducts.Where(p => !p.ExpirationDate.HasValue || p.ExpirationDate.Value > DateTimeOffset.Now)
            ];

            ServiceResponse<User?> userResp = await _userService.GetUserByIdAsync(userId, includes, cancellationToken).ConfigureAwait(false);
            if (userResp.IsError)
                return ServiceResponse<User>.Error($"Error retrieving user: {userResp.Message}");
            if (!userResp.HasData)
                return ServiceResponse<User>.Error($"User with id '{userId}' not found");

            return userResp!;
        }

        private async Task<ServiceResponse<Product>> RetrieveAndValidateProductAsync(Guid productId, CancellationToken cancellationToken)
        {
            ServiceResponse<Product?> productResp = await _productService.GetProductByIdAsync(productId, cancellationToken).ConfigureAwait(false);
            if (productResp.IsError)
                return ServiceResponse<Product>.Error(productResp.Message);
            if (!productResp.HasData)
                return ServiceResponse<Product>.Error($"Error retrieving product: {productResp.Message}");

            return productResp!;
        }

        private Task<ServiceResponse<Unit>> MapToEntities(SetUserContext context)
        {
            context.UserEntity = _mapper.Map<UserEntity>(context.User);
            context.ProductEntity = _mapper.Map<ProductEntity>(context.Product);

            return Task.FromResult(ServiceResponse<Unit>.Success(Unit.Value));
        }

        private async Task<ServiceResponse> SetUserProductAsync(UserEntity userEntity, ProductEntity productEntity, CancellationToken cancellationToken)
        {
            return await _userService.SetUserProductAsync(userEntity, productEntity, cancellationToken).ConfigureAwait(false);
        }
    }
}
