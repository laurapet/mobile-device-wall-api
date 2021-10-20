using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace device_wall_backend.Migrations.Dashboard
{
    public partial class DashboardModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Device",
                columns: table => new
                {
                    DeviceID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Features_OperatingSystem = table.Column<string>(type: "text", nullable: true),
                    Features_Version = table.Column<string>(type: "text", nullable: true),
                    Features_Manufacturer = table.Column<string>(type: "text", nullable: true),
                    Features_IsTablet = table.Column<bool>(type: "boolean", nullable: true),
                    Features_HorizontalSize = table.Column<int>(type: "integer", nullable: true),
                    Features_VerticalSize = table.Column<int>(type: "integer", nullable: true),
                    Features_Dpi = table.Column<int>(type: "integer", nullable: true),
                    Features_Port = table.Column<string>(type: "text", nullable: true),
                    Features_HasSIM = table.Column<bool>(type: "boolean", nullable: true),
                    Features_exceptionalCases = table.Column<List<string>>(type: "text[]", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Device", x => x.DeviceID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Device");
        }
    }
}
