using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Results;
using StartTemplateNew.Shared.Models.Dto.Responses.ActionResult;

namespace StartTemplateNew.Shared.FluentValidation.Factories
{
    public class FluentValidationResultFactory : IFluentValidationAutoValidationResultFactory
    {
        public IActionResult CreateActionResult(ActionExecutingContext context, ValidationProblemDetails? validationProblemDetails)
        {
            return new BadRequestObjectResult(new BadRequestObjectValidationValue("Validation errors", validationProblemDetails?.Errors ?? new Dictionary<string, string[]>()));
        }
    }
}
