using Microsoft.EntityFrameworkCore.Migrations;

namespace eCommerce_API.Migrations
{
    public partial class updateaddress : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "receiver_addess3",
                table: "ShippingInfo",
                newName: "receiver_address3");

            migrationBuilder.RenameColumn(
                name: "receiver_addess2",
                table: "ShippingInfo",
                newName: "receiver_address2");

            migrationBuilder.RenameColumn(
                name: "receiver_addess1",
                table: "ShippingInfo",
                newName: "receiver_address1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "receiver_address3",
                table: "ShippingInfo",
                newName: "receiver_addess3");

            migrationBuilder.RenameColumn(
                name: "receiver_address2",
                table: "ShippingInfo",
                newName: "receiver_addess2");

            migrationBuilder.RenameColumn(
                name: "receiver_address1",
                table: "ShippingInfo",
                newName: "receiver_addess1");
        }
    }
}
