using AutoMapper;
using DataAcess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.DTOs.Food;

[ApiController]
[Route("api/[controller]")]
public class FoodController : ControllerBase
{
    private readonly ApplicationDbContext _db;
    private readonly IMapper _mapper;

    public FoodController(IMapper mapper, ApplicationDbContext db)
    {
        _db = db;
        _mapper = mapper;
    }

    //  Get all recipes (limited to 4992) with full nutrition and ingredients
    [HttpGet("recipes")]
    public IActionResult GetAllRecipes()
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

    // Get first 500 recipes (preview)
    [HttpGet("recipes/preview")]
    public IActionResult GetRecipePreview()
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
    [HttpGet("ingredients")]
    public IActionResult GetAllIngredients()
    {
        var ingredients = _db.Ingredient
            .AsNoTracking()
            .ToList();

        return Ok(ingredients);
    }

    //  Search recipes by name (up to 50)
    [HttpGet("recipes/search/by-name/{name:alpha}")]
    public IActionResult SearchRecipesByName(string name)
    {
        var recipes = _db.Recipe
            .Where(x => EF.Functions.Like(x.Recipe_Name, $"%{name}%"))
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

        var result = _mapper.Map<List<RecipeWithNutritionDTO>>(recipes);
        return Ok(result);
    }

    //  Search ingredients by name
    [HttpGet("ingredients/search/by-name/{ingredientName:alpha}")]
    public IActionResult SearchIngredientIdsByName(string ingredientName)
    {
        var ids = _db.Ingredient
            .AsNoTracking()
            .Where(ing => EF.Functions.Like(ing.Ingredient_Name.ToLower(), $"%{ingredientName.ToLower()}%"))
            .Select(i => i.Ingredient_Id)
            .ToList();

        return Ok(ids);
    }

    //  Search recipes by ingredient IDs
    [HttpPost("recipes/search/by-ingredient-ids")]
    public ActionResult<List<RecipeWithNutritionDTO>> SearchRecipesByIngredientIds([FromBody] List<int> ingredientIds)
    {
        var recipes = _db.Recipe_Ingredient
            .AsNoTracking()
            .Where(ri => ingredientIds.Contains(ri.Ingredient_Id))
            .Select(ri => ri.Recipe)
            .Distinct()
            .Include(r => r.Nutrition)
            .Include(r => r.Recipe_Ingredient)
                .ThenInclude(ri => ri.Ingredient)
            .Take(50);

        var result = _mapper.Map<List<RecipeWithNutritionDTO>>(recipes);
        return Ok(result);
    }
}
