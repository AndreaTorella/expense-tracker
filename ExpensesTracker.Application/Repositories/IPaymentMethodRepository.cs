using ExpensesTracker.Domain.Entities;

namespace ExpensesTracker.Application.Repositories
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