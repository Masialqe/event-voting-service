using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EVS.App.Migrations.ApplicationDb
{
    /// <inheritdoc />
    public partial class RowVersionAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "VoterEvent_RowVersion",
                table: "VoterEvent",
                type: "bytea",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VoterEvent_RowVersion",
                table: "VoterEvent");
        }
    }
}
