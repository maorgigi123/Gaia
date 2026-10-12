using Gaia.Application.Common;

namespace Gaia.Application.Operations;

public sealed class SubtractOperation : IOperation
{
    public string Code => "subtract";

    public Result<string> Execute(string a, string b) =>
        DecimalCalculation.Execute(a, b, static (left, right) => left - right);
}
