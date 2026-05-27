using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTrainingModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApplyOnEveryDayIndex",
                table: "TrainingSets");

            migrationBuilder.AddColumn<int>(
                name: "IncrementOrder",
                table: "Trainings",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IncrementOrder",
                table: "Trainings");

            migrationBuilder.AddColumn<int>(
                name: "ApplyOnEveryDayIndex",
                table: "TrainingSets",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
