using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BillingApplication.Migrations
{
    /// <inheritdoc />
    public partial class ModelsFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Interval",
                table: "Bundles",
                newName: "CallTIme");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CallTIme",
                table: "Bundles",
                newName: "Interval");
        }
    }
}
