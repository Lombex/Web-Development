using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web_Development.Migrations
{
    /// <inheritdoc />
    public partial class AddAttendanceTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventAttendances_Events_EventID",
                table: "EventAttendances");

            migrationBuilder.DropForeignKey(
                name: "FK_EventAttendances_Users_UserID",
                table: "EventAttendances");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "EventAttendances",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "EventID",
                table: "EventAttendances",
                newName: "EventId");

            migrationBuilder.RenameIndex(
                name: "IX_EventAttendances_UserID",
                table: "EventAttendances",
                newName: "IX_EventAttendances_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_EventAttendances_EventID",
                table: "EventAttendances",
                newName: "IX_EventAttendances_EventId");

            migrationBuilder.AddForeignKey(
                name: "FK_EventAttendances_Events_EventId",
                table: "EventAttendances",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EventAttendances_Users_UserId",
                table: "EventAttendances",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventAttendances_Events_EventId",
                table: "EventAttendances");

            migrationBuilder.DropForeignKey(
                name: "FK_EventAttendances_Users_UserId",
                table: "EventAttendances");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "EventAttendances",
                newName: "UserID");

            migrationBuilder.RenameColumn(
                name: "EventId",
                table: "EventAttendances",
                newName: "EventID");

            migrationBuilder.RenameIndex(
                name: "IX_EventAttendances_UserId",
                table: "EventAttendances",
                newName: "IX_EventAttendances_UserID");

            migrationBuilder.RenameIndex(
                name: "IX_EventAttendances_EventId",
                table: "EventAttendances",
                newName: "IX_EventAttendances_EventID");

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
    }
}
