using ExpensesTracker.Data;
using ExpensesTracker.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExpensesTracker.Repositories
{
    public class ExpenceRepository : IExpenceRepository
    {
        private readonly ExpenseTrackerDbContext context;

        public ExpenceRepository(ExpenseTrackerDbContext expenseTrackerDbContext)
        {
            this.context = expenseTrackerDbContext ?? throw new ArgumentNullException(nameof(expenseTrackerDbContext));
        }
        public async Task<IEnumerable<Expense>> GetAllExpensesAsync()
        {
            return await this.context.Expenses.OrderBy(x => x.Date).ToListAsync();
        }
        public async Task<Expense?> GetExpenseByIdAsync(int id, bool includeCategory, bool includePaymentMethod)
        {
            IQueryable<Expense> query = this.context.Expenses;

            if (includeCategory)
            {
                query = query.Include(x => x.Category);
            }

            if (includePaymentMethod)
            {
                query = query.Include(x => x.PaymentMethod);
            }

            return await query.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task AddExpenseAsync(Expense expense)
        {
            if(expense != null)
            {
                await this.context.Expenses.AddAsync(expense);
            }
        }

        public async Task DeleteExpenseByIdAsync(int id)
        {
            var expense = await this.GetExpenseByIdAsync(id, false, false);

            if(expense != null)
            {
                await this.context.Expenses.Remove(expense);
            }
        }

        public Task<bool> SaveChangesAsync()
        {
            throw new NotImplementedException();
        }
    }
}
