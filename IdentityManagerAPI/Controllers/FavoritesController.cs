using DataAcess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Domain;
using Models.DTOs.Food;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class FavoritesController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public FavoritesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // Add recipe to favorites
    [HttpPost("{recipeId}")]
    public async Task<IActionResult> AddToFavorites(int recipeId)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Unauthorized();

        var exists = await _context.FavoriteRecipes
            .AnyAsync(f => f.UserId == user.Id && f.RecipeId == recipeId);
        if (exists) return BadRequest("Recipe already in favorites");

        var recipe = await _context.Recipe.FirstOrDefaultAsync(r => r.Recipe_Id == recipeId);
        if (recipe == null) return NotFound("Recipe not found");

        var favorite = new FavoriteRecipe
        {
            UserId = user.Id,
            RecipeId = recipeId
        };

        _context.FavoriteRecipes.Add(favorite);
        await _context.SaveChangesAsync();

        return Ok(new { message = $"Recipe '{recipe.Recipe_Name}' added to favorites successfully." });
    }

    // Get user's favorite recipes
    [HttpGet]
    public async Task<IActionResult> GetFavorites()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Unauthorized();

        var favorites = await _context.FavoriteRecipes
            .Where(f => f.UserId == user.Id)
            .Include(f => f.Recipe)
                .ThenInclude(r => r.Nutrition)
            .Include(f => f.Recipe)
                .ThenInclude(r => r.Recipe_Ingredient)
                    .ThenInclude(ri => ri.Ingredient)
            .Select(f => new RecipeWithNutritionDTO
            {
                RecipeId = f.Recipe.Recipe_Id,
                Recipe_Name = f.Recipe.Recipe_Name,
                Description = f.Recipe.Description,
                Preparation_Method = f.Recipe.Preparation_Method,
                Time = f.Recipe.Time,
                Calories_100g = f.Recipe.Nutrition.Calories_100g,
                Fat_100g = f.Recipe.Nutrition.Fat_100g,
                Sugar_100g = f.Recipe.Nutrition.Sugar_100g,
                Protein_100g = f.Recipe.Nutrition.Protein_100g,
                Carb_100 = f.Recipe.Nutrition.Carb_100,
                Type = f.Recipe.Nutrition.Type,
                IngredientNames = f.Recipe.Recipe_Ingredient
                    .Select(ri => ri.Ingredient.Ingredient_Name ?? "Unknown")
                    .ToList(),
                AddedAt = f.AddedAt
            })
            .ToListAsync();

        return Ok(favorites);
    }

    // Remove recipe from favorites
    [HttpDelete("{recipeId}")]
    public async Task<IActionResult> RemoveFromFavorites(int recipeId)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Unauthorized();

        var favorite = await _context.FavoriteRecipes
            .FirstOrDefaultAsync(f => f.UserId == user.Id && f.RecipeId == recipeId);

        if (favorite == null) return NotFound();

        _context.FavoriteRecipes.Remove(favorite);
        await _context.SaveChangesAsync();
        return Ok();
    }
}
