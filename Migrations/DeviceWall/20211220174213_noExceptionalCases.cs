using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace device_wall_backend.Migrations.DeviceWall
{
    public partial class noExceptionalCases : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           /* migrationBuilder.DropColumn(
                name: "ExceptionalCases",
                table: "Device");*/
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            /*migrationBuilder.AddColumn<List<string>>(
                name: "ExceptionalCases",
                table: "Device",
                type: "text[]",
                nullable: true);*/
        }
    }
}
