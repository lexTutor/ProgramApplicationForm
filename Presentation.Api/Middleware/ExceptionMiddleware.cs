

using Application.Core;
using Application.Infrastructure.Logger;
using Application.Infrastructure.Models.Common;
using System.Net;

namespace Presentation.Api.Middleware;

public class ExceptionMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task Invoke(HttpContext context, ILogManager<ExceptionMiddleware> logger)
    {
        try
        {
            await _next(context);
        }
        catch (ArgumentException ex)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            await context.Response.WriteAsJsonAsync(BaseResponse<object>.Fail("Failed", [ex.Message]));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled Exception for Request {Path}", new
            {
                context.Request.Path,
            });

            await HandleExceptionAsync(context);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context)
    {
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        await context.Response.WriteAsJsonAsync(BaseResponse<object>.Fail(Constants.InternalServerErrorMessage, statusCode: HttpStatusCode.InternalServerError));
    }
}

