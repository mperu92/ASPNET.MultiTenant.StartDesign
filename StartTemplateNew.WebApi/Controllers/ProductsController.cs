﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using StartTemplateNew.Shared.Helpers.Attributes;
using StartTemplateNew.Shared.Helpers.Extensions;
using StartTemplateNew.Shared.Models.Dto;
using StartTemplateNew.Shared.Models.Dto.Products;
using StartTemplateNew.Shared.Models.Dto.Requests;
using StartTemplateNew.Shared.Services.Application;
using StartTemplateNew.Shared.Services.Domain;
using StartTemplateNew.Shared.Services.Factories;
using StartTemplateNew.Shared.Services.Models;
using StartTemplateNew.WebApi.Controllers.Base.ApiVersions;

namespace StartTemplateNew.WebApi.Controllers
{
    [ControllerNestedRoute(".prds", typeof(ProductsController))]
    public class ProductsController : V1ApiController<ProductsController>
    {
        private readonly IProductService _productService;
        private readonly ISetUserProductService _setUserProductService;

        public ProductsController(ILogger<ProductsController> logger, IServiceFactory serviceFactory, IOptionsMonitor<AppSettings> options)
            : base(logger, serviceFactory, options)
        {
            _productService = GetService<IProductService>();
            _setUserProductService = GetService<ISetUserProductService>();
        }

        [HttpPost("all")]
        public async Task<IActionResult> GetProducts([FromBody] GetProductsRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                ServiceResponse<ICollection<Product>> response = await _productService.GetProductsAsync(request, cancellationToken).ConfigureAwait(false);
                if (!response.IsSuccess)
                    return BadRequest(response.Message);

                return Ok(response.Data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.GetFullMessage());
            }
        }

        [Authorize(Roles = "SysAdmin,TenantAdmin")]
        [HttpPost("create-update")]
        public async Task<IActionResult> CreateUpdateProduct([FromBody] CreateUpdateProductRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                ServiceResponse<EntityStateInfo> response = await _productService.CreateUpdateProductAsync(request, cancellationToken).ConfigureAwait(false);
                if (!response.IsSuccess)
                    return BadRequest(response.Message);

                return Ok(response.Data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.GetFullMessage());
            }
        }

        [Authorize(Roles = "SysAdmin,TenantAdmin")]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteProduct(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                ServiceResponse<EntityStateInfo> response = await _productService.DeleteProductAsync(id, cancellationToken).ConfigureAwait(false);
                if (!response.IsSuccess)
                    return BadRequest(response.Message);

                return Ok(response.Data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.GetFullMessage());
            }
        }

        [Authorize(Roles = "SysAdmin")]
        [HttpPost("set-tenantproduct")]
        public async Task<IActionResult> SetTenantProduct([FromBody] SetTenantProductRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                ServiceResponse<EntityStateInfo> response = await _productService.SetTenantProductAsync(request, cancellationToken).ConfigureAwait(false);
                if (!response.IsSuccess)
                    return BadRequest(response.Message);

                return Ok(response.Data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.GetFullMessage());
            }
        }

        [Authorize(Roles = "SysAdmin,TenantAdmin")]
        [HttpPost("set-user-prod")]
        public async Task<IActionResult> SetUserProduct([FromBody] SetUserProductRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                ServiceResponse response = await _setUserProductService.SetUserProductAsync(request, cancellationToken).ConfigureAwait(false);
                if (!response.IsSuccess)
                    return BadRequest(response.Message);

                return Ok(response.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.GetFullMessage());
            }
        }
    }
}
