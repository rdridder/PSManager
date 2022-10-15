using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PSAPI.Migrations
{
    public partial class status : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "StatusId",
                table: "ProcessTask",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "StatusId",
                table: "Processes",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "Status",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Status", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProcessTask_StatusId",
                table: "ProcessTask",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Processes_StatusId",
                table: "Processes",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Processes_Status_StatusId",
                table: "Processes",
                column: "StatusId",
                principalTable: "Status",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProcessTask_Status_StatusId",
                table: "ProcessTask",
                column: "StatusId",
                principalTable: "Status",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Processes_Status_StatusId",
                table: "Processes");

            migrationBuilder.DropForeignKey(
                name: "FK_ProcessTask_Status_StatusId",
                table: "ProcessTask");

            migrationBuilder.DropTable(
                name: "Status");

            migrationBuilder.DropIndex(
                name: "IX_ProcessTask_StatusId",
                table: "ProcessTask");

            migrationBuilder.DropIndex(
                name: "IX_Processes_StatusId",
                table: "Processes");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "ProcessTask");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "Processes");
        }
    }
}
