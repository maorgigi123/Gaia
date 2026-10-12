using Microsoft.EntityFrameworkCore;

namespace Gaia.Api.Data;

public sealed class GaiaDbContext(DbContextOptions<GaiaDbContext> options) : DbContext(options)
{
    public DbSet<ExecutionRecord> Executions => Set<ExecutionRecord>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<ExecutionRecord>();
        entity.Property(x => x.OperationCode).HasMaxLength(64);
        // Oracle stores empty VARCHAR strings as NULL; history maps them back to empty text.
        entity.Property(x => x.FieldA).HasMaxLength(1000);
        entity.Property(x => x.FieldB).HasMaxLength(1000);
        entity.Property(x => x.ResultValue).HasColumnType("NCLOB");
        entity.Property(x => x.ExecutedAtUtc).HasConversion(
            value => value, value => DateTime.SpecifyKind(value, DateTimeKind.Utc));
        entity.HasIndex(x => new { x.OperationCode, x.ExecutedAtUtc, x.Id });
    }
}

public sealed class ExecutionRecord
{
    public long Id { get; set; }
    public string OperationCode { get; set; } = "";
    public string? FieldA { get; set; } = "";
    public string? FieldB { get; set; } = "";
    public string? ResultValue { get; set; } = "";
    public DateTime ExecutedAtUtc { get; set; }
}
