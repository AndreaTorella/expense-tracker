using ExpensesTracker.Models;

namespace ExpensesTracker.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync();
        Task<CategoryDto?> GetCategoryByIdAsync(int categoryId);
        Task AddCategoryAsync(CategoryDto categoryDto);
        Task<bool> DeleteCategoryAsync(int categoryId);
    }
}