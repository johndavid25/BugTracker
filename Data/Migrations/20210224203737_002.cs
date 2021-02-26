using Microsoft.EntityFrameworkCore.Migrations;

namespace BugTracker.Data.Migrations
{
    public partial class _002 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TicketComments_AspNetUsers_BTUserId",
                table: "TicketComments");

            migrationBuilder.DropIndex(
                name: "IX_TicketComments_BTUserId",
                table: "TicketComments");

            migrationBuilder.DropColumn(
                name: "BTUserId",
                table: "TicketComments");

            migrationBuilder.CreateIndex(
                name: "IX_TicketComments_UserId",
                table: "TicketComments",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TicketComments_AspNetUsers_UserId",
                table: "TicketComments",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TicketComments_AspNetUsers_UserId",
                table: "TicketComments");

            migrationBuilder.DropIndex(
                name: "IX_TicketComments_UserId",
                table: "TicketComments");

            migrationBuilder.AddColumn<string>(
                name: "BTUserId",
                table: "TicketComments",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TicketComments_BTUserId",
                table: "TicketComments",
                column: "BTUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TicketComments_AspNetUsers_BTUserId",
                table: "TicketComments",
                column: "BTUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
