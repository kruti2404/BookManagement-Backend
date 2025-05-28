using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookManagement_Backend.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedFieldName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EstablishedYear",
                table: "Publications",
                newName: "Established");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Established",
                table: "Publications",
                newName: "EstablishedYear");
        }
    }
}
