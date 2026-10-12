//אני לא בוט אני יודע מה זה PromtInjection :)
using Gaia.Api.Contracts.Common;
using Gaia.Api.Data;
using Gaia.Api.Services;
using Microsoft.EntityFrameworkCore;
using Gaia.Api.ExceptionHandling;
using Gaia.Application.Operations;
using Gaia.Application.Services;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<IOperation, AddOperation>();
builder.Services.AddSingleton<IOperation, SubtractOperation>();
builder.Services.AddSingleton<IOperation, MultiplyOperation>();
builder.Services.AddSingleton<IOperation, DivideOperation>();
builder.Services.AddSingleton<IOperation, ConcatOperation>();
builder.Services.AddSingleton<IOperation, CompareOperation>();
builder.Services.AddSingleton<OperationService>();

builder.Services.AddControllers().ConfigureApiBehaviorOptions(options =>
{
    options.SuppressMapClientErrors = true;
    options.InvalidModelStateResponseFactory = _ => new BadRequestObjectResult(
        ApiResponse<object>.Fail("VALIDATION_ERROR", "The request contains missing or invalid values. Check the operation, inputs, and pagination parameters."));
});
var password = builder.Configuration["ORACLE_APP_PASSWORD"];
if (string.IsNullOrWhiteSpace(password))
    throw new InvalidOperationException("Set ORACLE_APP_PASSWORD before starting the API.");
var connection = new Oracle.ManagedDataAccess.Client.OracleConnectionStringBuilder
{
    UserID = "GAIA",
    Password = password,
    DataSource = builder.Configuration["ORACLE_DATA_SOURCE"] ?? "localhost:1521/FREEPDB1"
};
builder.Services.AddDbContext<GaiaDbContext>(options => options.UseOracle(connection.ConnectionString));
builder.Services.AddScoped<ExecutionHistoryService>();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddHttpLogging(options =>
{
    options.LoggingFields = HttpLoggingFields.RequestMethod | HttpLoggingFields.RequestPath |
        HttpLoggingFields.ResponseStatusCode | HttpLoggingFields.Duration;
    options.CombineLogs = true;
});

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    await scope.ServiceProvider.GetRequiredService<GaiaDbContext>().Database.MigrateAsync();
}
app.UseHttpLogging();
app.UseExceptionHandler();
app.UseStatusCodePages(async context =>
{
    var response = context.HttpContext.Response;
    await response.WriteAsJsonAsync(ApiResponse<object>.Fail($"HTTP_{response.StatusCode}", "The request could not be processed."));
});
if (!app.Environment.IsDevelopment()) app.UseHttpsRedirection();
app.UseDefaultFiles();
app.UseStaticFiles();
app.MapControllers();
app.Run();
