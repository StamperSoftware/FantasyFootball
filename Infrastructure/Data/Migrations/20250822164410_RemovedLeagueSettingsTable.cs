using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemovedLeagueSettingsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LeagueSettings");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LeagueSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FlexLimit = table.Column<int>(type: "int", nullable: false),
                    LeagueId = table.Column<int>(type: "int", nullable: false),
                    NumberOfGames = table.Column<int>(type: "int", nullable: false),
                    NumberOfTeams = table.Column<int>(type: "int", nullable: false),
                    PassingTouchdownsScore = table.Column<int>(type: "int", nullable: false),
                    PassingYardsScore = table.Column<double>(type: "float", nullable: false),
                    ReceivingTouchdownsScore = table.Column<int>(type: "int", nullable: false),
                    ReceivingYardsScore = table.Column<double>(type: "float", nullable: false),
                    ReceptionScore = table.Column<int>(type: "int", nullable: false),
                    RosterLimit = table.Column<int>(type: "int", nullable: false),
                    RushingTouchdownsScore = table.Column<int>(type: "int", nullable: false),
                    RushingYardsScore = table.Column<double>(type: "float", nullable: false),
                    StartingQuarterBackLimit = table.Column<int>(type: "int", nullable: false),
                    StartingRunningBackLimit = table.Column<int>(type: "int", nullable: false),
                    StartingTightEndLimit = table.Column<int>(type: "int", nullable: false),
                    StartingWideReceiverLimit = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeagueSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LeagueSettings_Leagues_LeagueId",
                        column: x => x.LeagueId,
                        principalTable: "Leagues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LeagueSettings_LeagueId",
                table: "LeagueSettings",
                column: "LeagueId",
                unique: true);
        }
    }
}
