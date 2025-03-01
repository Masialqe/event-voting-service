using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EVS.App.Migrations.ApplicationDb
{
    /// <inheritdoc />
    public partial class ExtendedEvent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Event_AvailableVotingPoints",
                table: "Events",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Event_VotesCount",
                table: "Events",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_VoterEvent_VoterEvent_Id",
                table: "VoterEvent",
                column: "VoterEvent_Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Events_Event_Id",
                table: "Events",
                column: "Event_Id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_VoterEvent_VoterEvent_Id",
                table: "VoterEvent");

            migrationBuilder.DropIndex(
                name: "IX_Events_Event_Id",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "Event_AvailableVotingPoints",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "Event_VotesCount",
                table: "Events");
        }
    }
}
