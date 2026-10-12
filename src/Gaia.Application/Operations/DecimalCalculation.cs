using System.Globalization;
using Gaia.Application.Common;

namespace Gaia.Application.Operations;

public static class DecimalCalculation
{
    public static Result<string> Execute(string a, string b, Func<decimal, decimal, decimal> calculate)
    {
        const NumberStyles styles = NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint |
            NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite;
        if (!decimal.TryParse(a, styles, CultureInfo.InvariantCulture, out var left) ||
            !decimal.TryParse(b, styles, CultureInfo.InvariantCulture, out var right))
            return Result<string>.Failure(new("INVALID_NUMBER", "Both fields must be decimal numbers using a dot, within the decimal range."));

        try
        {
            return Result<string>.Success(calculate(left, right).ToString("G29", CultureInfo.InvariantCulture));
        }
        catch (DivideByZeroException)
        {
            return Result<string>.Failure(new("DIVISION_BY_ZERO", "Field B must not be zero for division."));
        }
        catch (OverflowException)
        {
            return Result<string>.Failure(new("NUMBER_OUT_OF_RANGE", "The result exceeds the supported decimal range."));
        }
    }
}
