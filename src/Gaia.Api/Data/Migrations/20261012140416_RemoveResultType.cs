using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gaia.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveResultType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResultType",
                table: "Executions");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ResultType",
                table: "Executions",
                type: "NVARCHAR2(16)",
                maxLength: 16,
                nullable: false,
                defaultValue: "text");
        }
    }
}
