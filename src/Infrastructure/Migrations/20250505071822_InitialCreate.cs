using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Customer");

            migrationBuilder.CreateTable(
                name: "dbo",
                schema: "Customer",
                columns: table => new
                {
                    IdCustomer = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Asset = table.Column<bool>(type: "bit", nullable: false),
                    UserCreator = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    UserUpdater = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    HostCreator = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    HostUpdater = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateUpdate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AppCreator = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AppUpdater = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dbo", x => x.IdCustomer);
                });

            migrationBuilder.CreateIndex(
                name: "IX_dbo_Email",
                schema: "Customer",
                table: "dbo",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_dbo_IdCustomer",
                schema: "Customer",
                table: "dbo",
                column: "IdCustomer");

            migrationBuilder.CreateIndex(
                name: "IX_dbo_Name",
                schema: "Customer",
                table: "dbo",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "dbo",
                schema: "Customer");
        }
    }
}
