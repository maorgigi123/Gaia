using Gaia.Api.Contracts.Common;
using Gaia.Api.Contracts.Operations;
using Gaia.Api.Services;
using Gaia.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Gaia.Api.Controllers;

[ApiController]
[Route("api/v1/operations")]
public sealed class OperationsController(OperationService operations, ExecutionHistoryService history) : ControllerBase
{
    [HttpGet]
    public ActionResult<ApiResponse<string[]>> GetOperations() =>
        Ok(ApiResponse<string[]>.Ok(operations.GetAvailableOperations()));

    [HttpPost("execute")]
    public async Task<ActionResult<ApiResponse<ExecuteOperationResponse>>> Execute(
        ExecuteOperationRequest request, CancellationToken cancellationToken)
    {
        var result = operations.Execute(request.OperationCode, request.FieldA!, request.FieldB!);
        if (!result.IsSuccess)
            return BadRequest(ApiResponse<ExecuteOperationResponse>.Fail(result.Error!.Code, result.Error.Message));
        var execution = result.Value!;
        var summary = await history.RecordAsync(execution, request.FieldA!, request.FieldB!, cancellationToken);
        return Ok(ApiResponse<ExecuteOperationResponse>.Ok(new(execution.OperationCode, execution.Result, summary)));
    }

    [HttpGet("{code}/history")]
    public async Task<ActionResult<ApiResponse<ExecutionHistoryDto>>> GetHistory(
        string code, CancellationToken cancellationToken,
        [FromQuery] int page = 1, [FromQuery] int pageSize = ExecutionHistoryService.DefaultPageSize)
    {
        if (page < 1 || pageSize < 1 || pageSize > 50 || (long)(page - 1) * pageSize > int.MaxValue)
            return BadRequest(ApiResponse<ExecutionHistoryDto>.Fail("VALIDATION_ERROR",
                "Page must be positive, pageSize must be between 1 and 50, and the requested offset must not exceed 2147483647."));

        var operation = operations.GetAvailableOperations().FirstOrDefault(o => o.Equals(code, StringComparison.OrdinalIgnoreCase));
        if (operation is null)
            return NotFound(ApiResponse<ExecutionHistoryDto>.Fail("OPERATION_NOT_SUPPORTED", "The requested operation is not supported."));
        return Ok(ApiResponse<ExecutionHistoryDto>.Ok(await history.GetAsync(operation, page, pageSize, cancellationToken)));
    }
}
