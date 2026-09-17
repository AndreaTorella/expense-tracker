using AutoMapper;
using ExpensesTracker.Domain.Entities;
using ExpensesTracker.Models;

namespace ExpensesTracker.Profiles
{
    public class HouseholdProfile : Profile
    {
        public HouseholdProfile()
        {
            this.CreateMap<CreateHouseholdDto, Household>();
            this.CreateMap<Household, HouseholdDto>();
        }
    }
}
