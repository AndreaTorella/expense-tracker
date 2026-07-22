using ExpensesTracker.Data;
using ExpensesTracker.Entities;
using ExpensesTracker.Models;
using ExpensesTracker.Models.Enums;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ExpensesTracker.Repositories
{
    public class ExpenseRepository : IExpenseRepository
    {
        private readonly ExpenseTrackerDbContext context;

        public ExpenseRepository(ExpenseTrackerDbContext expenseTrackerDbContext)
        {
            this.context = expenseTrackerDbContext ?? throw new ArgumentNullException(nameof(expenseTrackerDbContext));
        }
        public async Task<PagedResult<Expense>> GetExpensesAsync(ExpenseFilterDto filters)
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

            var totalItems = await query.CountAsync();

            query = ApplySorting(query, filters);

            var items = await query
                .Skip((filters.PageNumber - 1) * filters.PageSize)
                .Take(filters.PageSize)
                .ToListAsync();

            return new PagedResult<Expense>()
            {
                Items = items,
                TotalItems = totalItems,
            };
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

        private static IQueryable<Expense> ApplySorting(
            IQueryable<Expense> query,
            ExpenseFilterDto filters)
        {
            return filters.ExpenseSortBy switch
            {
                ExpenseSortBy.Date =>
                    ApplyOrder(query, x => x.Date, filters.SortDirection),

                ExpenseSortBy.Amount =>
                    ApplyOrder(query, x => x.Amount, filters.SortDirection),

                ExpenseSortBy.Title =>
                    ApplyOrder(query, x => x.Title, filters.SortDirection),

                ExpenseSortBy.Category =>
                    ApplyOrder(query, x => x.Category.Name, filters.SortDirection),

                ExpenseSortBy.PaymentMethod =>
                    ApplyOrder(
                        query,
                        x => x.PaymentMethod.Name,
                        filters.SortDirection),

                _ =>
                    query
                        .OrderByDescending(x => x.Date)
                        .ThenByDescending(x => x.Id)
            };
        }

        private static IQueryable<Expense> ApplyOrder<TKey>(
            IQueryable<Expense> query,
            Expression<Func<Expense, TKey>> orderBy,
            SortDirection direction)
        {
            return direction == SortDirection.Asc
                ? query
                    .OrderBy(orderBy)
                    .ThenByDescending(x => x.Date)
                    .ThenByDescending(x => x.Id)
                : query
                    .OrderByDescending(orderBy)
                    .ThenByDescending(x => x.Date)
                    .ThenByDescending(x => x.Id);
        }
    }
}
