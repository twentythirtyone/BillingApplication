using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BillingApplication.Migrations
{
    /// <inheritdoc />
    public partial class InternetTableRemoveAddColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MessageText",
                table: "Internet");

            migrationBuilder.AddColumn<int>(
                name: "Price",
                table: "Internet",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "Internet");

            migrationBuilder.AddColumn<string>(
                name: "MessageText",
                table: "Internet",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
