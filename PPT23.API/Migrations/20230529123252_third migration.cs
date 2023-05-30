using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PPT23.API.Migrations
{
    /// <inheritdoc />
    public partial class thirdmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateTime",
                table: "Revizes",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "VybaveniId",
                table: "Revizes",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Revizes_VybaveniId",
                table: "Revizes",
                column: "VybaveniId");

            migrationBuilder.AddForeignKey(
                name: "FK_Revizes_Vybavenis_VybaveniId",
                table: "Revizes",
                column: "VybaveniId",
                principalTable: "Vybavenis",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Revizes_Vybavenis_VybaveniId",
                table: "Revizes");

            migrationBuilder.DropIndex(
                name: "IX_Revizes_VybaveniId",
                table: "Revizes");

            migrationBuilder.DropColumn(
                name: "DateTime",
                table: "Revizes");

            migrationBuilder.DropColumn(
                name: "VybaveniId",
                table: "Revizes");
        }
    }
}
