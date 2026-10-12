namespace Gaia.Api.Contracts.Operations;

public sealed record ExecutionHistoryDto(
    IReadOnlyList<HistoryEntryDto> Items,
    int Page,
    int PageSize,
    int TotalCount,
    int TotalPages,
    int CurrentMonthCount,
    DateTime MonthStartUtc);
public sealed record HistoryEntryDto(long Id, string FieldA, string FieldB, string Result, DateTime ExecutedAtUtc);
