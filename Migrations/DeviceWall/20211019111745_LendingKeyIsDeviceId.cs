using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace device_wall_backend.Migrations.DeviceWall
{
    public partial class LendingKeyIsDeviceId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Lending",
                table: "Lending");

            migrationBuilder.DropIndex(
                name: "IX_Lending_DeviceID",
                table: "Lending");

            migrationBuilder.DropColumn(
                name: "LendingID",
                table: "Lending");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Lending",
                table: "Lending",
                column: "DeviceID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Lending",
                table: "Lending");

            migrationBuilder.AddColumn<int>(
                name: "LendingID",
                table: "Lending",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Lending",
                table: "Lending",
                column: "LendingID");

            migrationBuilder.CreateIndex(
                name: "IX_Lending_DeviceID",
                table: "Lending",
                column: "DeviceID",
                unique: true);
        }
    }
}
