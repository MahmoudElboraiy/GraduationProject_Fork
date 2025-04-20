using AutoMapper; // For mapping entities to DTOs
using DataAcess;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore; // For EF Core operations
using Models.Domain; // Database models
using Models.DTOs.Food; // DTOs for API responses
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace IdentityManagerAPI.Controllers
{
    [Route("Api/[controller]")]
    [ApiController]
    public class FoodController : ControllerBase
    {
        private readonly ApplicationDbContext _db; // Database context
        private readonly IMapper _mapper; // AutoMapper for object mapping

        public FoodController(IMapper mapper, ApplicationDbContext db)
        {
            _db = db;
            _mapper = mapper;
        }

        // Get all recipes with their nutrition info
        [HttpGet("GetRecipesWithFullNutrition")]
        public IActionResult GetRecipesWithFullNutrition()
        {
            var recipes = _db.Recipe
                .Include(r => r.Nutrition)
                .AsNoTracking()
                .Take(4992)
                .ToList();

            var result = _mapper.Map<List<RecipeWithNutritionDTO>>(recipes);
            return Ok(result);
        }

        // Get all ingredients from DB
        [HttpGet("GetAllIngredients")]
        public IActionResult getALL()
        {
            var r = _db.Ingredient.ToList();
            return Ok(r);
        }

        // Search for recipes by name
        [HttpGet("{Name:alpha}")]
        public IActionResult GetSearchByName(string Name)
        {
            var recipes = _db.Recipe
                .Where(x => EF.Functions.Like(x.Recipe_Name, $"%{Name}%"))
                .Take(50)
                .AsNoTracking()
                .ToList();

            // Load related Nutrition and Ingredients
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

        // Search ingredients by name (returns IDs)
        [HttpGet("SearchByIngredient/{ingredientName:alpha}")]
        public IActionResult GetSearchByIngredient(string ingredientName)
        {
            ingredientName = ingredientName.ToLowerInvariant();

            var ingredientList = _db.Ingredient
                .AsNoTracking()
                .Where(ing => EF.Functions.Like(ing.Ingredient_Name, $"%{ingredientName}%"))
                .Select(i => i.Ingredient_Id);

            return Ok(ingredientList.ToList());
        }

        // Get recipes that include any of the given ingredient IDs
        [HttpPost]
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
