using Gaia.Api.Contracts.Operations;
using Gaia.Api.Data;
using Gaia.Application.Services;
using Microsoft.EntityFrameworkCore;

namespace Gaia.Api.Services;

public sealed class ExecutionHistoryService(GaiaDbContext db)
{
    public const int DefaultPageSize = 3;

    public async Task<ExecutionHistoryDto> RecordAsync(OperationExecution execution, string a, string b, CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;
        await using var transaction = await db.Database.BeginTransactionAsync(cancellationToken);
        db.Executions.Add(new ExecutionRecord
        {
            OperationCode = execution.OperationCode,
            FieldA = a,
            FieldB = b,
            ResultValue = execution.Result,
            ExecutedAtUtc = now
        });
        await db.SaveChangesAsync(cancellationToken);
        var history = await ReadAsync(execution.OperationCode, now, 1, DefaultPageSize, cancellationToken);
        await transaction.CommitAsync(cancellationToken);
        return history;
    }

    public Task<ExecutionHistoryDto> GetAsync(string code, int page, int pageSize, CancellationToken cancellationToken) =>
        ReadAsync(code, DateTime.UtcNow, page, pageSize, cancellationToken);

    private async Task<ExecutionHistoryDto> ReadAsync(string code, DateTime now, int page, int pageSize, CancellationToken cancellationToken)
    {
        var monthStart = new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc);
        var nextMonth = monthStart.AddMonths(1);
        var query = db.Executions.AsNoTracking().Where(x => x.OperationCode == code);
        var totalCount = await query.CountAsync(cancellationToken);
        var count = await query.CountAsync(x => x.ExecutedAtUtc >= monthStart && x.ExecutedAtUtc < nextMonth, cancellationToken);
        var items = await query.OrderByDescending(x => x.ExecutedAtUtc).ThenByDescending(x => x.Id)
            .Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);
        var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
        return new(items.Select(x => new HistoryEntryDto(x.Id, x.FieldA ?? "", x.FieldB ?? "",
            x.ResultValue ?? "", x.ExecutedAtUtc)).ToArray(), page, pageSize, totalCount, totalPages, count, monthStart);
    }
}
