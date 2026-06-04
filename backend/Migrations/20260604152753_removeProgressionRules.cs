using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class removeProgressionRules : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProgressionType",
                table: "TrainingSets");

            migrationBuilder.DropColumn(
                name: "ProgressionValue",
                table: "TrainingSets");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProgressionType",
                table: "TrainingSets",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ProgressionValue",
                table: "TrainingSets",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
