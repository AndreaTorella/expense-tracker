using ExpensesTracker.Application.Common;
using ExpensesTracker.Application.Enums;
using ExpensesTracker.Application.Models;
using ExpensesTracker.Application.Repositories;
using ExpensesTracker.Domain.Entities;
using ExpensesTracker.Domain.Enums;
using ExpensesTracker.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ExpensesTracker.Infrastructure.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly ExpenseTrackerDbContext context;

        public TransactionRepository(ExpenseTrackerDbContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<PagedResult<Transaction>> GetTransactionAsync(TransactionQuery filters, int houseHoldId)
        {
            IQueryable<Transaction> query = this.context.Transactions
                .AsNoTracking()
                .Include(x => x.Category)
                .Include(x => x.PaymentMethod);

            query = query.Where(x => x.HouseholdId == houseHoldId);

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

            if (filters.TransactionType.HasValue)
            {
                query = query.Where(x => x.TransactionType == filters.TransactionType.Value);
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

            return new PagedResult<Transaction>()
            {
                Items = items,
                TotalItems = totalItems,
            };
        }

        public async Task<Transaction?> GetTransactionByIdAsync(int id, int householdId)
        {
            IQueryable<Transaction> query = this.context.Transactions;

            return await query
                .Include(x => x.Category)
                .Include(x => x.PaymentMethod)
                .Where(x => x.HouseholdId == householdId)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task AddTransactionAsync(Transaction transaction)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException(nameof(transaction));
            }

            await this.context.Transactions.AddAsync(transaction);
        }

        public void DeleteTransactionAsync(Transaction transaction)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException(nameof(transaction));
            }

            this.context.Transactions.Remove(transaction);
        }

        public async Task SaveChangesAsync()
        {
            await this.context.SaveChangesAsync();
        }

        public Task<decimal> GetTotalAsync(DateTime fromDate, DateTime toDate, TransactionType transactionType, int householdId)
        {
            return this.context.Transactions
                .Where(x =>
                    x.HouseholdId == householdId &&
                    x.Date >= fromDate &&
                    x.Date < toDate &&
                    x.TransactionType == transactionType)
                .SumAsync(x => x.Amount);
        }

        public async Task<IEnumerable<CategoryTotal>> GetTotalsByCategoryAsync(
            DateTime fromDate,
            DateTime toDate,
            TransactionType transactionType,
            int householdId)
        {
            var transactionsByCategory = await this.context.Transactions
                .Where(x =>
                    x.HouseholdId == householdId &&
                    x.Date >= fromDate &&
                    x.Date < toDate &&
                    x.TransactionType == transactionType)
                .GroupBy(x => new
                {
                    x.CategoryId,
                    x.Category.Name
                })
                .Select(group => new CategoryTotal
                {
                    CategoryId = group.Key.CategoryId,
                    CategoryName = group.Key.Name,
                    Total = group.Sum(x => x.Amount)
                })
                .OrderByDescending(x => x.Total)
                .ToListAsync();

            return transactionsByCategory;
        }

        public async Task<IEnumerable<MonthlyTotal>> GetMonthlyTotalsAsync(
            DateTime fromDate,
            DateTime toDate,
            TransactionType transactionType,
            int householdId)
        {
            var transactionsByMonth = await this.context.Transactions
                .Where(x =>
                    x.HouseholdId == householdId &&
                    x.Date >= fromDate &&
                    x.Date < toDate &&
                    x.TransactionType == transactionType)
                .GroupBy(x => new
                {
                    x.Date.Month,
                    x.Date.Year,
                })
                .Select(group => new MonthlyTotal
                {
                    Month = group.Key.Month,
                    Year = group.Key.Year,
                    Total = group.Sum(x => x.Amount)
                })
                .OrderBy(x => x.Year)
                .ThenBy(x => x.Month)
                .ToListAsync();

            return transactionsByMonth;
        }

        private static IQueryable<Transaction> ApplySorting(
            IQueryable<Transaction> query,
            TransactionQuery filters)
        {
            return filters.TransactionSortBy switch
            {
                TransactionSortBy.Date =>
                    ApplyOrder(query, x => x.Date, filters.SortDirection),

                TransactionSortBy.Amount =>
                    ApplyOrder(query, x => x.Amount, filters.SortDirection),

                TransactionSortBy.Title =>
                    ApplyOrder(query, x => x.Title, filters.SortDirection),

                TransactionSortBy.Category =>
                    ApplyOrder(query, x => x.Category.Name, filters.SortDirection),

                TransactionSortBy.PaymentMethod =>
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

        private static IQueryable<Transaction> ApplyOrder<TKey>(
            IQueryable<Transaction> query,
            Expression<Func<Transaction, TKey>> orderBy,
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
