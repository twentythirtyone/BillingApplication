using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BillingApplication.Migrations
{
    /// <inheritdoc />
    public partial class column_rename : Migration
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

            migrationBuilder.RenameColumn(
                name: "salt",
                table: "Subscribers",
                newName: "Salt");

            migrationBuilder.RenameColumn(
                name: "password",
                table: "Subscribers",
                newName: "Password");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "Subscribers",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "balance",
                table: "Subscribers",
                newName: "Balance");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Subscribers",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "tariff_id",
                table: "Subscribers",
                newName: "TariffId");

            migrationBuilder.RenameColumn(
                name: "phone_number",
                table: "Subscribers",
                newName: "Number");

            migrationBuilder.RenameColumn(
                name: "payment_date",
                table: "Subscribers",
                newName: "PaymentDate");

            migrationBuilder.RenameColumn(
                name: "passport_id",
                table: "Subscribers",
                newName: "PassportId");

            migrationBuilder.RenameColumn(
                name: "messages",
                table: "Subscribers",
                newName: "MessagesCount");

            migrationBuilder.RenameColumn(
                name: "internet",
                table: "Subscribers",
                newName: "InternetAmount");

            migrationBuilder.RenameColumn(
                name: "creation_date",
                table: "Subscribers",
                newName: "CreationDate");

            migrationBuilder.RenameColumn(
                name: "call_time",
                table: "Subscribers",
                newName: "CallTime");

            migrationBuilder.RenameIndex(
                name: "IX_Subscribers_tariff_id",
                table: "Subscribers",
                newName: "IX_Subscribers_TariffId");

            migrationBuilder.RenameIndex(
                name: "IX_Subscribers_passport_id",
                table: "Subscribers",
                newName: "IX_Subscribers_PassportId");

            migrationBuilder.AddForeignKey(
                name: "FK_Subscribers_PassportInfos_PassportId",
                table: "Subscribers",
                column: "PassportId",
                principalTable: "PassportInfos",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Subscribers_Tariffs_TariffId",
                table: "Subscribers",
                column: "TariffId",
                principalTable: "Tariffs",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subscribers_PassportInfos_PassportId",
                table: "Subscribers");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscribers_Tariffs_TariffId",
                table: "Subscribers");

            migrationBuilder.RenameColumn(
                name: "Salt",
                table: "Subscribers",
                newName: "salt");

            migrationBuilder.RenameColumn(
                name: "Password",
                table: "Subscribers",
                newName: "password");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Subscribers",
                newName: "email");

            migrationBuilder.RenameColumn(
                name: "Balance",
                table: "Subscribers",
                newName: "balance");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Subscribers",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "TariffId",
                table: "Subscribers",
                newName: "tariff_id");

            migrationBuilder.RenameColumn(
                name: "PaymentDate",
                table: "Subscribers",
                newName: "payment_date");

            migrationBuilder.RenameColumn(
                name: "PassportId",
                table: "Subscribers",
                newName: "passport_id");

            migrationBuilder.RenameColumn(
                name: "Number",
                table: "Subscribers",
                newName: "phone_number");

            migrationBuilder.RenameColumn(
                name: "MessagesCount",
                table: "Subscribers",
                newName: "messages");

            migrationBuilder.RenameColumn(
                name: "InternetAmount",
                table: "Subscribers",
                newName: "internet");

            migrationBuilder.RenameColumn(
                name: "CreationDate",
                table: "Subscribers",
                newName: "creation_date");

            migrationBuilder.RenameColumn(
                name: "CallTime",
                table: "Subscribers",
                newName: "call_time");

            migrationBuilder.RenameIndex(
                name: "IX_Subscribers_TariffId",
                table: "Subscribers",
                newName: "IX_Subscribers_tariff_id");

            migrationBuilder.RenameIndex(
                name: "IX_Subscribers_PassportId",
                table: "Subscribers",
                newName: "IX_Subscribers_passport_id");

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
        }
    }
}
