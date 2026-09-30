using AutoMapper;
using ExpensesTracker.Application.Models;
using ExpensesTracker.Domain.Entities;

namespace ExpensesTracker.Application.Profiles
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<CreateCategoryCommand, Category>();
            CreateMap<Category, CategoryResult>();
        }
    }
}
