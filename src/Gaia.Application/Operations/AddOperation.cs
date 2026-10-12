using Gaia.Application.Common;

namespace Gaia.Application.Operations;

public sealed class AddOperation : IOperation
{
    public string Code => "add";

    public Result<string> Execute(string a, string b) =>
        DecimalCalculation.Execute(a, b, static (left, right) => left + right);
}
