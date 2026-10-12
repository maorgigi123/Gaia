using Gaia.Application.Common;

namespace Gaia.Application.Operations;

public sealed class DivideOperation : IOperation
{
    public string Code => "divide";

    public Result<string> Execute(string a, string b) =>
        DecimalCalculation.Execute(a, b, static (left, right) => left / right);
}
