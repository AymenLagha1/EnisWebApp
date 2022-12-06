using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASP.NETCoreIdentityCustom.Migrations
{
    public partial class alteProject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Project_AspNetUsers_ApplicationUserId1",
                table: "Project");

            migrationBuilder.DropIndex(
                name: "IX_Project_ApplicationUserId1",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId1",
                table: "Project");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId1",
                table: "Project",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Project_ApplicationUserId1",
                table: "Project",
                column: "ApplicationUserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Project_AspNetUsers_ApplicationUserId1",
                table: "Project",
                column: "ApplicationUserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
