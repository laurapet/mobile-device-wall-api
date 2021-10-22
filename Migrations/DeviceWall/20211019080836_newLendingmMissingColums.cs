using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace device_wall_backend.Migrations.DeviceWall
{
    public partial class newLendingmMissingColums : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Lending",
                columns: table => new
                {
                    LendingID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserID = table.Column<int>(type: "integer", nullable: false),
                    DeviceID = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lending", x => x.LendingID);
                });
            migrationBuilder.AddColumn<bool>(
                name: "IsLongterm",
                table: "Lending",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Username = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UserID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Lending_DeviceID",
                table: "Lending",
                column: "DeviceID");

            migrationBuilder.CreateIndex(
                name: "IX_Lending_UserID",
                table: "Lending",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Lending_Device_DeviceID",
                table: "Lending",
                column: "DeviceID",
                principalTable: "Device",
                principalColumn: "DeviceID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Lending_User_UserID",
                table: "Lending",
                column: "UserID",
                principalTable: "User",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lending_Device_DeviceID",
                table: "Lending");

            migrationBuilder.DropForeignKey(
                name: "FK_Lending_User_UserID",
                table: "Lending");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropIndex(
                name: "IX_Lending_DeviceID",
                table: "Lending");

            migrationBuilder.DropIndex(
                name: "IX_Lending_UserID",
                table: "Lending");

            migrationBuilder.DropColumn(
                name: "IsLongterm",
                table: "Lending");
        }
    }
}
