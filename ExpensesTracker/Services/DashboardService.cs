using AutoMapper;
using ExpensesTracker.Models.Dashboard;
using ExpensesTracker.Repositories;

namespace ExpensesTracker.Services
{
    public class DashboardService : IDashboardService
    {
        private const int DashboardMonthsCount = 6;
        private readonly IMapper mapper;
        private readonly ITransactionRepository transactionRepository;

        public DashboardService(
            IMapper mapper,
            ITransactionRepository transactionRepository)
        {
            this.mapper = mapper;
            this.transactionRepository = transactionRepository;
        }

        public async Task<DashboardSummaryDto> GetSummaryAsync(DashboardFilterDto filters)
        {
            var today = DateTime.Today;
            var requestYear = filters.Year ?? today.Year;
            var requestMonth = filters.Month ?? today.Month;

            var currentMonthStart = new DateTime(requestYear, requestMonth, 1);
            var nextMonthStart = currentMonthStart.AddMonths(1);
            var previousMonthStart = currentMonthStart.AddMonths(-1);

            var currentMonthTotalExpenses = await this.transactionRepository.GetTotalAsync(currentMonthStart, nextMonthStart, TransactionType.Expense);
            var currentMonthTotalIncomes = await this.transactionRepository.GetTotalAsync(currentMonthStart, nextMonthStart, TransactionType.Income);

            var previousMonthTotalExpenses = await this.transactionRepository.GetTotalAsync(previousMonthStart, currentMonthStart, TransactionType.Expense);
            var previousMonthTotalIncomes = await this.transactionRepository.GetTotalAsync(previousMonthStart, currentMonthStart, TransactionType.Income);

            var currentMonthBalance = currentMonthTotalIncomes - currentMonthTotalExpenses;

            decimal? differenceExpensePercentage =
                previousMonthTotalExpenses != 0
                    ? Math.Round((currentMonthTotalExpenses - previousMonthTotalExpenses)
                        / previousMonthTotalExpenses
                        * 100, 2)
                    : null;

            decimal? differenceIncomePercentage =
                previousMonthTotalIncomes != 0
                    ? Math.Round((currentMonthTotalIncomes - previousMonthTotalIncomes)
                        / previousMonthTotalIncomes
                        * 100, 2)
                    : null;

            var categoryTotalsDto =
                this.mapper.Map<IEnumerable<CategoryTotalDto>>(
                    await this.transactionRepository.GetTotalsByCategoryAsync(
                        currentMonthStart,
                        nextMonthStart,
                        TransactionType.Expense));

            var sixMonthsBeforeStart = currentMonthStart.AddMonths(-5);

            var monthlyTotalsDto =
                this.mapper.Map<IEnumerable<MonthlyTotalDto>>(
                    await this.transactionRepository.GetMonthlyTotalsAsync(
                        sixMonthsBeforeStart,
                        nextMonthStart,
                        TransactionType.Expense));

            var completeMonthlyTotals = Enumerable
                .Range(0, DashboardMonthsCount)
                .Select(i =>
                {
                    var monthDate = sixMonthsBeforeStart.AddMonths(i);

                    var existingMonth = monthlyTotalsDto.FirstOrDefault(x =>
                        x.Year == monthDate.Year &&
                        x.Month == monthDate.Month);

                    return new MonthlyTotalDto
                    {
                        Year = monthDate.Year,
                        Month = monthDate.Month,
                        Total = existingMonth?.Total ?? 0
                    };
                })
                .ToList();

            return new DashboardSummaryDto
            {
                CurrentMonthTotalExpenses = currentMonthTotalExpenses,
                CurrentMonthTotalIncomes = currentMonthTotalIncomes,
                CurrentMonthBalance = currentMonthBalance,
                PreviousMonthTotalExpenses = previousMonthTotalExpenses,
                DifferenceExpensePercentage = differenceExpensePercentage,
                DifferenceIncomePercentage = differenceIncomePercentage,
                CategoryTotals = categoryTotalsDto,
                MonthlyTotals = completeMonthlyTotals
            };
        }
    }
}
