using Microsoft.EntityFrameworkCore.Migrations;

namespace HealthEngineAPI.Migrations
{
    public partial class addrltshp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SpecialitieId",
                table: "SubSpecialities",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_SubSpecialities_SpecialitieId",
                table: "SubSpecialities",
                column: "SpecialitieId");

            migrationBuilder.AddForeignKey(
                name: "FK_SubSpecialities_Specialities_SpecialitieId",
                table: "SubSpecialities",
                column: "SpecialitieId",
                principalTable: "Specialities",
                principalColumn: "SpecialitieId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubSpecialities_Specialities_SpecialitieId",
                table: "SubSpecialities");

            migrationBuilder.DropIndex(
                name: "IX_SubSpecialities_SpecialitieId",
                table: "SubSpecialities");

            migrationBuilder.DropColumn(
                name: "SpecialitieId",
                table: "SubSpecialities");
        }
    }
}
