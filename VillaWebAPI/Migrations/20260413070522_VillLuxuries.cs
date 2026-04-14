using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VillaWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class VillLuxuries : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Villa",
                columns: new[] { "Id", "CreatedDate", "Details", "ImageUrl", "Name", "Occupancy", "Rate", "Sqft", "UpdatedDate" },
                values: new object[] { 4, new DateTime(2026, 4, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "Luxurious villa", "https://dotnetexample.swagger,jsx", "Raj Villa", 6, 500.0, 2500, new DateTime(2026, 4, 13, 0, 0, 0, 0, DateTimeKind.Unspecified) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Villa",
                keyColumn: "Id",
                keyValue: 4);
        }
    }
}
