using AutoMapper;
using ExpensesTracker.Application.Models;
using ExpensesTracker.Domain.Entities;

namespace ExpensesTracker.Application.Profiles
{
    public class HouseholdProfile : Profile
    {
        public HouseholdProfile()
        {
            this.CreateMap<Household, HouseholdResult>();
            this.CreateMap<CreateHouseholdCommand, Household>();
        }
    }
}
