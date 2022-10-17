using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PSAPI.Migrations
{
    public partial class TaskType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ProcessTaskTypeId",
                table: "ProcessTaskDefinition",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "ProcessTaskType",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessTaskType", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProcessTaskDefinition_ProcessTaskTypeId",
                table: "ProcessTaskDefinition",
                column: "ProcessTaskTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProcessTaskDefinition_ProcessTaskType_ProcessTaskTypeId",
                table: "ProcessTaskDefinition",
                column: "ProcessTaskTypeId",
                principalTable: "ProcessTaskType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProcessTaskDefinition_ProcessTaskType_ProcessTaskTypeId",
                table: "ProcessTaskDefinition");

            migrationBuilder.DropTable(
                name: "ProcessTaskType");

            migrationBuilder.DropIndex(
                name: "IX_ProcessTaskDefinition_ProcessTaskTypeId",
                table: "ProcessTaskDefinition");

            migrationBuilder.DropColumn(
                name: "ProcessTaskTypeId",
                table: "ProcessTaskDefinition");
        }
    }
}
