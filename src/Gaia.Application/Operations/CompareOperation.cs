using Gaia.Application.Common;

namespace Gaia.Application.Operations;

public sealed class CompareOperation : IOperation
{
    public string Code => "compare";

    public Result<string> Execute(string a, string b) =>
        Result<string>.Success(string.Equals(a, b, StringComparison.OrdinalIgnoreCase) ? "true" : "false");
}
