using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace device_wall_backend.Migrations.DeviceWall
{
    public partial class ExceptionalCasesColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<List<string>>(
                name: "ExceptionalCases",
                table: "Device",
                type: "text[]",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExceptionalCases",
                table: "Device");
        }
    }
}
