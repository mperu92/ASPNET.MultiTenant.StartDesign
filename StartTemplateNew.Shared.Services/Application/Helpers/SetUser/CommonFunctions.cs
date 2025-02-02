using AutoMapper;
using Microsoft.Extensions.Logging;
using StartTemplateNew.DAL.Entities;
using StartTemplateNew.DAL.Entities.Identity;
using StartTemplateNew.Shared.Models;
using StartTemplateNew.Shared.Models.Dto.Identity;
using StartTemplateNew.Shared.Models.Dto.Products;
using StartTemplateNew.Shared.Services.Application.Impl;
using StartTemplateNew.Shared.Services.Domain;
using StartTemplateNew.Shared.Services.Models;
using StartTemplateNew.Shared.Services.Models.States;
using System.Linq.Expressions;

namespace StartTemplateNew.Shared.Services.Application.Helpers.SetUser
{
    public static class CommonFunctions
    {
        public static async Task<ServiceResponse<User>> RetrieveAndValidateUserAsync(this IUserService userService, Guid userId, ILogger<SetUserProductService> logger, CancellationToken cancellationToken)
        {
            logger.LogInformation("Retrieving user with id '{UserId}'", userId);

            ICollection<Expression<Func<UserEntity, object>>> includes =
            [
                i => i.UserProducts.Where(p => !p.ExpirationDate.HasValue || p.ExpirationDate.Value > DateTimeOffset.Now)
            ];

            ServiceResponse<User?> userResp = await userService.GetUserByIdAsync(userId, includes, cancellationToken).ConfigureAwait(false);
            if (userResp.IsError)
            {
                logger.LogError("Error retrieving user: {Message}", userResp.Message);
                return ServiceResponse<User>.Error($"Error retrieving user: {userResp.Message}");
            }

            if (!userResp.HasData)
            {
                logger.LogError("User with id '{UserId}' not found.\n{Message}", userId, userResp.Message);
                return ServiceResponse<User>.Error($"User with id '{userId}' not found");
            }

            return userResp!;
        }

        // i could return the domain service response directly, but its response data needs to be not null here
        public static async Task<ServiceResponse<Product>> RetrieveAndValidateProductAsync(this IProductService productService, Guid productId, ILogger<SetUserProductService> logger, CancellationToken cancellationToken)
        {
            logger.LogInformation("Retrieving product with id '{ProductId}'", productId);
            ServiceResponse<Product?> productResp = await productService.GetProductByIdAsync(productId, cancellationToken).ConfigureAwait(false);
            if (productResp.IsError)
            {
                logger.LogError("Error retrieving product: {Message}", productResp.Message);
                return ServiceResponse<Product>.Error(productResp.Message);
            }

            if (!productResp.HasData)
            {
                logger.LogError("Product with id '{ProductId}' not found.\n{Message}", productId, productResp.Message);
                return ServiceResponse<Product>.Error($"Error retrieving product: {productResp.Message}");
            }

            Product product = productResp.Data;
            return ServiceResponse<Product>.Success(product, productResp.Message);
        }

        public static Task<ServiceResponse<Unit>> MapToEntities(this SetUserContext context, IMapper _mapper, ILogger<SetUserProductService> logger)
        {
            logger.LogInformation("Mapping user and product to entities");

            context.UserEntity = _mapper.Map<UserEntity>(context.User);
            context.ProductEntity = _mapper.Map<ProductEntity>(context.Product);

            return Task.FromResult(ServiceResponse<Unit>.Success(Unit.Value));
        }

        public static async Task<ServiceResponse> SetUserProductAsync(this IUserService userService, UserEntity userEntity, ProductEntity productEntity, DateTimeOffset? expirationDate, ILogger<SetUserProductService> logger, CancellationToken cancellationToken)
        {
            logger.LogInformation("Setting user product for UserId: {UserId}, ProductId: {ProductId}", userEntity.Id, productEntity.Id);
            return await userService.SetUserProductAsync(userEntity, productEntity, expirationDate, cancellationToken).ConfigureAwait(false);
        }
    }
}
