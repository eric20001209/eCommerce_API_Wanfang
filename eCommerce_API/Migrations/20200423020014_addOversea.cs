using Microsoft.EntityFrameworkCore.Migrations;

namespace eCommerce_API.Migrations
{
    public partial class addOversea : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Oversea",
                table: "card",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "product_details",
                columns: table => new
                {
                    code = table.Column<int>(nullable: false),
                    highlight = table.Column<string>(type: "ntext", nullable: true),
                    spec = table.Column<string>(type: "ntext", nullable: true),
                    manufacture = table.Column<string>(unicode: false, maxLength: 1024, nullable: true),
                    pic = table.Column<string>(unicode: false, maxLength: 1024, nullable: true),
                    rev = table.Column<string>(unicode: false, maxLength: 1024, nullable: true),
                    warranty = table.Column<string>(unicode: false, maxLength: 255, nullable: true),
                    details = table.Column<string>(maxLength: 2550, nullable: true),
                    ingredients = table.Column<string>(maxLength: 550, nullable: true),
                    directions = table.Column<string>(maxLength: 550, nullable: true),
                    advice = table.Column<string>(maxLength: 550, nullable: true),
                    shipping = table.Column<string>(maxLength: 550, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product_details", x => x.code);
                });

            migrationBuilder.CreateIndex(
                name: "IDX_product_details_code",
                table: "product_details",
                column: "code",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "product_details");

            migrationBuilder.DropColumn(
                name: "Oversea",
                table: "card");
        }
    }
}
