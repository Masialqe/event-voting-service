using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EVS.App.Migrations.ApplicationDb
{
    /// <inheritdoc />
    public partial class EventNameIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Events_Event_Name",
                table: "Events",
                column: "Event_Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Events_Event_Name",
                table: "Events");
        }
    }
}
