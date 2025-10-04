using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HattrickApp.Api.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddOffersAndOfferTips : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Offer",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstCompetitor = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SecondCompetitor = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    StartTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    IsTopOffer = table.Column<bool>(type: "bit", nullable: false),
                    SportType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Offer", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OfferTip",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OfferId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TipCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Quota = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfferTip", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OfferTip_Offer_OfferId",
                        column: x => x.OfferId,
                        principalTable: "Offer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OfferTip_OfferId",
                table: "OfferTip",
                column: "OfferId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OfferTip");

            migrationBuilder.DropTable(
                name: "Offer");
        }
    }
}
