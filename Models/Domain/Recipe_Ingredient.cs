    using System;
    using System.Collections.Generic;

    namespace Models.Domain
    {
        public partial class Recipe_Ingredient
        {
            public int RecipeId { get; set; }

            public int Ingredient_Id { get; set; }

            public double? Amount { get; set; }

            public virtual Ingredient? Ingredient { get; set; }

            public virtual Recipe? Recipe { get; set; }
        }
    }
