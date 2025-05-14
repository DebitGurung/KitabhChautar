using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KitabhChauta.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderAndOrderItem1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CancellationDate",
                table: "Orders",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCanceled",
                table: "Orders",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CancellationDate",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "IsCanceled",
                table: "Orders");
        }
    }
}
