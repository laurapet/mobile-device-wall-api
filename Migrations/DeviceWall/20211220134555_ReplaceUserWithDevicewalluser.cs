using Microsoft.EntityFrameworkCore.Migrations;

namespace device_wall_backend.Migrations.DeviceWall
{
    public partial class ReplaceUserWithDevicewalluser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lending_User_UserID",
                table: "Lending");

            migrationBuilder.AlterColumn<int>(
                name: "UserID",
                table: "Lending",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_Lending_User_UserID",
                table: "Lending",
                column: "UserID",
                principalTable: "User",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lending_User_UserID",
                table: "Lending");

            migrationBuilder.AlterColumn<int>(
                name: "UserID",
                table: "Lending",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Lending_User_UserID",
                table: "Lending",
                column: "UserID",
                principalTable: "User",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
