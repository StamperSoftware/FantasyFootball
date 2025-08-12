using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemovedWeeklyStatsFromGame : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AthleteWeeklyStats_Games_GameId",
                table: "AthleteWeeklyStats");

            migrationBuilder.DropIndex(
                name: "IX_AthleteWeeklyStats_GameId",
                table: "AthleteWeeklyStats");

            migrationBuilder.DropColumn(
                name: "GameId",
                table: "AthleteWeeklyStats");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GameId",
                table: "AthleteWeeklyStats",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AthleteWeeklyStats_GameId",
                table: "AthleteWeeklyStats",
                column: "GameId");

            migrationBuilder.AddForeignKey(
                name: "FK_AthleteWeeklyStats_Games_GameId",
                table: "AthleteWeeklyStats",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id");
        }
    }
}
