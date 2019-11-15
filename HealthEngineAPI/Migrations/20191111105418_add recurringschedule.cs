using Microsoft.EntityFrameworkCore.Migrations;

namespace HealthEngineAPI.Migrations
{
    public partial class addrecurringschedule : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DoctorRecurringSchedules",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DoctorId = table.Column<string>(nullable: true),
                    ToTime = table.Column<string>(nullable: true),
                    FromTime = table.Column<string>(nullable: true),
                    IsMon = table.Column<bool>(nullable: false),
                    IsTue = table.Column<bool>(nullable: false),
                    IsWed = table.Column<bool>(nullable: false),
                    IsThurs = table.Column<bool>(nullable: false),
                    IsFri = table.Column<bool>(nullable: false),
                    IsSat = table.Column<bool>(nullable: false),
                    IsSun = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoctorRecurringSchedules", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DoctorRecurringSchedules");
        }
    }
}
