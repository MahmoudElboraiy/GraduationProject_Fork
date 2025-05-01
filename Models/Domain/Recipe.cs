using Models.Domain;

public partial class Recipe
{
    public int Recipe_Id { get; set; }
    public string Recipe_Name { get; set; } = null!;
    public int Time { get; set; }
    public string Description { get; set; } = null!;
    public string? Preparation_Method { get; set; }

    public Nutrition Nutrition { get; set; }
    public ICollection<Recipe_Ingredient> Recipe_Ingredient { get; set; }

    public ICollection<FavoriteRecipe> FavoritedBy { get; set; }
}
