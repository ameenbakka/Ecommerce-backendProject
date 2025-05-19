using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Migrations
{
    /// <inheritdoc />
    public partial class second : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "Bmw");

            migrationBuilder.InsertData(
                table: "Category",
                columns: new[] { "Id", "Name" },
                values: new object[] { 2, "Benz" });

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Description", "Price", "Title" },
                values: new object[] { "The BMW 7 Series is a full-size luxury sedan, known for its blend of comfort, performance, and advanced technology, serving as BMW's flagship model", 100m, "BMW 7 SERIES" });

            migrationBuilder.InsertData(
                table: "products",
                columns: new[] { "Id", "CategoryId", "Description", "Image", "Price", "Stock", "Title" },
                values: new object[] { 2, 2, "The Mercedes-Benz S-Class is the flagship, full-size luxury sedan and coupe series", "https://www.google.com/url?sa=i&url=https%3A%2F%2Fwww.amazon.in%2FHot-Wheels-Nissan-Skyline-Imports%2Fdp%2FB0DDHLX5B3&psig=AOvVaw3EoMvC6pCTZDa6eXw2gY_8&ust=1734499339117000&source=images&cd=vfe&opi=89978449&ved=0CBQQjRxqFwoTCKC409uHrooDFQAAAAAdAAAAABAE", 200m, 20, "BENZ S-CLASS" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "products",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "HotWheels");

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Description", "Price", "Title" },
                values: new object[] { "he Hot Wheels Nissan GT-R R34 is a detailed 1:64 scale die-cast model, capturing the iconic design and performance of the legendary Japanese sports car. It features aggressive styling, signature features like quad headlights and a rear spoiler, often in various color schemes and special editions", 700m, "Nissan Gtr R34" });
        }
    }
}
