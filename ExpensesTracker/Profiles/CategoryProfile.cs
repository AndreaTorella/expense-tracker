using AutoMapper;
using ExpensesTracker.Domain.Entities;

namespace ExpensesTracker.Profiles
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            this.CreateMap<Category, Models.CategoryDto>();
            this.CreateMap<Models.CategoryDto, Category>();
        }
    }
}
