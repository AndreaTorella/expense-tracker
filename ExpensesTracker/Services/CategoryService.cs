using AutoMapper;
using ExpensesTracker.Entities;
using ExpensesTracker.Models;
using ExpensesTracker.Repositories;

namespace ExpensesTracker.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository categoryRepository;
        private readonly IMapper mapper;

        public CategoryService(
            ICategoryRepository categoryRepository,
            IMapper mapper)
        {
            this.categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
        {
            var categoryEntities = await categoryRepository.GetAllCategoriesAsync();
            return mapper.Map<IEnumerable<CategoryDto>>(categoryEntities);
        }

        public async Task<CategoryDto?> GetCategoryByIdAsync(int categoryId)
        {
            var categoryEntity = await categoryRepository.GetCategoryByIdAsync(categoryId);

            if (categoryEntity == null)
            {
                return null;
            }

            return mapper.Map<CategoryDto>(categoryEntity);
        }

        public async Task<CategoryDto> AddCategoryAsync(CategoryDto categoryDto)
        {
            if (categoryDto == null)
            {
                throw new ArgumentNullException(nameof(categoryDto));
            }

            var categoryEntity = mapper.Map<Category>(categoryDto);
            await categoryRepository.AddCategoryAsync(categoryEntity);
            await categoryRepository.SaveChangesAsync();

            return mapper.Map<CategoryDto>(categoryEntity);
        }

        public async Task<bool> DeleteCategoryAsync(int categoryId)
        {
            var categoryEntity = await categoryRepository.GetCategoryByIdAsync(categoryId);

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