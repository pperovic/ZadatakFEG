using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HattrickApp.Api.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ModifyOffer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OfferTip_Offer_OfferId",
                table: "OfferTip");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OfferTip",
                table: "OfferTip");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Offer",
                table: "Offer");

            migrationBuilder.RenameTable(
                name: "OfferTip",
                newName: "OfferTips");

            migrationBuilder.RenameTable(
                name: "Offer",
                newName: "Offers");

            migrationBuilder.RenameIndex(
                name: "IX_OfferTip_OfferId",
                table: "OfferTips",
                newName: "IX_OfferTips_OfferId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OfferTips",
                table: "OfferTips",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Offers",
                table: "Offers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OfferTips_Offers_OfferId",
                table: "OfferTips",
                column: "OfferId",
                principalTable: "Offers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OfferTips_Offers_OfferId",
                table: "OfferTips");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OfferTips",
                table: "OfferTips");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Offers",
                table: "Offers");

            migrationBuilder.RenameTable(
                name: "OfferTips",
                newName: "OfferTip");

            migrationBuilder.RenameTable(
                name: "Offers",
                newName: "Offer");

            migrationBuilder.RenameIndex(
                name: "IX_OfferTips_OfferId",
                table: "OfferTip",
                newName: "IX_OfferTip_OfferId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OfferTip",
                table: "OfferTip",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Offer",
                table: "Offer",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OfferTip_Offer_OfferId",
                table: "OfferTip",
                column: "OfferId",
                principalTable: "Offer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
