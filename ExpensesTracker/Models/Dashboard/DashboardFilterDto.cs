using System.ComponentModel.DataAnnotations;

namespace ExpensesTracker.Models.Dashboard
{
    public class DashboardFilterDto
    {
        [Range(2000, 2100)]
        public int? Year { get; set; }

        [Range(1, 12)]
        public int? Month { get; set; }
    }
}
