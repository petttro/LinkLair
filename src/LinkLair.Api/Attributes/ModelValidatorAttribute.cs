using System.Linq;
using LinkLair.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LinkLair.Api.Attributes;

public class ModelValidatorAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.ActionArguments.Any(kv => kv.Value == null))
        {
            // Validation of "Required" properties in api call is passed if argument is null.
            // We need this check to prevent passing any null parameter as part of the request.
            context.ModelState.AddModelError("request", "Arguments cannot be null");
            var inputErrors = new SerializableError(context.ModelState);
            throw new BaseInputException(inputErrors);
        }

        if (!context.ModelState.IsValid)
        {
            var inputErrors = new SerializableError(context.ModelState);
            throw new BaseInputException(inputErrors);
        }
    }
}
