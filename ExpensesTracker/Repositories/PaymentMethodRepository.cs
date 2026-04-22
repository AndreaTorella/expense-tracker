using ExpensesTracker.Data;
using ExpensesTracker.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExpensesTracker.Repositories
{
    public class PaymentMethodRepository : IPaymentMethodRepository
    {
        private readonly ExpenseTrackerDbContext context;

        public PaymentMethodRepository(ExpenseTrackerDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<PaymentMethod>> GetAllPaymentMethodsAsync()
        {
            return await this.context.PaymentMethod.ToListAsync();
        }
        
        public async Task<PaymentMethod?> GetPaymentMethodByIdAsync(int paymentMethodId)
        {
            return await this.context.PaymentMethod.FirstOrDefaultAsync(x => x.Id == paymentMethodId);
        }

        public async Task AddPaymentMethodAsync(PaymentMethod paymentMethod)
        {
            if(paymentMethod == null)
            {
                throw new ArgumentNullException(nameof(paymentMethod));
            }

            await this.context.PaymentMethod.AddAsync(paymentMethod);
        }

        public void DeletePaymentMethodAsync(PaymentMethod paymentMethod)
        {
            if(paymentMethod == null)
            {
                throw new ArgumentNullException(nameof(paymentMethod));
            }

            this.context.PaymentMethod.Remove(paymentMethod);
        }

        public async Task SaveChangesAsync()
        {
            await this.context.SaveChangesAsync();
        }
    }
}
