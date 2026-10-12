using Gaia.Application.Common;
using Gaia.Application.Operations;

namespace Gaia.Application.Services;

public sealed class OperationService(IEnumerable<IOperation> operations)
{
    private readonly Dictionary<string, IOperation> _operations =
        operations.ToDictionary(operation => operation.Code, StringComparer.OrdinalIgnoreCase);

    public string[] GetAvailableOperations() => _operations.Keys.ToArray();

    public Result<OperationExecution> Execute(string code, string a, string b)
    {
        if (!_operations.TryGetValue(code, out var operation))
            return Result<OperationExecution>.Failure(new(
                "OPERATION_NOT_SUPPORTED", "The requested operation is not supported."));

        var result = operation.Execute(a, b);
        return result.IsSuccess
            ? Result<OperationExecution>.Success(new(operation.Code, result.Value!))
            : Result<OperationExecution>.Failure(result.Error!);
    }
}

public sealed record OperationExecution(string OperationCode, string Result);
