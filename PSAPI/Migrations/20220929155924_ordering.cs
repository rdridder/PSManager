using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PSAPI.Migrations
{
    public partial class ordering : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "ProcessTask",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "ProcessDefinitionTaskDefinition",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Order",
                table: "ProcessTask");

            migrationBuilder.DropColumn(
                name: "Order",
                table: "ProcessDefinitionTaskDefinition");
        }
    }
}
