using Gaia.Api.Contracts.Common;
using Microsoft.AspNetCore.Diagnostics;
using Oracle.ManagedDataAccess.Client;
using Microsoft.EntityFrameworkCore;

namespace Gaia.Api.ExceptionHandling;

public sealed class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
    {
        logger.LogError(exception, "Unhandled server error. Trace: {TraceId}", context.TraceIdentifier);
        var databaseFailure = exception is DbUpdateException or OracleException;
        context.Response.StatusCode = databaseFailure ? StatusCodes.Status503ServiceUnavailable : StatusCodes.Status500InternalServerError;
        await context.Response.WriteAsJsonAsync(
            ApiResponse<object>.Fail(databaseFailure ? "DATABASE_UNAVAILABLE" : "INTERNAL_SERVER_ERROR",
                databaseFailure ? "Execution history is unavailable. Please try again later." : "An unexpected error occurred."), cancellationToken);
        return true;
    }
}
