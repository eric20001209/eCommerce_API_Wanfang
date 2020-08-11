using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eCommerce_API.Migrations
{
    public partial class addshippinginfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ShippingInfo",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    orderId = table.Column<int>(nullable: false),
                    name = table.Column<string>(nullable: true),
                    company = table.Column<string>(nullable: true),
                    addess1 = table.Column<string>(nullable: true),
                    addess2 = table.Column<string>(nullable: true),
                    addess3 = table.Column<string>(nullable: true),
                    city = table.Column<string>(nullable: true),
                    country = table.Column<string>(nullable: true),
                    phone = table.Column<string>(nullable: true),
                    contact = table.Column<string>(nullable: true),
                    note = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShippingInfo", x => x.id);
                    table.ForeignKey(
                        name: "FK_ShippingInfo_orders_orderId",
                        column: x => x.orderId,
                        principalTable: "orders",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShippingInfo_orderId",
                table: "ShippingInfo",
                column: "orderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShippingInfo");
        }
    }
}
