using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SonseArt.Migrations
{
    /// <inheritdoc />
    public partial class ProductCheck : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comments",
                table: "Product");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Comments",
                table: "Product",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
