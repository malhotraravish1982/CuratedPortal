using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MasterGenerator.Data.Migrations
{
    public partial class add_FieldPermission_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FieldPermissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    CustomerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProjectName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PONumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PODate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalQuantity = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    POAmount = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CSStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DisplayStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShipmentMethod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductionCompletion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VesselETA = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VesselETD = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EstimatedDeliveryDate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ActualDeliveryDate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PreProductionManager = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OnTimeStatus = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FieldPermissions", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FieldPermissions");
        }
    }
}
