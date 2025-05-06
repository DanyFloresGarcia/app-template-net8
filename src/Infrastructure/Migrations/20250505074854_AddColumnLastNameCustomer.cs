using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnLastNameCustomer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_dbo_Name",
                schema: "Customer",
                table: "dbo");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                schema: "Customer",
                table: "dbo",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastName",
                schema: "Customer",
                table: "dbo");

            migrationBuilder.CreateIndex(
                name: "IX_dbo_Name",
                schema: "Customer",
                table: "dbo",
                column: "Name",
                unique: true);
        }
    }
}
