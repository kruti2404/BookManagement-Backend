using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookManagement_Backend.Migrations
{
    /// <inheritdoc />
    public partial class fixedTheFieldName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Agr",
                table: "Authors",
                newName: "Age");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Age",
                table: "Authors",
                newName: "Agr");
        }
    }
}
