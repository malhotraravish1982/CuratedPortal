using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MasterGenerator.Data.Migrations
{
    public partial class DealDetails_ProjectId_migration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DealDetails_Project_ProjectId",
                table: "DealDetails");

            migrationBuilder.DropIndex(
                name: "IX_DealDetails_ProjectId",
                table: "DealDetails");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_DealDetails_ProjectId",
                table: "DealDetails",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_DealDetails_Project_ProjectId",
                table: "DealDetails",
                column: "ProjectId",
                principalTable: "Project",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
