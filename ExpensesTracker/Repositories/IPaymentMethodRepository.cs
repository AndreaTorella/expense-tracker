using ExpensesTracker.Entities;

namespace ExpensesTracker.Repositories
{
public interface IPaymentMethodRepository
    {
        Task<IEnumerable<PaymentMethod>> GetAllPaymentMethodsAsync();
        Task<PaymentMethod?> GetPaymentMethodByIdAsync(int paymentMethodId);
        Task AddPaymentMethodAsync(PaymentMethod paymentMethod);
        void DeletePaymentMethodAsync(PaymentMethod paymentMethod);
        Task SaveChangesAsync();
    }
}