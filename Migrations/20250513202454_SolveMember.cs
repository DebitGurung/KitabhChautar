using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KitabhChauta.Migrations
{
    /// <inheritdoc />
    public partial class SolveMember : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MembershipStatus",
                table: "Members");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MembershipStatus",
                table: "Members",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
