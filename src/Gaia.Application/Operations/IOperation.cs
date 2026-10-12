using Gaia.Application.Common;

namespace Gaia.Application.Operations;

public interface IOperation
{
    string Code { get; }

    Result<string> Execute(string a, string b);
}
