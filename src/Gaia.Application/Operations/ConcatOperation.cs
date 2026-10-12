using Gaia.Application.Common;

namespace Gaia.Application.Operations;

public sealed class ConcatOperation : IOperation
{
    public string Code => "concat";

    public Result<string> Execute(string a, string b) =>
        Result<string>.Success(a + b);
}
