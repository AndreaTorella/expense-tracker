using AutoMapper;
using ExpensesTracker.Entities;

namespace ExpensesTracker.Profiles
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            this.CreateMap<Entities.Category, Models.CategoryDto>();
            this.CreateMap<Models.CategoryDto, Entities.Category>();
        }
    }
}
