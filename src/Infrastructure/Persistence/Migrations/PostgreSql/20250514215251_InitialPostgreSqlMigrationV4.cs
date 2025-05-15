using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations.PostgreSql
{
    /// <inheritdoc />
    public partial class InitialPostgreSqlMigrationV4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                type: "character varying(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(150)",
                oldMaxLength: 150);
        }
    }
}
