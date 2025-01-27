using FluentValidation;
using StartTemplateNew.Shared.Models.Dto.Requests;

namespace StartTemplateNew.Shared.FluentValidation.Products
{
    public class CreateUpdateProductRequestValidator : AbstractValidator<CreateUpdateProductRequest>
    {
        public CreateUpdateProductRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(50).WithMessage("Name must not exceed 50 characters.");

            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("Code is required.")
                .MaximumLength(10).WithMessage("Code must not exceed 10 characters");

            RuleFor(x => x.ShortDescription)
                .NotEmpty().WithMessage("ShortDescription is required.")
                .MaximumLength(150).WithMessage("ShortDescription must not exceed 150 characters");

            RuleFor(x => x.ServiceId)
                .NotEmpty().WithMessage("ServiceId is required.");
        }
    }
}
