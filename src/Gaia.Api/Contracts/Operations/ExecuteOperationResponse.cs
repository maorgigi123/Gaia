namespace Gaia.Api.Contracts.Operations;

public sealed record ExecuteOperationResponse(string OperationCode, string Result, ExecutionHistoryDto History);
