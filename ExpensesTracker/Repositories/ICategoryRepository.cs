using ExpensesTracker.Entities;

namespace ExpensesTracker.Repositories
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllCategoriesAsync(int householdId);
        Task<Category?> GetCategoryByIdAsync(int id, int householdId);
        Task AddCategoryAsync(Category category);
        void DeleteCategoryAsync(Category category);
        Task SaveChangesAsync();
    }
}