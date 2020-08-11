using Microsoft.EntityFrameworkCore.Migrations;

namespace eCommerce_API.Migrations
{
    public partial class freedilevery : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "FreeDelivery",
                table: "code_relations",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FreeDelivery",
                table: "code_relations");
        }
    }
}
