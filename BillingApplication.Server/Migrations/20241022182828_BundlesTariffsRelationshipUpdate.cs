using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BillingApplication.Migrations
{
    /// <inheritdoc />
    public partial class BundlesTariffsRelationshipUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bundles_Tariffs_TariffEntityId",
                table: "Bundles");

            migrationBuilder.DropColumn(
                name: "TariffEntityId",
                table: "Bundles");

            migrationBuilder.AddColumn<int>(
                name: "TariffPlan",
                table: "Tariffs",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Tariffs_TariffPlan",
                table: "Tariffs",
                column: "TariffPlan");

            migrationBuilder.AddForeignKey(
                name: "FK_Tariffs_Bundles_TariffPlan",
                table: "Tariffs",
                column: "TariffPlan",
                principalTable: "Bundles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tariffs_Bundles_TariffPlan",
                table: "Tariffs");

            migrationBuilder.DropIndex(
                name: "IX_Tariffs_TariffPlan",
                table: "Tariffs");

            migrationBuilder.DropColumn(
                name: "TariffPlan",
                table: "Tariffs");

            migrationBuilder.AddColumn<int>(
                name: "TariffEntityId",
                table: "Bundles",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bundles_TariffEntityId",
                table: "Bundles",
                column: "TariffEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bundles_Tariffs_TariffEntityId",
                table: "Bundles",
                column: "TariffEntityId",
                principalTable: "Tariffs",
                principalColumn: "Id");
        }
    }
}
