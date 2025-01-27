using FluentValidation;
using StartTemplateNew.Shared.Models.Dto.Requests;

namespace StartTemplateNew.Shared.FluentValidation.Identity
{
    public class CreateUserFormRequestValidator : AbstractValidator<CreateUpdateUserFormRequest>
    {
        public CreateUserFormRequestValidator()
        {
            RuleFor(x => x.UserName)
                .MaximumLength(16).WithMessage("Username must not exceed 16 characters.")
                .NotEmpty().WithMessage("Username is required.");

            RuleFor(x => x.Email)
                .MaximumLength(100).WithMessage("Email must not exceed 100 characters.")
                .NotEmpty().WithMessage("Email is required.");

            RuleFor(x => x.Password)
                .MaximumLength(30).WithMessage("Password must not exceed 30 characters.")
                .NotEmpty().WithMessage("Password is required.");

            RuleFor(x => x.FirstName)
                .MaximumLength(50).WithMessage("First name must not exceed 50 characters.")
                .NotEmpty().WithMessage("First name is required.");

            RuleFor(x => x.LastName)
                .MaximumLength(50).WithMessage("Last name must not exceed 50 characters.")
                .NotEmpty().WithMessage("Last name is required.");
        }
    }
}
