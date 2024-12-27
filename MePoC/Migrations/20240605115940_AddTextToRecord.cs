﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MePoC.Migrations
{
    /// <inheritdoc />
    public partial class AddTextToRecord : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Text",
                table: "Records",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Text",
                table: "Records");
        }
    }
}
