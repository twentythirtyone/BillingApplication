using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BillingApplication.Migrations
{
    /// <inheritdoc />
    public partial class AdminChangedToOperator : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Calls_SubscriberEntity_FromPhoneId",
                table: "Calls");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_SubscriberEntity_FromPhoneId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_OwnerChanges_SubscriberEntity_PhoneId",
                table: "OwnerChanges");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_SubscriberEntity_PhoneId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_SubscriberEntity_PassportInfos_passport_id",
                table: "SubscriberEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_SubscriberEntity_Tariffs_tariff_id",
                table: "SubscriberEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_TariffChanges_SubscriberEntity_PhoneId",
                table: "TariffChanges");

            migrationBuilder.DropForeignKey(
                name: "FK_TopUps_SubscriberEntity_PhoneId",
                table: "TopUps");

            migrationBuilder.DropTable(
                name: "Admins");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SubscriberEntity",
                table: "SubscriberEntity");

            migrationBuilder.RenameTable(
                name: "SubscriberEntity",
                newName: "Subscribers");

            migrationBuilder.RenameColumn(
                name: "FromPhoneId",
                table: "Calls",
                newName: "FromSubscriberId");

            migrationBuilder.RenameIndex(
                name: "IX_Calls_FromPhoneId",
                table: "Calls",
                newName: "IX_Calls_FromSubscriberId");

            migrationBuilder.RenameIndex(
                name: "IX_SubscriberEntity_tariff_id",
                table: "Subscribers",
                newName: "IX_Subscribers_tariff_id");

            migrationBuilder.RenameIndex(
                name: "IX_SubscriberEntity_passport_id",
                table: "Subscribers",
                newName: "IX_Subscribers_passport_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Subscribers",
                table: "Subscribers",
                column: "id");

            migrationBuilder.CreateTable(
                name: "Operators",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Nickname = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    IsAdmin = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operators", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Calls_Subscribers_FromSubscriberId",
                table: "Calls",
                column: "FromSubscriberId",
                principalTable: "Subscribers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Subscribers_FromPhoneId",
                table: "Messages",
                column: "FromPhoneId",
                principalTable: "Subscribers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OwnerChanges_Subscribers_PhoneId",
                table: "OwnerChanges",
                column: "PhoneId",
                principalTable: "Subscribers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Subscribers_PhoneId",
                table: "Payments",
                column: "PhoneId",
                principalTable: "Subscribers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

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
                name: "FK_TariffChanges_Subscribers_PhoneId",
                table: "TariffChanges",
                column: "PhoneId",
                principalTable: "Subscribers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TopUps_Subscribers_PhoneId",
                table: "TopUps",
                column: "PhoneId",
                principalTable: "Subscribers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Calls_Subscribers_FromSubscriberId",
                table: "Calls");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Subscribers_FromPhoneId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_OwnerChanges_Subscribers_PhoneId",
                table: "OwnerChanges");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Subscribers_PhoneId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscribers_PassportInfos_passport_id",
                table: "Subscribers");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscribers_Tariffs_tariff_id",
                table: "Subscribers");

            migrationBuilder.DropForeignKey(
                name: "FK_TariffChanges_Subscribers_PhoneId",
                table: "TariffChanges");

            migrationBuilder.DropForeignKey(
                name: "FK_TopUps_Subscribers_PhoneId",
                table: "TopUps");

            migrationBuilder.DropTable(
                name: "Operators");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Subscribers",
                table: "Subscribers");

            migrationBuilder.RenameTable(
                name: "Subscribers",
                newName: "SubscriberEntity");

            migrationBuilder.RenameColumn(
                name: "FromSubscriberId",
                table: "Calls",
                newName: "FromPhoneId");

            migrationBuilder.RenameIndex(
                name: "IX_Calls_FromSubscriberId",
                table: "Calls",
                newName: "IX_Calls_FromPhoneId");

            migrationBuilder.RenameIndex(
                name: "IX_Subscribers_tariff_id",
                table: "SubscriberEntity",
                newName: "IX_SubscriberEntity_tariff_id");

            migrationBuilder.RenameIndex(
                name: "IX_Subscribers_passport_id",
                table: "SubscriberEntity",
                newName: "IX_SubscriberEntity_passport_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SubscriberEntity",
                table: "SubscriberEntity",
                column: "id");

            migrationBuilder.CreateTable(
                name: "Admins",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Nickname = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admins", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Calls_SubscriberEntity_FromPhoneId",
                table: "Calls",
                column: "FromPhoneId",
                principalTable: "SubscriberEntity",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_SubscriberEntity_FromPhoneId",
                table: "Messages",
                column: "FromPhoneId",
                principalTable: "SubscriberEntity",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OwnerChanges_SubscriberEntity_PhoneId",
                table: "OwnerChanges",
                column: "PhoneId",
                principalTable: "SubscriberEntity",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_SubscriberEntity_PhoneId",
                table: "Payments",
                column: "PhoneId",
                principalTable: "SubscriberEntity",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SubscriberEntity_PassportInfos_passport_id",
                table: "SubscriberEntity",
                column: "passport_id",
                principalTable: "PassportInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SubscriberEntity_Tariffs_tariff_id",
                table: "SubscriberEntity",
                column: "tariff_id",
                principalTable: "Tariffs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TariffChanges_SubscriberEntity_PhoneId",
                table: "TariffChanges",
                column: "PhoneId",
                principalTable: "SubscriberEntity",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TopUps_SubscriberEntity_PhoneId",
                table: "TopUps",
                column: "PhoneId",
                principalTable: "SubscriberEntity",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
