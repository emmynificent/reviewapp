using AutoMapper;
using reviewapp.Dto;
using reviewapp.Model;

namespace reviewapp.Helper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Pokemon, PokemonDto>();
            CreateMap<PokemonDto, Pokemon>();
            CreateMap<Category, CategoryDto>();
            CreateMap<CategoryDto, Category>();
            CreateMap<Review, ReviewDto>(); 
            CreateMap<ReviewDto, Review>(); 
          //  CreateMap<Owner, OwnerDto>();
            CreateMap<OwnerDto, Owner>();
            CreateMap<ReviewerDto, Reviewer>();
            CreateMap<CountryDto, Country>();
        }
    }
}
