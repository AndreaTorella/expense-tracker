using AutoMapper;
using ExpensesTracker.Application.Repositories;
using ExpensesTracker.Application.Services;
using ExpensesTracker.Domain.Enums;
using ExpensesTracker.Models.Dashboard;

namespace ExpensesTracker.Services
{
    public class DashboardService : IDashboardService
    {
        private const int DashboardMonthsCount = 6;
        private readonly IMapper mapper;
        private readonly ITransactionRepository transactionRepository;
        private readonly ICurrentUserService currentUserService;

        public DashboardService(
            IMapper mapper,
            ICurrentUserService currentUserService,
            ITransactionRepository transactionRepository)
        {
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
            this.transactionRepository = transactionRepository ?? throw new ArgumentNullException(nameof(transactionRepository));
        }

        public async Task<DashboardSummaryDto> GetSummaryAsync(DashboardFilterDto filters)
        {
            var today = DateTime.Today;
            var requestYear = filters.Year ?? today.Year;
            var requestMonth = filters.Month ?? today.Month;

            var currentMonthStart = new DateTime(requestYear, requestMonth, 1);
            var nextMonthStart = currentMonthStart.AddMonths(1);
            var previousMonthStart = currentMonthStart.AddMonths(-1);

            var householdId = await this.currentUserService.GetHouseholdIdAsync();

            var currentMonthTotalExpenses = await this.transactionRepository.GetTotalAsync(currentMonthStart, nextMonthStart, TransactionType.Expense, householdId);
            var currentMonthTotalIncomes = await this.transactionRepository.GetTotalAsync(currentMonthStart, nextMonthStart, TransactionType.Income, householdId);

            var previousMonthTotalExpenses = await this.transactionRepository.GetTotalAsync(previousMonthStart, currentMonthStart, TransactionType.Expense, householdId);
            var previousMonthTotalIncomes = await this.transactionRepository.GetTotalAsync(previousMonthStart, currentMonthStart, TransactionType.Income, householdId);

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
                        TransactionType.Expense,
                        householdId));

            var sixMonthsBeforeStart = currentMonthStart.AddMonths(-5);

            var monthlyTotalsDto =
                this.mapper.Map<IEnumerable<MonthlyTotalDto>>(
                    await this.transactionRepository.GetMonthlyTotalsAsync(
                        sixMonthsBeforeStart,
                        nextMonthStart,
                        TransactionType.Expense,
                        householdId));

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
