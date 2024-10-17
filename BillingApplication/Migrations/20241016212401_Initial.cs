using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BillingApplication.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateTable(
                name: "PassportInfos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PassportNumber = table.Column<string>(type: "text", nullable: true),
                    IssueDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ExpiryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IssuedBy = table.Column<string>(type: "text", nullable: true),
                    Registration = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PassportInfos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tariffs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Price = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tariffs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Bundles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Interval = table.Column<TimeSpan>(type: "interval", nullable: false),
                    Messages = table.Column<int>(type: "integer", nullable: false),
                    Internet = table.Column<long>(type: "bigint", nullable: false),
                    TariffEntityId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bundles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bundles_Tariffs_TariffEntityId",
                        column: x => x.TariffEntityId,
                        principalTable: "Tariffs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SubscriberEntity",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    email = table.Column<string>(type: "text", nullable: false),
                    password = table.Column<string>(type: "text", nullable: false),
                    salt = table.Column<string>(type: "text", nullable: false),
                    passport_id = table.Column<int>(type: "integer", nullable: false),
                    tariff_id = table.Column<int>(type: "integer", nullable: false),
                    phone_number = table.Column<string>(type: "text", nullable: false),
                    balance = table.Column<decimal>(type: "numeric", nullable: false),
                    payment_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    call_time = table.Column<TimeSpan>(type: "interval", nullable: false),
                    messages = table.Column<int>(type: "integer", nullable: false),
                    internet = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscriberEntity", x => x.id);
                    table.ForeignKey(
                        name: "FK_SubscriberEntity_PassportInfos_passport_id",
                        column: x => x.passport_id,
                        principalTable: "PassportInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubscriberEntity_Tariffs_tariff_id",
                        column: x => x.tariff_id,
                        principalTable: "Tariffs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Extras",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Package = table.Column<int>(type: "integer", nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Extras", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Extras_Bundles_Package",
                        column: x => x.Package,
                        principalTable: "Bundles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Calls",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FromPhoneId = table.Column<int>(type: "integer", nullable: false),
                    ToPhoneNumber = table.Column<string>(type: "text", nullable: true),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Duration = table.Column<int>(type: "integer", nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Calls", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Calls_SubscriberEntity_FromPhoneId",
                        column: x => x.FromPhoneId,
                        principalTable: "SubscriberEntity",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FromPhoneId = table.Column<int>(type: "integer", nullable: false),
                    ToPhoneNumber = table.Column<string>(type: "text", nullable: true),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    MessageText = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Messages_SubscriberEntity_FromPhoneId",
                        column: x => x.FromPhoneId,
                        principalTable: "SubscriberEntity",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OwnerChanges",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PhoneId = table.Column<int>(type: "integer", nullable: false),
                    LastUserId = table.Column<int>(type: "integer", nullable: false),
                    NewUserId = table.Column<int>(type: "integer", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OwnerChanges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OwnerChanges_PassportInfos_LastUserId",
                        column: x => x.LastUserId,
                        principalTable: "PassportInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OwnerChanges_PassportInfos_NewUserId",
                        column: x => x.NewUserId,
                        principalTable: "PassportInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OwnerChanges_SubscriberEntity_PhoneId",
                        column: x => x.PhoneId,
                        principalTable: "SubscriberEntity",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PhoneId = table.Column<int>(type: "integer", nullable: false),
                    SenderInfo = table.Column<string>(type: "text", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payments_SubscriberEntity_PhoneId",
                        column: x => x.PhoneId,
                        principalTable: "SubscriberEntity",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TariffChanges",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PhoneId = table.Column<int>(type: "integer", nullable: false),
                    LastTariffId = table.Column<int>(type: "integer", nullable: false),
                    NewTariffId = table.Column<int>(type: "integer", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TariffChanges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TariffChanges_SubscriberEntity_PhoneId",
                        column: x => x.PhoneId,
                        principalTable: "SubscriberEntity",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TariffChanges_Tariffs_LastTariffId",
                        column: x => x.LastTariffId,
                        principalTable: "Tariffs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TariffChanges_Tariffs_NewTariffId",
                        column: x => x.NewTariffId,
                        principalTable: "Tariffs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TopUps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PhoneId = table.Column<int>(type: "integer", nullable: false),
                    SenderInfo = table.Column<string>(type: "text", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TopUps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TopUps_SubscriberEntity_PhoneId",
                        column: x => x.PhoneId,
                        principalTable: "SubscriberEntity",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bundles_TariffEntityId",
                table: "Bundles",
                column: "TariffEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Calls_FromPhoneId",
                table: "Calls",
                column: "FromPhoneId");

            migrationBuilder.CreateIndex(
                name: "IX_Extras_Package",
                table: "Extras",
                column: "Package");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_FromPhoneId",
                table: "Messages",
                column: "FromPhoneId");

            migrationBuilder.CreateIndex(
                name: "IX_OwnerChanges_LastUserId",
                table: "OwnerChanges",
                column: "LastUserId");

            migrationBuilder.CreateIndex(
                name: "IX_OwnerChanges_NewUserId",
                table: "OwnerChanges",
                column: "NewUserId");

            migrationBuilder.CreateIndex(
                name: "IX_OwnerChanges_PhoneId",
                table: "OwnerChanges",
                column: "PhoneId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_PhoneId",
                table: "Payments",
                column: "PhoneId");

            migrationBuilder.CreateIndex(
                name: "IX_SubscriberEntity_passport_id",
                table: "SubscriberEntity",
                column: "passport_id");

            migrationBuilder.CreateIndex(
                name: "IX_SubscriberEntity_tariff_id",
                table: "SubscriberEntity",
                column: "tariff_id");

            migrationBuilder.CreateIndex(
                name: "IX_TariffChanges_LastTariffId",
                table: "TariffChanges",
                column: "LastTariffId");

            migrationBuilder.CreateIndex(
                name: "IX_TariffChanges_NewTariffId",
                table: "TariffChanges",
                column: "NewTariffId");

            migrationBuilder.CreateIndex(
                name: "IX_TariffChanges_PhoneId",
                table: "TariffChanges",
                column: "PhoneId");

            migrationBuilder.CreateIndex(
                name: "IX_TopUps_PhoneId",
                table: "TopUps",
                column: "PhoneId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Admins");

            migrationBuilder.DropTable(
                name: "Calls");

            migrationBuilder.DropTable(
                name: "Extras");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "OwnerChanges");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "TariffChanges");

            migrationBuilder.DropTable(
                name: "TopUps");

            migrationBuilder.DropTable(
                name: "Bundles");

            migrationBuilder.DropTable(
                name: "SubscriberEntity");

            migrationBuilder.DropTable(
                name: "PassportInfos");

            migrationBuilder.DropTable(
                name: "Tariffs");
        }
    }
}
