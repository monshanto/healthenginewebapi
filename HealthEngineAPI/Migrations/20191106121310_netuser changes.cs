using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HealthEngineAPI.Migrations
{
    public partial class netuserchanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsMailVerified",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IsPhoneVerified",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<DateTime>(
                name: "RecordedAt",
                table: "DoctorDetails",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "DoctorDetails",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "DoctorDetails",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OTP",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RecordedAt",
                table: "DoctorDetails");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "DoctorDetails");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "DoctorDetails");

            migrationBuilder.DropColumn(
                name: "OTP",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<bool>(
                name: "IsMailVerified",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPhoneVerified",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
