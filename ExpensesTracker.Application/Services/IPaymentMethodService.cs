using ExpensesTracker.Application.Models;

namespace ExpensesTracker.Application.Services
{
    public interface IPaymentMethodService
    {
        Task<IEnumerable<PaymentMethodResult>> GetAllPaymentMethodsAsync();
        Task<PaymentMethodResult?> GetPaymentMethodByIdAsync(int paymentMethodId);
        Task<PaymentMethodResult> AddPaymentMethodAsync(CreatePaymentMethodCommand paymentMethodDto);
        Task<bool> DeletePaymentMethodAsync(int paymentMethodId);
    }
}