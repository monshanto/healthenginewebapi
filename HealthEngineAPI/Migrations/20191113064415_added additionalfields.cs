using Microsoft.EntityFrameworkCore.Migrations;

namespace HealthEngineAPI.Migrations
{
    public partial class addedadditionalfields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Experience",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "GraduatedFrom",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MasterFrom",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MasterIn",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ReferralBonus",
                table: "AspNetUsers",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "TotalConsultations",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Experience",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "GraduatedFrom",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "MasterFrom",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "MasterIn",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ReferralBonus",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TotalConsultations",
                table: "AspNetUsers");
        }
    }
}
