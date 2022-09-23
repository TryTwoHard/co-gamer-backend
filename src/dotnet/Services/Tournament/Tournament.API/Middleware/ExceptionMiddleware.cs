using System.Net;
using System.Text.Json;
using Infrastructure.Common.Models;
using Tournament.API.Exceptions;

namespace Tournament.API.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, ex);        
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        
        if (exception is Exception exception1)
        {
            context.Response.StatusCode = (int) HttpStatusCode.NotFound;
            var result = JsonSerializer.Serialize(new
            {
                context.Response.StatusCode,
                exception1.Message
            });
            await context.Response.WriteAsync(result);
        }

        if (exception is TournamentNotFoundException tournamentNotFoundException)
        {
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            await context.Response.WriteAsync(new ErrorDetails()
            {
                StatusCode = context.Response.StatusCode,
                Message = tournamentNotFoundException.Message
            }.ToString());
        }

        else {
            context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
            await context.Response.WriteAsync(new ErrorDetails()
            {
                StatusCode = context.Response.StatusCode,
                Message = exception.Message
            }.ToString());
        }
    }
}