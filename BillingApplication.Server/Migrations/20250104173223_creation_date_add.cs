using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BillingApplication.Migrations
{
    /// <inheritdoc />
    public partial class creation_date_add : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subscribers_PassportInfos_passport_id",
                table: "Subscribers");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscribers_Tariffs_tariff_id",
                table: "Subscribers");

            migrationBuilder.DropForeignKey(
                name: "FK_TariffChanges_Tariffs_LastTariffId",
                table: "TariffChanges");

            migrationBuilder.DropForeignKey(
                name: "FK_TariffChanges_Tariffs_NewTariffId",
                table: "TariffChanges");

            migrationBuilder.AlterColumn<int>(
                name: "NewTariffId",
                table: "TariffChanges",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "LastTariffId",
                table: "TariffChanges",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "tariff_id",
                table: "Subscribers",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "passport_id",
                table: "Subscribers",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<DateTime>(
                name: "creation_date",
                table: "Subscribers",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddForeignKey(
                name: "FK_Subscribers_PassportInfos_passport_id",
                table: "Subscribers",
                column: "passport_id",
                principalTable: "PassportInfos",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Subscribers_Tariffs_tariff_id",
                table: "Subscribers",
                column: "tariff_id",
                principalTable: "Tariffs",
                principalColumn: "Id");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subscribers_PassportInfos_passport_id",
                table: "Subscribers");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscribers_Tariffs_tariff_id",
                table: "Subscribers");

            migrationBuilder.DropForeignKey(
                name: "FK_TariffChanges_Tariffs_LastTariffId",
                table: "TariffChanges");

            migrationBuilder.DropForeignKey(
                name: "FK_TariffChanges_Tariffs_NewTariffId",
                table: "TariffChanges");

            migrationBuilder.DropColumn(
                name: "creation_date",
                table: "Subscribers");

            migrationBuilder.AlterColumn<int>(
                name: "NewTariffId",
                table: "TariffChanges",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "LastTariffId",
                table: "TariffChanges",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "tariff_id",
                table: "Subscribers",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "passport_id",
                table: "Subscribers",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Subscribers_PassportInfos_passport_id",
                table: "Subscribers",
                column: "passport_id",
                principalTable: "PassportInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Subscribers_Tariffs_tariff_id",
                table: "Subscribers",
                column: "tariff_id",
                principalTable: "Tariffs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TariffChanges_Tariffs_LastTariffId",
                table: "TariffChanges",
                column: "LastTariffId",
                principalTable: "Tariffs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TariffChanges_Tariffs_NewTariffId",
                table: "TariffChanges",
                column: "NewTariffId",
                principalTable: "Tariffs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
