using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAcess.Migrations
{
    /// <inheritdoc />
    public partial class test : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Ingredient",
                columns: table => new
                {
                    Ingredient_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ingredient_Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ingredient", x => x.Ingredient_Id);
                });

            migrationBuilder.CreateTable(
                name: "Recipe",
                columns: table => new
                {
                    Recipe_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Recipe_Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Time = table.Column<int>(type: "int", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Preparation_Method = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recipe", x => x.Recipe_Id);
                });

            migrationBuilder.CreateTable(
                name: "RecipeRawDTO",
                columns: table => new
                {
                    RecipeId = table.Column<int>(type: "int", nullable: false),
                    Recipe_Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Preparation_Method = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Time = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Calories_100g = table.Column<double>(type: "float", nullable: false),
                    Fat_100g = table.Column<double>(type: "float", nullable: false),
                    Sugar_100g = table.Column<double>(type: "float", nullable: false),
                    Protein_100g = table.Column<double>(type: "float", nullable: false),
                    Carb_100 = table.Column<double>(type: "float", nullable: false),
                    Ingredient_Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "Nutrition",
                columns: table => new
                {
                    Recipe_Id = table.Column<int>(type: "int", nullable: false),
                    Calories_100g = table.Column<double>(type: "float", nullable: false),
                    Fat_100g = table.Column<double>(type: "float", nullable: false),
                    Sugar_100g = table.Column<double>(type: "float", nullable: false),
                    Protein_100g = table.Column<double>(type: "float", nullable: false),
                    Carb_100 = table.Column<double>(type: "float", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nutrition", x => x.Recipe_Id);
                    table.ForeignKey(
                        name: "FK_Nutrition_Recipe_Recipe_Id",
                        column: x => x.Recipe_Id,
                        principalTable: "Recipe",
                        principalColumn: "Recipe_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Recipe_Ingredient",
                columns: table => new
                {
                    RecipeId = table.Column<int>(type: "int", nullable: false),
                    Ingredient_Id = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<double>(type: "float", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recipe_Ingredient", x => new { x.RecipeId, x.Ingredient_Id });
                    table.ForeignKey(
                        name: "FK_Recipe_Ingredient_Ingredient_Ingredient_Id",
                        column: x => x.Ingredient_Id,
                        principalTable: "Ingredient",
                        principalColumn: "Ingredient_Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Recipe_Ingredient_Recipe_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipe",
                        principalColumn: "Recipe_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Recipe_Ingredient_Ingredient_Id",
                table: "Recipe_Ingredient",
                column: "Ingredient_Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Nutrition");

            migrationBuilder.DropTable(
                name: "Recipe_Ingredient");

            migrationBuilder.DropTable(
                name: "RecipeRawDTO");

            migrationBuilder.DropTable(
                name: "Ingredient");

            migrationBuilder.DropTable(
                name: "Recipe");
        }
    }
}
