using AutoMapper;
using Models.Domain;
using Models.DTOs.Food;
using Models.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs.Mapper
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            // Mapping between ApplicationUser and UserDTO
            CreateMap<ApplicationUser, UserDTO>().ReverseMap();

            // Mapping between Recipe and RecipeWithFullNutritionDTO
            CreateMap<Recipe, RecipeWithNutritionDTO>()
                .ForMember(dest => dest.Calories_100g, opt => opt.MapFrom(src => src.Nutrition.Calories_100g))
                .ForMember(dest => dest.Fat_100g, opt => opt.MapFrom(src => src.Nutrition.Fat_100g))
                .ForMember(dest => dest.Sugar_100g, opt => opt.MapFrom(src => src.Nutrition.Sugar_100g))
                .ForMember(dest => dest.Protein_100g, opt => opt.MapFrom(src => src.Nutrition.Protein_100g))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Nutrition.Type));
        }
    }
}
