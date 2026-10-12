using Gaia.Application.Common;

namespace Gaia.Application.Operations;

public sealed class MultiplyOperation : IOperation
{
    public string Code => "multiply";

    public Result<string> Execute(string a, string b) =>
        DecimalCalculation.Execute(a, b, static (left, right) => left * right);
}
