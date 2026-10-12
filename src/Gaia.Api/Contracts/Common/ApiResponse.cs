using Gaia.Application.Common;

namespace Gaia.Api.Contracts.Common;

public sealed record ApiResponse<T>(
    bool Success,
    T? Data,
    Error? Error
)
{
    public static ApiResponse<T> Ok(T data) =>
        new(true, data, null);

    public static ApiResponse<T> Fail(
        string code,
        string message) =>
        new(false, default, new Error(code, message));
}
