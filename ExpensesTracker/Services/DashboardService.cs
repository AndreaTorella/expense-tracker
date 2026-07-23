using AutoMapper;
using ExpensesTracker.Models.Dashboard;
using ExpensesTracker.Repositories;

namespace ExpensesTracker.Services
{
    public class DashboardService : IDashboardService
    {
        private const int DashboardMonthsCount = 6;
        private readonly IMapper mapper;
        private readonly IExpenseRepository expenseRepository;

        public DashboardService(
            IMapper mapper,
            IExpenseRepository expenseRepository)
        {
            this.mapper = mapper;
            this.expenseRepository = expenseRepository;
        }

        public async Task<DashboardSummaryDto> GetSummaryAsync(DashboardFilterDto filters)
        {
            var today = DateTime.Today;
            var requestYear = filters.Year ?? today.Year;
            var requestMonth = filters.Month ?? today.Month;

            var currentMonthStart = new DateTime(requestYear, requestMonth, 1);
            var nextMonthStart = currentMonthStart.AddMonths(1);
            var previousMonthStart = currentMonthStart.AddMonths(-1);

            var currentMonthTotal = await this.expenseRepository.GetTotalAsync(currentMonthStart, nextMonthStart);
            var previousMonthTotal = await this.expenseRepository.GetTotalAsync(previousMonthStart, currentMonthStart);

            decimal? differencePercentage =
                previousMonthTotal != 0
                    ? Math.Round((currentMonthTotal - previousMonthTotal)
                        / previousMonthTotal
                        * 100, 2)
                    : null;

            var categoryTotalsDto =
                this.mapper.Map<IEnumerable<CategoryTotalDto>>(
                    await this.expenseRepository.GetTotalsByCategoryAsync(currentMonthStart, nextMonthStart));

            var sixMonthsBeforeStart = currentMonthStart.AddMonths(-5);

            var monthlyTotalsDto =
                this.mapper.Map<IEnumerable<MonthlyTotalDto>>(
                    await this.expenseRepository.GetMonthlyTotalsAsync(sixMonthsBeforeStart, nextMonthStart));

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
                CurrentMonthTotal = currentMonthTotal,
                PreviousMonthTotal = previousMonthTotal,
                DifferencePercentage = differencePercentage,
                CategoryTotals = categoryTotalsDto,
                MonthlyTotals = completeMonthlyTotals
            };
        }
    }
}
