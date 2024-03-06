using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kavifx.API.Migrations
{
    /// <inheritdoc />
    public partial class Init3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Picdata",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "ProfilePicturePath",
                table: "AspNetUsers",
                newName: "PictureUrl");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PictureUrl",
                table: "AspNetUsers",
                newName: "ProfilePicturePath");

            migrationBuilder.AddColumn<byte[]>(
                name: "Picdata",
                table: "AspNetUsers",
                type: "varbinary(max)",
                nullable: true);
        }
    }
}
