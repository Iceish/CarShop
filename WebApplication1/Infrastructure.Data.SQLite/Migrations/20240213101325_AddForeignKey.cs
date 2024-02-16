using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class AddForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Parameter_RecipeId",
                table: "Parameter",
                column: "RecipeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Parameter_Recipe_RecipeId",
                table: "Parameter",
                column: "RecipeId",
                principalTable: "Recipe",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Parameter_Recipe_RecipeId",
                table: "Parameter");

            migrationBuilder.DropIndex(
                name: "IX_Parameter_RecipeId",
                table: "Parameter");
        }
    }
}
