using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DotnetApiGuideline.Migrations
{
    /// <inheritdoc />
    public partial class AddedIsActiveFieldToBaseEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_active",
                table: "orders",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_active",
                table: "order_items",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_active",
                table: "customers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_active",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "is_active",
                table: "order_items");

            migrationBuilder.DropColumn(
                name: "is_active",
                table: "customers");
        }
    }
}
