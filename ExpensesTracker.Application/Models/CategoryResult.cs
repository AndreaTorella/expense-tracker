using ExpensesTracker.Domain.Enums;

namespace ExpensesTracker.Application.Models
{
    public class CategoryResult
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public TransactionType TransactionType { get; set; }
    }
}
