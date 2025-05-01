using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Domain
{
    public class FavoriteRecipe
    {
        public string UserId { get; set; }
        public int RecipeId { get; set; }
        public DateTime AddedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public int? ImageId { get; set; }  
        public ApplicationUser User { get; set; }
        public Recipe Recipe { get; set; }
    }
}
