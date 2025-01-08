using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BillingApplication.Migrations
{
    /// <inheritdoc />
    public partial class deletion_rules : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OwnerChanges_PassportInfos_LastUserId",
                table: "OwnerChanges");

            migrationBuilder.DropForeignKey(
                name: "FK_OwnerChanges_PassportInfos_NewUserId",
                table: "OwnerChanges");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Subscribers_PhoneId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_TariffChanges_Subscribers_PhoneId",
                table: "TariffChanges");

            migrationBuilder.DropForeignKey(
                name: "FK_TariffChanges_Tariffs_LastTariffId",
                table: "TariffChanges");

            migrationBuilder.DropForeignKey(
                name: "FK_TariffChanges_Tariffs_NewTariffId",
                table: "TariffChanges");

            migrationBuilder.DropForeignKey(
                name: "FK_TopUps_Subscribers_PhoneId",
                table: "TopUps");

            migrationBuilder.AddForeignKey(
                name: "FK_OwnerChanges_PassportInfos_LastUserId",
                table: "OwnerChanges",
                column: "LastUserId",
                principalTable: "PassportInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_OwnerChanges_PassportInfos_NewUserId",
                table: "OwnerChanges",
                column: "NewUserId",
                principalTable: "PassportInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Subscribers_PhoneId",
                table: "Payments",
                column: "PhoneId",
                principalTable: "Subscribers",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_TariffChanges_Subscribers_PhoneId",
                table: "TariffChanges",
                column: "PhoneId",
                principalTable: "Subscribers",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_TariffChanges_Tariffs_LastTariffId",
                table: "TariffChanges",
                column: "LastTariffId",
                principalTable: "Tariffs",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_TariffChanges_Tariffs_NewTariffId",
                table: "TariffChanges",
                column: "NewTariffId",
                principalTable: "Tariffs",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_TopUps_Subscribers_PhoneId",
                table: "TopUps",
                column: "PhoneId",
                principalTable: "Subscribers",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OwnerChanges_PassportInfos_LastUserId",
                table: "OwnerChanges");

            migrationBuilder.DropForeignKey(
                name: "FK_OwnerChanges_PassportInfos_NewUserId",
                table: "OwnerChanges");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Subscribers_PhoneId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_TariffChanges_Subscribers_PhoneId",
                table: "TariffChanges");

            migrationBuilder.DropForeignKey(
                name: "FK_TariffChanges_Tariffs_LastTariffId",
                table: "TariffChanges");

            migrationBuilder.DropForeignKey(
                name: "FK_TariffChanges_Tariffs_NewTariffId",
                table: "TariffChanges");

            migrationBuilder.DropForeignKey(
                name: "FK_TopUps_Subscribers_PhoneId",
                table: "TopUps");

            migrationBuilder.AddForeignKey(
                name: "FK_OwnerChanges_PassportInfos_LastUserId",
                table: "OwnerChanges",
                column: "LastUserId",
                principalTable: "PassportInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OwnerChanges_PassportInfos_NewUserId",
                table: "OwnerChanges",
                column: "NewUserId",
                principalTable: "PassportInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Subscribers_PhoneId",
                table: "Payments",
                column: "PhoneId",
                principalTable: "Subscribers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TariffChanges_Subscribers_PhoneId",
                table: "TariffChanges",
                column: "PhoneId",
                principalTable: "Subscribers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TariffChanges_Tariffs_LastTariffId",
                table: "TariffChanges",
                column: "LastTariffId",
                principalTable: "Tariffs",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TariffChanges_Tariffs_NewTariffId",
                table: "TariffChanges",
                column: "NewTariffId",
                principalTable: "Tariffs",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TopUps_Subscribers_PhoneId",
                table: "TopUps",
                column: "PhoneId",
                principalTable: "Subscribers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
