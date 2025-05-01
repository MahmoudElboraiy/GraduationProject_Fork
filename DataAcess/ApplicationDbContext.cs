using DataAcess.Configuration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Models.Domain;
using Models.DTOs.Food;

namespace DataAcess
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            // this is pull
            // this is pull 2 
        }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<FavoriteRecipe> FavoriteRecipes { get; set; }
        public DbSet<Image> Image { get; set; }
        public DbSet<Recipe> Recipe { get; set; }
        public DbSet<Nutrition> Nutrition { get; set; }
        public DbSet<Ingredient> Ingredient { get; set; }
        public DbSet<Recipe_Ingredient> Recipe_Ingredient { get; set; }
        public DbSet<RecipeRawDTO> RecipeRawDTO { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<FavoriteRecipe>()
    .HasKey(fr => new { fr.UserId, fr.RecipeId }); 

            builder.Entity<FavoriteRecipe>()
                .HasOne(fr => fr.User)
                .WithMany(u => u.FavoriteRecipes)
                .HasForeignKey(fr => fr.UserId)
                .OnDelete(DeleteBehavior.Cascade); 

            builder.Entity<FavoriteRecipe>()
                .HasOne(fr => fr.Recipe)
                .WithMany(r => r.FavoritedBy)
                .HasForeignKey(fr => fr.RecipeId)
                .OnDelete(DeleteBehavior.Cascade); 

            // Apply separate configuration classes
            builder.ApplyConfiguration(new ApplicationUserConfiguration());
            builder.ApplyConfiguration(new RoleConfiguration());
            builder.ApplyConfiguration(new RecipeConfiguration());
            builder.ApplyConfiguration(new NutritionConfiguration());
            builder.ApplyConfiguration(new IngredientConfiguration());
            builder.ApplyConfiguration(new RecipeIngredientConfiguration());
            builder.Entity<RecipeRawDTO>().HasNoKey();
        }

    }
}
