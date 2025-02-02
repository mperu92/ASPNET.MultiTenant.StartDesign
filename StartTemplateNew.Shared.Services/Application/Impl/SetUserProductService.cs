using AutoMapper;
using Microsoft.Extensions.Logging;
using StartTemplateNew.Shared.Helpers.Extensions;
using StartTemplateNew.Shared.Models.Dto.Requests;
using StartTemplateNew.Shared.Services.Application.Helpers.SetUser;
using StartTemplateNew.Shared.Services.Domain;
using StartTemplateNew.Shared.Services.Extensions;
using StartTemplateNew.Shared.Services.Models;
using StartTemplateNew.Shared.Services.Models.States;

namespace StartTemplateNew.Shared.Services.Application.Impl
{
    public class SetUserProductService : ISetUserProductService
    {
        private readonly ILogger<SetUserProductService> _logger;
        private readonly IUserService _userService;
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public SetUserProductService(ILogger<SetUserProductService> logger, IMapper mapper, IUserService userService, IProductService productService)
        {
            _logger = logger;
            _mapper = mapper;
            _userService = userService;
            _productService = productService;
        }

        /// <summary>
        /// Implements ROP pattern to set a product for a user, ensuring that always a response is returned.
        /// </summary>
        /// <param name="request">The client json request</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns>A dataless service response that notices what occurred</returns>
        public async Task<ServiceResponse> SetUserProductAsync(SetUserProductRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting SetUserProductAsync for UserId: '{UserId}', ProductId: '{ProductId}'", request.UserId, request.ProductId);

            try
            {
                SetUserContext context = new();

                return await _userService
                    .RetrieveAndValidateUserAsync(request.UserId, _logger, cancellationToken)
                    .Meanwhile(user => context.User = user)
                    .Continue(_ => _productService.RetrieveAndValidateProductAsync(request.ProductId, _logger, cancellationToken)
                        .Meanwhile(prod => context.Product = prod))
                    .Continue(_ => context.MapToEntities(_mapper, _logger))
                    .EndsWithNoValue(_ => _userService.SetUserProductAsync(context.UserEntity, context.ProductEntity, request.ExpirationDate, _logger, cancellationToken))
                    .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in SetUserProductAsync: {Message}", ex.Message);
                return ServiceResponse.Error(ex.GetFullMessage());
            }
        }
    }
}
