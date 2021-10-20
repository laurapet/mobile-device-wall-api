using Microsoft.EntityFrameworkCore.Migrations;

namespace device_wall_backend.Migrations.DeviceWall
{
    public partial class LendingColumnInDevice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Lending_DeviceID",
                table: "Lending");

            migrationBuilder.CreateIndex(
                name: "IX_Lending_DeviceID",
                table: "Lending",
                column: "DeviceID",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Lending_DeviceID",
                table: "Lending");

            migrationBuilder.CreateIndex(
                name: "IX_Lending_DeviceID",
                table: "Lending",
                column: "DeviceID");
        }
    }
}
