using AutoMapper;
using ExpensesTracker.Entities;
using ExpensesTracker.Models;
using ExpensesTracker.Repositories;

namespace ExpensesTracker.Services
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

        public async Task<HouseholdDto> AddHouseholdAsync(CreateHouseholdDto createHouseholdDto)
        {
            var household = this.mapper.Map<Household>(createHouseholdDto);
            await this.householdRepository.AddHouseholdAsync(household);
            await this.householdRepository.SaveChangesAsync();

            return this.mapper.Map<HouseholdDto>(household);
        }
    }
}