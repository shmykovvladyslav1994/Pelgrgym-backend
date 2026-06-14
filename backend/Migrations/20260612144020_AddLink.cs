using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class AddLink : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_TrainingEvents_TrainingId",
                table: "TrainingEvents",
                column: "TrainingId");

            migrationBuilder.AddForeignKey(
                name: "FK_TrainingEvents_Trainings_TrainingId",
                table: "TrainingEvents",
                column: "TrainingId",
                principalTable: "Trainings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrainingEvents_Trainings_TrainingId",
                table: "TrainingEvents");

            migrationBuilder.DropIndex(
                name: "IX_TrainingEvents_TrainingId",
                table: "TrainingEvents");
        }
    }
}
