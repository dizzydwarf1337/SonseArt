using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SonseArt.Migrations
{
    /// <inheritdoc />
    public partial class CartItemImgSrc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImgSrc",
                table: "CartItem",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImgSrc",
                table: "CartItem");
        }
    }
}
