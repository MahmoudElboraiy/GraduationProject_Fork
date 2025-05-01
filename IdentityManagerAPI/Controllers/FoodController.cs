using AutoMapper;
using DataAcess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.DTOs.Food;

namespace IdentityManagerAPI.Controllers
{
    [Route("Api/[controller]")]
    [ApiController]
    public class FoodController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public FoodController(IMapper mapper, ApplicationDbContext db)
        {
            _db = db;
            _mapper = mapper;
        }

        // Get up to 4992 recipes with nutrition and ingredients
        [HttpGet("RecipesWithNutritionAndIngredients")]
        public IActionResult GetRecipesWithFullNutrition()
        {
            try
            {
                var recipes = _db.Recipe
                    .Include(r => r.Nutrition)
                    .Include(r => r.Recipe_Ingredient)
                        .ThenInclude(ri => ri.Ingredient)
                    .AsNoTracking()
                    .Take(4992)
                    .ToList();

                var result = _mapper.Map<List<RecipeWithNutritionDTO>>(recipes);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving recipes: " + ex.Message);
            }
        }

        // Get 500 recipes with nutrition and ingredients
        [HttpGet("First500RecipesWithNutritionAndIngredients")]
        public IActionResult Get500Recipes()
        {
            try
            {
                var recipes = _db.Recipe
                    .Include(r => r.Nutrition)
                    .Include(r => r.Recipe_Ingredient)
                        .ThenInclude(ri => ri.Ingredient)
                    .AsNoTracking()
                    .Take(500)
                    .ToList();

                var result = _mapper.Map<List<RecipeWithNutritionDTO>>(recipes);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving recipes: " + ex.Message);
            }
        }

        // Get all ingredients
        [HttpGet("AllIngredients")]
        public IActionResult getALL()
        {
            var r = _db.Ingredient.ToList();
            return Ok(r);
        }

        // Search recipes by name (up to 50)
        [HttpGet("SearchRecipesByName/{Name:alpha}")]
        public IActionResult GetSearchByName(string Name)
        {
            var recipes = _db.Recipe
                .Where(x => EF.Functions.Like(x.Recipe_Name, $"%{Name}%"))
                .Take(50)
                .AsNoTracking()
                .ToList();

            foreach (var recipe in recipes)
            {
                _db.Entry(recipe).Reference(r => r.Nutrition).Load();
                _db.Entry(recipe).Collection(r => r.Recipe_Ingredient).Load();

                foreach (var ri in recipe.Recipe_Ingredient)
                {
                    _db.Entry(ri).Reference(r => r.Ingredient).Load();
                }
            }

            var recipeWithNutritionDTOs = _mapper.Map<List<RecipeWithNutritionDTO>>(recipes);
            return Ok(recipeWithNutritionDTOs);
        }

        // Search ingredient IDs by name
        [HttpGet("SearchIngredientIdsByName/{ingredientName:alpha}")]
        public IActionResult GetSearchByIngredient(string ingredientName)
        {
            ingredientName = ingredientName.ToLowerInvariant();

            var ingredientList = _db.Ingredient
                .AsNoTracking()
                .Where(ing => EF.Functions.Like(ing.Ingredient_Name, $"%{ingredientName}%"))
                .Select(i => i.Ingredient_Id);

            return Ok(ingredientList.ToList());
        }

        // Search recipes by ingredient IDs (up to 50)
        [HttpPost("SearchRecipesByIngredientIds")]
        public ActionResult<List<RecipeWithNutritionDTO>> PostSearchByIngredientId([FromBody] List<int> ingredientsId)
        {
            var recipes = _db.Recipe_Ingredient
                .AsNoTracking()
                .Where(ri => ingredientsId.Contains(ri.Ingredient_Id))
                .Take(50)
                .Select(ri => ri.Recipe)
                .Distinct()
                .Include(r => r.Nutrition)
                .Include(r => r.Recipe_Ingredient)
                    .ThenInclude(ri => ri.Ingredient);

            var recipeWithNutritionDTOs = _mapper.Map<List<RecipeWithNutritionDTO>>(recipes);
            return Ok(recipeWithNutritionDTOs);
        }





    }
}
