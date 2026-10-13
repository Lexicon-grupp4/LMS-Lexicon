using Domain.Models.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Net.NetworkInformation;

public static class ExceptionMiddlewareExtensions
{
    public static void ConfigureExceptionHandler(this WebApplication app)
    {
        app.UseExceptionHandler(builder =>
        {
            builder.Run(async context =>
            {
                var contextFeature =
                    context.Features.Get<IExceptionHandlerFeature>();

                if (contextFeature is not null)
                {
                    var problemDetailsFactory =
                        app.Services.GetRequiredService<ProblemDetailsFactory>();

                    var exception = contextFeature.Error;

                    var (statusCode, title, errorCode) = exception switch
                    {
                        CannotDeleteOwnAccountException =>
                            (StatusCodes.Status409Conflict,
                                "Cannot delete user",
                                "CANNOTDELETEOWN_ACCOUNT"),

                        NotFoundException ex =>
                            (StatusCodes.Status404NotFound,
                                ex.Title,
                                "RESOURCENOTFOUND"),

                        UserAlreadyExistsException ex =>
                            (StatusCodes.Status409Conflict,
                                ex.Title,
                                "EMAILALREADYTAKEN"),

                        TokenValidationException =>
                            (StatusCodes.Status401Unauthorized,
                                "Unauthorized",
                                "TOKEN_INVALID"),

                        _ =>
                            (StatusCodes.Status500InternalServerError,
                                "Internal Server Error",
                                (string?)null)
                    };

                    var problemDetails = new ApiProblemDetails
                    {
                        Status = statusCode,
                        Title = title,
                        Detail = exception.Message,
                        Instance = context.Request.Path,
                        Type = "about:blank",
                        ErrorCode = errorCode
                    };

                    context.Response.StatusCode = statusCode;

                    await context.Response.WriteAsJsonAsync(problemDetails);
                }
            });
        });
    }
}