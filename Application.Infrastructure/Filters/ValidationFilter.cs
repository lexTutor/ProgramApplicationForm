using Application.Core;
using Application.Infrastructure.Logger;
using Application.Infrastructure.Models.Common;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Infrastructure.Filters;

public class ValidationFilter<T>(IServiceProvider serviceProvider, ILogManager<ValidationFilter<T>> logger) : IAsyncActionFilter where T : class
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (context.ActionArguments.SingleOrDefault(p => p.Value is T).Value is T typedArgument)
        {
            try
            {
                var validator = _serviceProvider.GetService<IValidator<T>>();
                if (validator == null)
                {
                    return;
                }

                var result = await validator.ValidateAsync(typedArgument);
                if (!result.IsValid)
                {
                    var errors = result.Errors.Select(e => e.ErrorMessage).ToArray();
                    context.Result = new BadRequestObjectResult(BaseResponse<T>.Fail(Constants.ValidationError, errors));
                    return;
                }

                await next();
            }
            catch (Exception ex)
            {
                logger.LogError(ex);
                context.Result = new BadRequestObjectResult(BaseResponse<T>.Fail("Error processing request"));
            }
        }
        else
        {
            logger.LogWarning("Invalid request arguments", context.ActionArguments);
            context.Result = new BadRequestObjectResult(BaseResponse<T>.Fail($"Invalid request body"));
        }
    }
}

