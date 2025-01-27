using FluentValidation;
using StartTemplateNew.Shared.Models.Dto.Requests;

namespace StartTemplateNew.Shared.FluentValidation.Services
{
    public class SetTenantServiceRequestValidator : AbstractValidator<SetTenantServiceRequest>
    {
        public SetTenantServiceRequestValidator()
        {
            RuleFor(x => x.ServiceId).NotEmpty().WithMessage("ServiceId missing");
            RuleFor(x => x.TenantId).NotEmpty().WithMessage("TenantId missing");
        }
    }
}
