using AutoMapper;
using ExpensesTracker.Application.Models;
using ExpensesTracker.Models.Dashboard;

namespace ExpensesTracker.Profiles
{
    public class DashboardSummaryProfile : Profile
    {
        public DashboardSummaryProfile()
        {
            this.CreateMap<DashboardFilterDto, DashboardQuery>();

            this.CreateMap<CategoryTotal, CategoryTotalDto>();
            this.CreateMap<MonthlyTotal, MonthlyTotalDto>();

            this.CreateMap<DashboardSummaryResult, DashboardSummaryDto>();
        }
    }
}
