using AutoMapper;
using ExpensesTracker.Application.Models;
using ExpensesTracker.Application.Repositories;
using ExpensesTracker.Domain.Entities;

namespace ExpensesTracker.Application.Services
{
    public class HouseholdService : IHouseholdService
    {
        private readonly IMapper mapper;
        private readonly IHouseholdRepository householdRepository;

        public HouseholdService(
            IMapper mapper,
            IHouseholdRepository householdRepository)
        {
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.householdRepository = householdRepository ?? throw new ArgumentNullException(nameof(householdRepository));
        }

        public async Task<HouseholdResult> AddHouseholdAsync(CreateHouseholdCommand createHouseholdCommand)
        {
            var household = this.mapper.Map<Household>(createHouseholdCommand);
            await this.householdRepository.AddHouseholdAsync(household);
            await this.householdRepository.SaveChangesAsync();

            return this.mapper.Map<HouseholdResult>(household);
        }
    }
}