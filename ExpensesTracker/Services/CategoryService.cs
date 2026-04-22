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
            var categoryEntities = await this.categoryRepository.GetAllCategoriesAsync();
            return this.mapper.Map<IEnumerable<CategoryDto>>(categoryEntities);
        }

        public async Task<CategoryDto?> GetCategoryByIdAsync(int categoryId)
        {
            var categoryEntity = await this.categoryRepository.GetCategoryByIdAsync(categoryId);

            if(categoryEntity == null)
            {
                return null;
            }

            return this.mapper.Map<CategoryDto>(categoryEntity);
        }

        public async Task AddCategoryAsync(CategoryDto categoryDto)
        {
            if(categoryDto == null)
            {
                throw new ArgumentNullException(nameof(categoryDto));
            }
            
            var categoryEntity = this.mapper.Map<Category>(categoryDto);
            await this.categoryRepository.AddCategoryAsync(categoryEntity);
            await this.categoryRepository.SaveChangesAsync();
        }

        public async Task<bool> DeleteCategoryAsync(int categoryId)
        {
            var categoryEntity = await this.categoryRepository.GetCategoryByIdAsync(categoryId);

            if (categoryEntity == null)
            {
                return false;
            }

            this.categoryRepository.DeleteCategoryAsync(categoryEntity);
            await this.categoryRepository.SaveChangesAsync();
            return true;
        }
    }
}