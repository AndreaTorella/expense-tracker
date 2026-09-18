using ExpensesTracker.Application.Repositories;
using ExpensesTracker.Domain.Entities;
using ExpensesTracker.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ExpensesTracker.Infrastructure.Repositories
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
            return await this.context.PaymentMethods.ToListAsync();
        }

        public async Task<PaymentMethod?> GetPaymentMethodByIdAsync(int paymentMethodId)
        {
            return await this.context.PaymentMethods.FirstOrDefaultAsync(x => x.Id == paymentMethodId);
        }

        public async Task AddPaymentMethodAsync(PaymentMethod paymentMethod)
        {
            if (paymentMethod == null)
            {
                throw new ArgumentNullException(nameof(paymentMethod));
            }

            await this.context.PaymentMethods.AddAsync(paymentMethod);
        }

        public void DeletePaymentMethodAsync(PaymentMethod paymentMethod)
        {
            if (paymentMethod == null)
            {
                throw new ArgumentNullException(nameof(paymentMethod));
            }

            this.context.PaymentMethods.Remove(paymentMethod);
        }

        public async Task SaveChangesAsync()
        {
            await this.context.SaveChangesAsync();
        }
    }
}
