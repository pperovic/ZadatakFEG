using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HattrickApp.Api.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddMissingNavigationPropertyOfferToTicketSelection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_TicketSelections_OfferId",
                table: "TicketSelections",
                column: "OfferId");

            migrationBuilder.AddForeignKey(
                name: "FK_TicketSelections_Offers_OfferId",
                table: "TicketSelections",
                column: "OfferId",
                principalTable: "Offers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TicketSelections_Offers_OfferId",
                table: "TicketSelections");

            migrationBuilder.DropIndex(
                name: "IX_TicketSelections_OfferId",
                table: "TicketSelections");
        }
    }
}
