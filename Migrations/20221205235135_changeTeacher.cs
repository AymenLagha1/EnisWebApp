using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASP.NETCoreIdentityCustom.Migrations
{
    public partial class changeTeacher : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TeacherId",
                table: "Project",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Project_TeacherId",
                table: "Project",
                column: "TeacherId");

            migrationBuilder.AddForeignKey(
                name: "FK_Project_AspNetUsers_TeacherId",
                table: "Project",
                column: "TeacherId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Project_AspNetUsers_TeacherId",
                table: "Project");

            migrationBuilder.DropIndex(
                name: "IX_Project_TeacherId",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "TeacherId",
                table: "Project");
        }
    }
}
