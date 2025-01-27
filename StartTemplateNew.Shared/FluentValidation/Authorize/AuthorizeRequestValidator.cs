using FluentValidation;
using StartTemplateNew.Shared.Models.Dto.Requests;

namespace StartTemplateNew.Shared.FluentValidation.Authorize
{
    public class AuthorizeRequestValidator : AbstractValidator<AuthorizeRequest>
    {
        public AuthorizeRequestValidator()
        {
            RuleFor(x => x.ClientId).NotEmpty().WithMessage("ClientId is required");
            RuleFor(x => x.RedirectUri).NotEmpty().WithMessage("RedirectUri is required");
            RuleFor(x => x.ResponseType).NotEmpty().WithMessage("ResponseType is required");
            RuleFor(x => x.ResponseType).Must(x => x == "code").WithMessage("ResponseType must be 'code'");
            RuleFor(x => x.Scope).NotEmpty().WithMessage("Scope is required");
        }
    }
}
