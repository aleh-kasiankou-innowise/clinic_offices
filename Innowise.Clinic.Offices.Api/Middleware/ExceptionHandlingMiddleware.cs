using System.Text;
using System.Text.Json;
using Innowise.Clinic.Offices.Services.Exceptions;

namespace Innowise.Clinic.Offices.Api.Middleware;

public class ExceptionHandlingMiddleware : IMiddleware
{
    private const string DefaultUnhandledErrorMessage =
        "The error occured during this operation. Please, try again later.";

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }

        catch (BlobServiceException e)
        {
            if (e.IsCritical)
            {
                context.Response.StatusCode = 500;
                await WriteExceptionMessageToResponse(e.Message, context);
                return;

            }
            
            context.Response.StatusCode =  200;
            context.Response.Headers.Add("X-Clinic-Warning", e.Message);
            await next(context);
        }

        catch (ApplicationException e)
        {
            context.Response.StatusCode = 400;
            await WriteExceptionMessageToResponse(e.Message, context);
        }

        catch (Exception e)
        {
            context.Response.StatusCode = 500;
            Console.WriteLine(e);
            await WriteExceptionMessageToResponse(DefaultUnhandledErrorMessage, context);
        }
    }

    private async Task WriteExceptionMessageToResponse(string message, HttpContext context)
    {
        context.Response.ContentType = "application/json";
        var jsonMessage = JsonSerializer.Serialize(message);
        await context.Response.WriteAsync(jsonMessage, Encoding.UTF8);
    }
}