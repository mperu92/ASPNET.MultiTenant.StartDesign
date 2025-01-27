using FluentValidation;
using StartTemplateNew.Shared.Models.Dto.Requests;

namespace StartTemplateNew.Shared.FluentValidation.Services
{
    public class CreateUpdateServiceRequestValidator : AbstractValidator<CreateUpdateServiceRequest>
    {
        public CreateUpdateServiceRequestValidator()
        {
            RuleFor(x => x.Name)
                .MaximumLength(30).WithMessage("Name must be less than 30 characters")
                .NotEmpty().WithMessage("Name must be not empty");

            RuleFor(x => x.Description)
                .MaximumLength(100).WithMessage("Description must be less than 100 characters");

            RuleFor(x => x.Code)
                .MaximumLength(10).WithMessage("Code must be less than 10 characters");
        }
    }
}
