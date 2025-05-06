using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Cambiar_Esquema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_dbo",
                schema: "Customer",
                table: "dbo");

            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.RenameTable(
                name: "dbo",
                schema: "Customer",
                newName: "Customer",
                newSchema: "dbo");

            migrationBuilder.RenameIndex(
                name: "IX_dbo_IdCustomer",
                schema: "dbo",
                table: "Customer",
                newName: "IX_Customer_IdCustomer");

            migrationBuilder.RenameIndex(
                name: "IX_dbo_Email",
                schema: "dbo",
                table: "Customer",
                newName: "IX_Customer_Email");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Customer",
                schema: "dbo",
                table: "Customer",
                column: "IdCustomer");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Customer",
                schema: "dbo",
                table: "Customer");

            migrationBuilder.EnsureSchema(
                name: "Customer");

            migrationBuilder.RenameTable(
                name: "Customer",
                schema: "dbo",
                newName: "dbo",
                newSchema: "Customer");

            migrationBuilder.RenameIndex(
                name: "IX_Customer_IdCustomer",
                schema: "Customer",
                table: "dbo",
                newName: "IX_dbo_IdCustomer");

            migrationBuilder.RenameIndex(
                name: "IX_Customer_Email",
                schema: "Customer",
                table: "dbo",
                newName: "IX_dbo_Email");

            migrationBuilder.AddPrimaryKey(
                name: "PK_dbo",
                schema: "Customer",
                table: "dbo",
                column: "IdCustomer");
        }
    }
}
