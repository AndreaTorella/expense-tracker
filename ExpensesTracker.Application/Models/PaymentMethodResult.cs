using ExpensesTracker.Domain.Enums;

namespace ExpensesTracker.Application.Models
{
    public class PaymentMethodResult
    {
        public int Id { get; set; }
        public PaymentMethodName Name { get; set; }
    }
}
