using ExpensesTracker.Models;

namespace ExpensesTracker.Services
{
    public interface IPaymentMethodService
    {
        Task<IEnumerable<PaymentMethodDto>> GetAllPaymentMethodsAsync();
        Task<PaymentMethodDto?> GetPaymentMethodByIdAsync(int paymentMethodId);
        Task<PaymentMethodDto> AddPaymentMethodAsync(PaymentMethodDto paymentMethodDto);
        Task<bool> DeletePaymentMethodAsync(int paymentMethodId);
    }
}