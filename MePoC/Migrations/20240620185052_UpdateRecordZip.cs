using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MePoC.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRecordZip : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsProcessed",
                table: "Records",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "JobId",
                table: "Records",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsProcessed",
                table: "Records");

            migrationBuilder.DropColumn(
                name: "JobId",
                table: "Records");
        }
    }
}
