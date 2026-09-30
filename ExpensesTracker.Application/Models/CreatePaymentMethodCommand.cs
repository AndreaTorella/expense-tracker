using ExpensesTracker.Domain.Enums;

namespace ExpensesTracker.Application.Models
{
    public class CreatePaymentMethodCommand
    {
        public PaymentMethodName Name { get; set; }
    }
}
