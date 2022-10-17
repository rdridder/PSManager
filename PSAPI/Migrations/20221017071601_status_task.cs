using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PSAPI.Migrations
{
    public partial class status_task : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ProcessTaskTypeId",
                table: "ProcessTask",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_ProcessTask_ProcessTaskTypeId",
                table: "ProcessTask",
                column: "ProcessTaskTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProcessTask_ProcessTaskType_ProcessTaskTypeId",
                table: "ProcessTask",
                column: "ProcessTaskTypeId",
                principalTable: "ProcessTaskType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProcessTask_ProcessTaskType_ProcessTaskTypeId",
                table: "ProcessTask");

            migrationBuilder.DropIndex(
                name: "IX_ProcessTask_ProcessTaskTypeId",
                table: "ProcessTask");

            migrationBuilder.DropColumn(
                name: "ProcessTaskTypeId",
                table: "ProcessTask");
        }
    }
}
