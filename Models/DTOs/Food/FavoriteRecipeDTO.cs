using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs.Food
{
    public class FavoriteRecipeDTO
    {
        public int RecipeId { get; set; }
        public string RecipeName { get; set; }
        public DateTime AddedAt { get; set; }
    }
}
