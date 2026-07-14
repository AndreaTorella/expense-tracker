using ExpensesTracker.Data;
using ExpensesTracker.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExpensesTracker.Repositories
{
    public class ExpenseRepository : IExpenseRepository
    {
        private readonly ExpenseTrackerDbContext context;

        public ExpenseRepository(ExpenseTrackerDbContext expenseTrackerDbContext)
        {
            this.context = expenseTrackerDbContext ?? throw new ArgumentNullException(nameof(expenseTrackerDbContext));
        }
        public async Task<IEnumerable<Expense>> GetAllExpensesAsync()
        {
            return await this.context.Expenses
            .Include(x => x.Category)
            .Include(x => x.PaymentMethod)
            .OrderByDescending(x => x.Date)
            .ToListAsync();
        }

        public async Task<Expense?> GetExpenseByIdAsync(int id)
        {
            IQueryable<Expense> query = this.context.Expenses;

            return await query
                .Include(x => x.Category)
                .Include(x => x.PaymentMethod)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task AddExpenseAsync(Expense expense)
        {
            if (expense == null)
            {
                throw new ArgumentNullException(nameof(expense));
            }

            await this.context.Expenses.AddAsync(expense);
        }

        public void DeleteExpenseAsync(Expense expense)
        {
            if (expense == null)
            {
                throw new ArgumentNullException(nameof(expense));
            }

            this.context.Expenses.Remove(expense);
        }

        public async Task SaveChangesAsync()
        {
            await this.context.SaveChangesAsync();
        }
    }
}
