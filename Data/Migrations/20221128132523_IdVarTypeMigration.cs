using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace razorsecretsanta.Data.Migrations
{
    /// <inheritdoc />
    public partial class IdVarTypeMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GiftModels",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GiftModels", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "SettingModels",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    value = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SettingModels", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "UserModels",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    group = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserModels", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "UserDetails",
                columns: table => new
                {
                    userID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    friendID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    giftID = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDetails", x => x.userID);
                    table.ForeignKey(
                        name: "FK_UserDetails_GiftModels_giftID",
                        column: x => x.giftID,
                        principalTable: "GiftModels",
                        principalColumn: "id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_UserDetails_UserModels_friendID",
                        column: x => x.friendID,
                        principalTable: "UserModels",
                        principalColumn: "id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_UserDetails_UserModels_userID",
                        column: x => x.userID,
                        principalTable: "UserModels",
                        principalColumn: "id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserDetails_userID",
                table: "UserDetails",
                column: "userID");

            migrationBuilder.CreateIndex(
                name: "IX_UserDetails_friendID",
                table: "UserDetails",
                column: "friendID");

            migrationBuilder.CreateIndex(
                name: "IX_UserDetails_giftID",
                table: "UserDetails",
                column: "giftID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SettingModels");

            migrationBuilder.DropTable(
                name: "UserDetails");

            migrationBuilder.DropTable(
                name: "GiftModels");

            migrationBuilder.DropTable(
                name: "UserModels");
        }
    }
}
