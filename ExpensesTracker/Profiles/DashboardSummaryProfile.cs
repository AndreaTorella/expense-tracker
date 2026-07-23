using AutoMapper;
using ExpensesTracker.Models.Common;
using ExpensesTracker.Models.Dashboard;

namespace ExpensesTracker.Profiles
{
    public class DashboardSummaryProfile : Profile
    {
        public DashboardSummaryProfile()
        {
            this.CreateMap<CategoryTotal, CategoryTotalDto>();
            this.CreateMap<CategoryTotalDto, CategoryTotal>();

            this.CreateMap<MonthlyTotal, MonthlyTotalDto>();
            this.CreateMap<MonthlyTotalDto, MonthlyTotal>();
        }
    }
}
