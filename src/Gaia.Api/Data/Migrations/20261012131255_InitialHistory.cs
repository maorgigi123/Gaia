using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gaia.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Executions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    OperationCode = table.Column<string>(type: "NVARCHAR2(64)", maxLength: 64, nullable: false),
                    FieldA = table.Column<string>(type: "NVARCHAR2(1000)", maxLength: 1000, nullable: true),
                    FieldB = table.Column<string>(type: "NVARCHAR2(1000)", maxLength: 1000, nullable: true),
                    ResultType = table.Column<string>(type: "NVARCHAR2(16)", maxLength: 16, nullable: false),
                    ResultValue = table.Column<string>(type: "NCLOB", nullable: true),
                    ExecutedAtUtc = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Executions", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Executions_OperationCode_ExecutedAtUtc_Id",
                table: "Executions",
                columns: new[] { "OperationCode", "ExecutedAtUtc", "Id" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Executions");
        }
    }
}
