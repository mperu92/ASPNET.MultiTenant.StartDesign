using FluentValidation;
using StartTemplateNew.Shared.Models.Dto.Requests;

namespace StartTemplateNew.Shared.FluentValidation.Identity
{
    public class CreateRoleFormRequestValidator : AbstractValidator<CreateRoleRequest>
    {
        public CreateRoleFormRequestValidator()
        {
            RuleFor(p => p.Name)
                .MaximumLength(30).WithMessage("Role name must not exceed 30 characters.")
                .NotEmpty().WithMessage("Role name is required.");

            RuleFor(p => p.Description)
                .MaximumLength(50).WithMessage("Role description must not exceed 50 characters.")
                .NotEmpty().WithMessage("Role description is required.");
        }
    }
}
