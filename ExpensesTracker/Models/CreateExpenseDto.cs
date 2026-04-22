using ExpensesTracker.Entities;

namespace ExpensesTracker.Models
{
    public class CreateExpenseDto
    {
        public string Title { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public int CategoryId { get; set; }
        public int PaymentMethodId { get; set; }
    }
}
