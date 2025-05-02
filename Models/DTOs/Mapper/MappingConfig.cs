using AutoMapper;
using Models.Domain;
using Models.DTOs.Food;
using Models.DTOs.User;

namespace Models.DTOs.Mapper
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            // Mapping between ApplicationUser and UserDTO
            CreateMap<ApplicationUser, UserDTO>().ReverseMap();

            CreateMap<Recipe, RecipeWithNutritionDTO>()
     .ForMember(dest => dest.RecipeId, opt => opt.MapFrom(src => src.Recipe_Id))
     .ForMember(dest => dest.Recipe_Name, opt => opt.MapFrom(src => src.Recipe_Name))
     .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
     .ForMember(dest => dest.Preparation_Method, opt => opt.MapFrom(src => src.Preparation_Method))
     .ForMember(dest => dest.Time, opt => opt.MapFrom(src => src.Time))
     .ForMember(dest => dest.Calories_100g, opt => opt.MapFrom(src => src.Nutrition.Calories_100g))
     .ForMember(dest => dest.Fat_100g, opt => opt.MapFrom(src => src.Nutrition.Fat_100g))
     .ForMember(dest => dest.Sugar_100g, opt => opt.MapFrom(src => src.Nutrition.Sugar_100g))
     .ForMember(dest => dest.Protein_100g, opt => opt.MapFrom(src => src.Nutrition.Protein_100g))
     .ForMember(dest => dest.Carb_100, opt => opt.MapFrom(src => src.Nutrition.Carb_100))
     .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Nutrition.Type)) // 👈 أضف السطر ده
     .ForMember(dest => dest.IngredientNames,
         opt => opt.MapFrom(src => src.Recipe_Ingredient
                                      .Select(ri => ri.Ingredient.Ingredient_Name ?? "Unknown")
                                      .ToList()));


        }
    }
}
