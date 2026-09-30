using ExpensesTracker.Application.Models;

namespace ExpensesTracker.Application.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryResult>> GetAllCategoriesAsync();
        Task<CategoryResult?> GetCategoryByIdAsync(int categoryId);
        Task<CategoryResult> AddCategoryAsync(CreateCategoryCommand command);
        Task<bool> DeleteCategoryAsync(int categoryId);
    }
}