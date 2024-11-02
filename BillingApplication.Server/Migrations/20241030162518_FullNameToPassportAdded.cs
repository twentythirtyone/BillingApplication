﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BillingApplication.Migrations
{
    /// <inheritdoc />
    public partial class FullNameToPassportAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "PassportInfos",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FullName",
                table: "PassportInfos");
        }
    }
}
