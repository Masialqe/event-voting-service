using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EVS.App.Migrations.ApplicationDb
{
    /// <inheritdoc />
    public partial class AppMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Event",
                columns: table => new
                {
                    Event_Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Event_Name = table.Column<string>(type: "varchar(100)", nullable: false),
                    Event_Description = table.Column<string>(type: "varchar(500)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Event", x => x.Event_Id);
                });

            migrationBuilder.CreateTable(
                name: "Voters",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Voter_Nickname = table.Column<string>(type: "varchar(50)", nullable: false),
                    Voter_UserId = table.Column<string>(type: "varchar(100)", nullable: false),
                    Voter_Email = table.Column<string>(type: "varchar(254)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Voters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VoterEvent",
                columns: table => new
                {
                    VoterEvent_Id = table.Column<Guid>(type: "uuid", nullable: false),
                    VoterId = table.Column<Guid>(type: "uuid", nullable: false),
                    EventId = table.Column<Guid>(type: "uuid", nullable: false),
                    VoterEvent_Score = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    VoterEvent_HasVoted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VoterEvent", x => x.VoterEvent_Id);
                    table.ForeignKey(
                        name: "FK_VoterEvent_Event_EventId",
                        column: x => x.EventId,
                        principalTable: "Event",
                        principalColumn: "Event_Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VoterEvent_Voters_VoterId",
                        column: x => x.VoterId,
                        principalTable: "Voters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VoterEvent_EventId",
                table: "VoterEvent",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_VoterEvent_VoterId",
                table: "VoterEvent",
                column: "VoterId");

            migrationBuilder.CreateIndex(
                name: "IX_Voters_Voter_Email",
                table: "Voters",
                column: "Voter_Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Voters_Voter_Nickname",
                table: "Voters",
                column: "Voter_Nickname",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Voters_Voter_UserId",
                table: "Voters",
                column: "Voter_UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VoterEvent");

            migrationBuilder.DropTable(
                name: "Event");

            migrationBuilder.DropTable(
                name: "Voters");
        }
    }
}
