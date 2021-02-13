using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccessLayer.Migrations
{
    public partial class updateDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_Weapons_WeaponId",
                table: "Games");

            migrationBuilder.DropIndex(
                name: "IX_Games_WeaponId",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "posX",
                table: "Player");

            migrationBuilder.DropColumn(
                name: "posY",
                table: "Player");

            migrationBuilder.DropColumn(
                name: "posX",
                table: "Monsters");

            migrationBuilder.DropColumn(
                name: "posY",
                table: "Monsters");

            migrationBuilder.DropColumn(
                name: "WeaponId",
                table: "Games");

            migrationBuilder.AddColumn<int>(
                name: "GameId",
                table: "Weapons",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GameId1",
                table: "Objects",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "name",
                table: "Games",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Weapons_GameId",
                table: "Weapons",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_Objects_GameId1",
                table: "Objects",
                column: "GameId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Objects_Games_GameId1",
                table: "Objects",
                column: "GameId1",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Weapons_Games_GameId",
                table: "Weapons",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Objects_Games_GameId1",
                table: "Objects");

            migrationBuilder.DropForeignKey(
                name: "FK_Weapons_Games_GameId",
                table: "Weapons");

            migrationBuilder.DropIndex(
                name: "IX_Weapons_GameId",
                table: "Weapons");

            migrationBuilder.DropIndex(
                name: "IX_Objects_GameId1",
                table: "Objects");

            migrationBuilder.DropColumn(
                name: "GameId",
                table: "Weapons");

            migrationBuilder.DropColumn(
                name: "GameId1",
                table: "Objects");

            migrationBuilder.DropColumn(
                name: "name",
                table: "Games");

            migrationBuilder.AddColumn<int>(
                name: "posX",
                table: "Player",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "posY",
                table: "Player",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "posX",
                table: "Monsters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "posY",
                table: "Monsters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WeaponId",
                table: "Games",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Games_WeaponId",
                table: "Games",
                column: "WeaponId");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Weapons_WeaponId",
                table: "Games",
                column: "WeaponId",
                principalTable: "Weapons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
