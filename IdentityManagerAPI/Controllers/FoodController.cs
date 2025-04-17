using AutoMapper;
using DataAcess;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Models.Domain;
using Models.DTOs.Food;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
namespace IdentityManagerAPI.Controllers
{
    [Route("Api/[controller]")]
    [ApiController]
    public class FoodController : ControllerBase
    {
        //private readonly ConvertExcelDbContext _db;
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        public FoodController(IMapper mapper, ApplicationDbContext db)
        {
            _db = db;
            _mapper = mapper;
        }

        // NEW GET API All Recipes With Full Nutrition

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

        [HttpGet("GetAllIngredients")]

        public IActionResult getALL()
        {
            var r = _db.Ingredient.ToList();
            return Ok(r);
        }

        [HttpGet("{Name:alpha}")]
        public IActionResult GetSearchByName(string Name)
        {
            var recipes = _db.Recipe.Where(x => EF.Functions
            .Like(x.Recipe_Name, $"%{Name}%"))
                .Take(50)
                .AsNoTracking()
                .ToList();

            //if (pageSize > 0)
            //{
            //    if (pageSize > 100)
            //    {
            //        pageSize = 100;
            //    }
            //    recipes = recipes.Skip(pageSize * (pageNumber - 1)).Take(pageSize);
            //}

            foreach (var recipe in recipes)
            {

                _db.Entry(recipe).Reference(r => r.Nutrition).Load();

                _db.Entry(recipe).Collection(r => r.Recipe_Ingredient).Load();

                foreach (var ri in recipe.Recipe_Ingredient)
                {
                    _db.Entry(ri).Reference(r => r.Ingredient).Load();
                }
            }
           
            //var recipes = _db.Recipe
            //    .Where(x => EF.Functions.Like(x.Recipe_Name, $"%{Name}%"))
            //    .Include(r => r.Nutrition)
            //    .Include(r => r.Recipe_Ingredient)
            //        .ThenInclude(ri => ri.Ingredient)
            //    .AsNoTracking()
            //    .ToList();
            List<RecipeWithNutritionDTO> recipeWithNutritionDTOs = _mapper.Map<List<RecipeWithNutritionDTO>>(recipes);
            return Ok(recipeWithNutritionDTOs);
        }
        [HttpGet("SearchByIngredient/{ingredientName:alpha}")]
        public IActionResult GetSearchByIngredient(string ingredientName)
        {
            ingredientName = ingredientName.ToLowerInvariant();

            var ingredientList = _db.Ingredient.AsNoTracking()
                .Where(ing => EF.Functions.Like(ing.Ingredient_Name, $"%{ingredientName}%")).AsNoTracking()
                .Select(i => i.Ingredient_Id);

            return Ok(ingredientList.ToList());
        }

        [HttpPost]
        public ActionResult<List<RecipeWithNutritionDTO>> PostSearchByIngredientId([FromBody] List<int> ingredientsId)
        {

            //var recipes = _db.Recipe_Ingredient.AsNoTracking()
            //       .Where(ri => ingredientsId.Contains(ri.Ingredient_Id))
            //       .Select(ri => ri.Recipe)
            //        .Distinct();
            var recipes = _db.Recipe_Ingredient
               .AsNoTracking()
               .Where(ri => ingredientsId.Contains(ri.Ingredient_Id))
               .Take(50)
               .Select(ri => ri.Recipe)
               .Distinct()
               .Include(r => r.Nutrition)
               .Include(r => r.Recipe_Ingredient)
                   .ThenInclude(ri => ri.Ingredient);

            List<RecipeWithNutritionDTO> recipeWithNutritionDTOs = _mapper.Map<List<RecipeWithNutritionDTO>>(recipes);
            return Ok(recipeWithNutritionDTOs);
        }
    }
}
