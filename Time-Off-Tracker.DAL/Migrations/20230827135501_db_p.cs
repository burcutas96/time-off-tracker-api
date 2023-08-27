using Microsoft.EntityFrameworkCore.Migrations;

namespace Time_Off_Tracker.DAL.Migrations
{
    public partial class db_p : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TimeOffType",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeOffType",
                table: "Users");
        }
    }
}
