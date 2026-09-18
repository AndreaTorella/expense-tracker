using ExpensesTracker.Application.Repositories;
using ExpensesTracker.Domain.Entities;
using ExpensesTracker.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ExpensesTracker.Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ExpenseTrackerDbContext context;

        public CategoryRepository(ExpenseTrackerDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync(int householdId)
        {
            return await context.Categories
                .Where(x => x.HouseholdId == householdId)
                .ToListAsync();
        }

        public async Task<Category?> GetCategoryByIdAsync(int id, int householdId)
        {
            return await context.Categories
                .Where(x => x.HouseholdId == householdId)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task AddCategoryAsync(Category category)
        {
            if (category == null)
            {
                throw new ArgumentNullException(nameof(category));
            }

            await context.Categories.AddAsync(category);
        }

        public void DeleteCategoryAsync(Category category)
        {
            if (category == null)
            {
                throw new ArgumentNullException(nameof(category));
            }

            context.Categories.Remove(category);
        }

        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}
