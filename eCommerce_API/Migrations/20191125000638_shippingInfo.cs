using Microsoft.EntityFrameworkCore.Migrations;

namespace eCommerce_API.Migrations
{
    public partial class shippingInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "phone",
                table: "ShippingInfo",
                newName: "sender_phone");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "ShippingInfo",
                newName: "sender_country");

            migrationBuilder.RenameColumn(
                name: "country",
                table: "ShippingInfo",
                newName: "sender_city");

            migrationBuilder.RenameColumn(
                name: "contact",
                table: "ShippingInfo",
                newName: "sender_address");

            migrationBuilder.RenameColumn(
                name: "company",
                table: "ShippingInfo",
                newName: "sender");

            migrationBuilder.RenameColumn(
                name: "city",
                table: "ShippingInfo",
                newName: "receiver_phone");

            migrationBuilder.RenameColumn(
                name: "addess3",
                table: "ShippingInfo",
                newName: "receiver_country");

            migrationBuilder.RenameColumn(
                name: "addess2",
                table: "ShippingInfo",
                newName: "receiver_contact");

            migrationBuilder.RenameColumn(
                name: "addess1",
                table: "ShippingInfo",
                newName: "receiver_company");

            migrationBuilder.AddColumn<string>(
                name: "receiver",
                table: "ShippingInfo",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "receiver_addess1",
                table: "ShippingInfo",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "receiver_addess2",
                table: "ShippingInfo",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "receiver_addess3",
                table: "ShippingInfo",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "receiver_city",
                table: "ShippingInfo",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "receiver",
                table: "ShippingInfo");

            migrationBuilder.DropColumn(
                name: "receiver_addess1",
                table: "ShippingInfo");

            migrationBuilder.DropColumn(
                name: "receiver_addess2",
                table: "ShippingInfo");

            migrationBuilder.DropColumn(
                name: "receiver_addess3",
                table: "ShippingInfo");

            migrationBuilder.DropColumn(
                name: "receiver_city",
                table: "ShippingInfo");

            migrationBuilder.RenameColumn(
                name: "sender_phone",
                table: "ShippingInfo",
                newName: "phone");

            migrationBuilder.RenameColumn(
                name: "sender_country",
                table: "ShippingInfo",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "sender_city",
                table: "ShippingInfo",
                newName: "country");

            migrationBuilder.RenameColumn(
                name: "sender_address",
                table: "ShippingInfo",
                newName: "contact");

            migrationBuilder.RenameColumn(
                name: "sender",
                table: "ShippingInfo",
                newName: "company");

            migrationBuilder.RenameColumn(
                name: "receiver_phone",
                table: "ShippingInfo",
                newName: "city");

            migrationBuilder.RenameColumn(
                name: "receiver_country",
                table: "ShippingInfo",
                newName: "addess3");

            migrationBuilder.RenameColumn(
                name: "receiver_contact",
                table: "ShippingInfo",
                newName: "addess2");

            migrationBuilder.RenameColumn(
                name: "receiver_company",
                table: "ShippingInfo",
                newName: "addess1");
        }
    }
}
