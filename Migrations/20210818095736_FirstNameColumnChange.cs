using Microsoft.EntityFrameworkCore.Migrations;

namespace efcore.Migrations
{
    public partial class FirstNameColumnChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FirstName",
                table: "Student",
                newName: "StartingName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StartingName",
                table: "Student",
                newName: "FirstName");
        }
    }
}
