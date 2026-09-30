using AutoMapper;
using ExpensesTracker.Application.Models;
using ExpensesTracker.Application.Repositories;
using ExpensesTracker.Application.Services;
using ExpensesTracker.Domain.Entities;

namespace ExpensesTracker.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository categoryRepository;
        private readonly IMapper mapper;
        private readonly ICurrentUserService currentUserService;

        public CategoryService(
            ICategoryRepository categoryRepository,
            IMapper mapper,
            ICurrentUserService currentUserService)
        {
            this.categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
        }

        public async Task<IEnumerable<CategoryResult>> GetAllCategoriesAsync()
        {
            var householdId = await this.currentUserService.GetHouseholdIdAsync();
            var categoryEntities = await categoryRepository.GetAllCategoriesAsync(householdId);
            return mapper.Map<IEnumerable<CategoryResult>>(categoryEntities);
        }

        public async Task<CategoryResult?> GetCategoryByIdAsync(int categoryId)
        {
            var householdId = await this.currentUserService.GetHouseholdIdAsync();
            var categoryEntity = await categoryRepository.GetCategoryByIdAsync(categoryId, householdId);

            if (categoryEntity == null)
            {
                return null;
            }

            return mapper.Map<CategoryResult>(categoryEntity);
        }

        public async Task<CategoryResult> AddCategoryAsync(CreateCategoryCommand categoryDto)
        {
            ArgumentNullException.ThrowIfNull(categoryDto);

            var householdId = await this.currentUserService.GetHouseholdIdAsync();
            var categoryEntity = mapper.Map<Category>(categoryDto);

            categoryEntity.HouseholdId = householdId;

            await categoryRepository.AddCategoryAsync(categoryEntity);
            await categoryRepository.SaveChangesAsync();

            return mapper.Map<CategoryResult>(categoryEntity);
        }

        public async Task<bool> DeleteCategoryAsync(int categoryId)
        {
            var householdId = await this.currentUserService.GetHouseholdIdAsync();
            var categoryEntity = await categoryRepository.GetCategoryByIdAsync(categoryId, householdId);

            if (categoryEntity == null)
            {
                return false;
            }

            categoryRepository.DeleteCategoryAsync(categoryEntity);
            await categoryRepository.SaveChangesAsync();
            return true;
        }
    }
}