using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web_Development.Migrations
{
    /// <inheritdoc />
    public partial class AddEventAttendanceRelationToUserAndEvent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_EventAttendances_EventID",
                table: "EventAttendances",
                column: "EventID");

            migrationBuilder.CreateIndex(
                name: "IX_EventAttendances_UserID",
                table: "EventAttendances",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_EventAttendances_Events_EventID",
                table: "EventAttendances",
                column: "EventID",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EventAttendances_Users_UserID",
                table: "EventAttendances",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventAttendances_Events_EventID",
                table: "EventAttendances");

            migrationBuilder.DropForeignKey(
                name: "FK_EventAttendances_Users_UserID",
                table: "EventAttendances");

            migrationBuilder.DropIndex(
                name: "IX_EventAttendances_EventID",
                table: "EventAttendances");

            migrationBuilder.DropIndex(
                name: "IX_EventAttendances_UserID",
                table: "EventAttendances");
        }
    }
}
