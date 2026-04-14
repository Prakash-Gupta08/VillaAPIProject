using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VillaWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class SeedVillas3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Villa",
                keyColumn: "Id",
                keyValue: 2);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Villa",
                columns: new[] { "Id", "CreatedDate", "Details", "ImageUrl", "Name", "Occupancy", "Rate", "Sqft", "UpdatedDate" },
                values: new object[] { 2, new DateTime(2026, 4, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "Very Premium", "https://dor=tnet.dotnet,com", "Luxury Villa", 6, 1200.0, 5000, new DateTime(2026, 4, 13, 0, 0, 0, 0, DateTimeKind.Unspecified) });
        }
    }
}
