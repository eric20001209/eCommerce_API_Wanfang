using Microsoft.EntityFrameworkCore.Migrations;

namespace eCommerce_API.Migrations
{
    public partial class addweborderstatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "web_order_status",
                table: "orders",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "web_order_status",
                table: "orders");
        }
    }
}
