using ExpensesTracker.Domain.Enums;

namespace ExpensesTracker.Application.Models
{
    public class CreateCategoryCommand
    {
        public string Name { get; set; } = string.Empty;
        public TransactionType TransactionType { get; set; }
    }
}
