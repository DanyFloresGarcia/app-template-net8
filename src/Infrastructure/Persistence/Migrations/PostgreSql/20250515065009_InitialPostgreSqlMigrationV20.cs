using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations.PostgreSql
{
    /// <inheritdoc />
    public partial class InitialPostgreSqlMigrationV20 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.RenameTable(
                name: "Customer",
                schema: "mae",
                newName: "Customer",
                newSchema: "dbo");

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                schema: "dbo",
                table: "Customer",
                type: "character varying(120)",
                maxLength: 120,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "mae");

            migrationBuilder.RenameTable(
                name: "Customer",
                schema: "dbo",
                newName: "Customer",
                newSchema: "mae");

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                schema: "mae",
                table: "Customer",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(120)",
                oldMaxLength: 120);
        }
    }
}
