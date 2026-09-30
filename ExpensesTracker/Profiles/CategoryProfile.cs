using AutoMapper;
using ExpensesTracker.Application.Models;
using ExpensesTracker.Models;

namespace ExpensesTracker.Profiles
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            this.CreateMap<CategoryDto, CreateCategoryCommand>();
            this.CreateMap<CategoryResult, CategoryDto>();
        }
    }
}
