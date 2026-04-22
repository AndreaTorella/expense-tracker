using ExpensesTracker.Data;
using ExpensesTracker.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExpensesTracker.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ExpenseTrackerDbContext context;

        public CategoryRepository(ExpenseTrackerDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await this.context.Category.ToListAsync();
        }

        public async Task<Category?> GetCategoryByIdAsync(int id)
        {
            return await this.context.Category.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task AddCategoryAsync(Category category)
        {
            if(category == null)
            {
                throw new ArgumentNullException(nameof(category));
            }

            await this.context.Category.AddAsync(category);
        }

        public void DeleteCategoryAsync(Category category)
        {
            if(category == null)
            {
                throw new ArgumentNullException(nameof(category));
            }

            this.context.Category.Remove(category);
        }

        public async Task SaveChangesAsync()
        {
            await this.context.SaveChangesAsync();
        }
    }
}
