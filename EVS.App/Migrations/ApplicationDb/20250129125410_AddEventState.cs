using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EVS.App.Migrations.ApplicationDb
{
    /// <inheritdoc />
    public partial class AddEventState : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Event_State",
                table: "Events",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Event_State",
                table: "Events");
        }
    }
}
