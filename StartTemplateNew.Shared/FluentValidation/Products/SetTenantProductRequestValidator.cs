using FluentValidation;
using StartTemplateNew.Shared.Models.Dto.Requests;

namespace StartTemplateNew.Shared.FluentValidation.Products
{
    public class SetTenantProductRequestValidator : AbstractValidator<SetTenantProductRequest>
    {
        public SetTenantProductRequestValidator()
        {
            RuleFor(x => x.ProductId)
                .NotEmpty()
                .WithMessage("Product id cannot be empty");

            RuleFor(x => x.TenantId)
                .NotEmpty()
                .WithMessage("Tenant id cannot be empty");
        }
    }
}
