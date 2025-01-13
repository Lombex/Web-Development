using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web_Development.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "EventAttendanceId",
                table: "Attendances",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_EventAttendanceId",
                table: "Attendances",
                column: "EventAttendanceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attendances_EventAttendances_EventAttendanceId",
                table: "Attendances",
                column: "EventAttendanceId",
                principalTable: "EventAttendances",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attendances_EventAttendances_EventAttendanceId",
                table: "Attendances");

            migrationBuilder.DropIndex(
                name: "IX_Attendances_EventAttendanceId",
                table: "Attendances");

            migrationBuilder.DropColumn(
                name: "EventAttendanceId",
                table: "Attendances");
        }
    }
}
