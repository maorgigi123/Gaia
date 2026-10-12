using System.ComponentModel.DataAnnotations;

namespace Gaia.Api.Contracts.Operations;

public sealed class ExecuteOperationRequest
{
    [Required, StringLength(64)]
    public string OperationCode { get; init; } = "";

    [Required(AllowEmptyStrings = true), StringLength(1000)]
    public string? FieldA { get; init; }

    [Required(AllowEmptyStrings = true), StringLength(1000)]
    public string? FieldB { get; init; }
}
