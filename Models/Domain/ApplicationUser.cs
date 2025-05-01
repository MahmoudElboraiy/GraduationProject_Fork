using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Models.Domain;

public class ApplicationUser : IdentityUser
{
    [Required]
    public string FullName { get; set; }

    public int? ImageId { get; set; }

    [ForeignKey("ImageId")]

    public ICollection<FavoriteRecipe> FavoriteRecipes { get; set; }
}
