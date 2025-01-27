using FluentValidation;
using StartTemplateNew.Shared.Models.Dto.Requests;

namespace StartTemplateNew.Shared.FluentValidation.Token
{
    public class TokenRequestValidator : AbstractValidator<TokenRequest>
    {
        public TokenRequestValidator()
        {
            RuleFor(x => x.ClientId).NotEmpty().WithMessage("ClientId is required");
            RuleFor(x => x.ClientSecret).NotEmpty().WithMessage("ClientSecret is required");
            RuleFor(x => x.GrantType).NotEmpty().WithMessage("GrantType is required");
            RuleFor(x => x.GrantType).Must(x => x == "authorization_code" || x == "refresh_token").WithMessage("GrantType must be 'authorization_code' or 'refresh_token'");
            RuleFor(x => x.Code).NotEmpty().When(x => x.GrantType == "authorization_code").WithMessage("Code is required");
            RuleFor(x => x.RefreshToken).NotEmpty().When(x => x.GrantType == "refresh_token").WithMessage("RefreshToken is required");
            RuleFor(x => x.RedirectUri).NotEmpty().When(x => x.GrantType == "authorization_code").WithMessage("RedirectUri is required");
        }
    }
}
