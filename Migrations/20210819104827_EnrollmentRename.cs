using Microsoft.EntityFrameworkCore.Migrations;

namespace efcore.Migrations
{
    public partial class EnrollmentRename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Entrollment_Course_CourseID",
                table: "Entrollment");

            migrationBuilder.DropForeignKey(
                name: "FK_Entrollment_Student_StudentID",
                table: "Entrollment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Entrollment",
                table: "Entrollment");

            migrationBuilder.RenameTable(
                name: "Entrollment",
                newName: "Enrollment");

            migrationBuilder.RenameIndex(
                name: "IX_Entrollment_StudentID",
                table: "Enrollment",
                newName: "IX_Enrollment_StudentID");

            migrationBuilder.RenameIndex(
                name: "IX_Entrollment_CourseID",
                table: "Enrollment",
                newName: "IX_Enrollment_CourseID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Enrollment",
                table: "Enrollment",
                column: "EnrollmentID");

            migrationBuilder.AddForeignKey(
                name: "FK_Enrollment_Course_CourseID",
                table: "Enrollment",
                column: "CourseID",
                principalTable: "Course",
                principalColumn: "CourseID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Enrollment_Student_StudentID",
                table: "Enrollment",
                column: "StudentID",
                principalTable: "Student",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Enrollment_Course_CourseID",
                table: "Enrollment");

            migrationBuilder.DropForeignKey(
                name: "FK_Enrollment_Student_StudentID",
                table: "Enrollment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Enrollment",
                table: "Enrollment");

            migrationBuilder.RenameTable(
                name: "Enrollment",
                newName: "Entrollment");

            migrationBuilder.RenameIndex(
                name: "IX_Enrollment_StudentID",
                table: "Entrollment",
                newName: "IX_Entrollment_StudentID");

            migrationBuilder.RenameIndex(
                name: "IX_Enrollment_CourseID",
                table: "Entrollment",
                newName: "IX_Entrollment_CourseID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Entrollment",
                table: "Entrollment",
                column: "EnrollmentID");

            migrationBuilder.AddForeignKey(
                name: "FK_Entrollment_Course_CourseID",
                table: "Entrollment",
                column: "CourseID",
                principalTable: "Course",
                principalColumn: "CourseID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Entrollment_Student_StudentID",
                table: "Entrollment",
                column: "StudentID",
                principalTable: "Student",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
