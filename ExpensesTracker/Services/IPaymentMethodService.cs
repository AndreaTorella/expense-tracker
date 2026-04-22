using ExpensesTracker.Models;

namespace ExpensesTracker.Services
{
    public interface IPaymentMethodService
    {
        Task<IEnumerable<PaymentMethodDto>> GetAllPaymentMethodAsync();
        Task<PaymentMethodDto?> GetPaymentMethodByIdAsync(int paymentMethodId);
        Task AddPaymentMethodAsync(PaymentMethodDto paymentMethodDto);
        Task<bool> DeletePaymentMethodAsync(int paymentMethodId);
    }
}