using ExpensesTracker.Data;
using ExpensesTracker.Entities;
using ExpensesTracker.Models;
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
        public async Task<IEnumerable<Expense>> GetExpensesAsync(ExpenseFilterDto filters)
        {
            IQueryable<Expense> query = this.context.Expenses
                .AsNoTracking()
                .Include(x => x.Category)
                .Include(x => x.PaymentMethod);

            if (!string.IsNullOrWhiteSpace(filters.Search))
            {
                var searchText = filters.Search.Trim();

                query = query.Where(x => x.Title.Contains(searchText));
            }

            if (filters.FromDate is DateTime fromDate)
            {
                query = query.Where(x => x.Date >= fromDate.Date);
            }

            if (filters.ToDate is DateTime toDate)
            {
                var endDateExclusive = toDate.Date.AddDays(1);

                query = query.Where(x =>
                    x.Date < endDateExclusive);
            }

            if (filters.CategoryId is int categoryId)
            {
                query = query.Where(x => x.CategoryId == categoryId);
            }

            if (filters.PaymentMethodId is int paymentMethodId)
            {
                query = query.Where(x => x.PaymentMethodId == paymentMethodId);
            }

            return await query
                .OrderByDescending(x => x.Date)
                .ThenByDescending(x => x.Id)
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
