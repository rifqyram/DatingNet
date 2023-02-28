using System.Diagnostics.CodeAnalysis;
using System.Net;
using Enigma.DatingNet.Exceptions;
using Enigma.DatingNet.Models.Responses;
using TokonyadiaRestAPI.Exceptions;

namespace Enigma.DatingNet.Middlewares;

public class ExceptionHandlingMiddleware : IMiddleware
{
    private readonly ILogger _logger;

    public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger)
    {
        _logger = logger;
    }

    [SuppressMessage("ReSharper.DPA", "DPA0003: Excessive memory allocations in LOH",
        MessageId = "type: System.Byte[]; size: 74MB")]
    [SuppressMessage("ReSharper.DPA", "DPA0003: Excessive memory allocations in LOH", MessageId = "type: System.Byte[]; size: 78MB")]
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception e)
        {
            await HandlingExceptionAsync(context, e);
            _logger.LogError(e.Message);
        }
    }

    private static async Task HandlingExceptionAsync(HttpContext context, Exception e)
    {
        var error = new ErrorResponse();
        switch (e)
        {
            case NotFoundException:
                error.Code = (int)HttpStatusCode.NotFound;
                error.Status = HttpStatusCode.NotFound.ToString();
                error.Message = e.Message;
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                break;
            case UnauthorizedException:
                error.Code = (int)HttpStatusCode.Unauthorized;
                error.Status = HttpStatusCode.Unauthorized.ToString();
                error.Message = e.Message;
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                break;
            case BadRequestException:
                error.Code = (int)HttpStatusCode.BadRequest;
                error.Status = HttpStatusCode.BadRequest.ToString();
                error.Message = e.Message;
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                break;
            case DuplicateDataException:
                error.Code = (int)HttpStatusCode.Conflict;
                error.Status = HttpStatusCode.Conflict.ToString();
                error.Message = e.Message;
                context.Response.StatusCode = (int)HttpStatusCode.Conflict;
                break;
            case not null:
                break;
        }

        await context.Response.WriteAsJsonAsync(error);
    }
}