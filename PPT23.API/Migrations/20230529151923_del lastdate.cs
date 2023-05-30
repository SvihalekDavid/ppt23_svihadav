using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PPT23.API.Migrations
{
    /// <inheritdoc />
    public partial class dellastdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastRevisionDateTime",
                table: "Vybavenis");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastRevisionDateTime",
                table: "Vybavenis",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
