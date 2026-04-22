using ExpensesTracker.Entities;

namespace ExpensesTracker.Repositories
{
public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task<Category?> GetCategoryByIdAsync(int id);
        Task AddCategoryAsync(Category category);
        void DeleteCategoryAsync(Category category);
        Task SaveChangesAsync();
    }
}