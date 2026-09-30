using AutoMapper;
using ExpensesTracker.Application.Models;
using ExpensesTracker.Application.Repositories;
using ExpensesTracker.Domain.Entities;

namespace ExpensesTracker.Application.Services
{
    public class PaymentMethodService : IPaymentMethodService
    {
        private readonly IPaymentMethodRepository paymentMethodRepository;
        private readonly IMapper mapper;

        public PaymentMethodService(
            IPaymentMethodRepository paymentMethodRepository,
            IMapper mapper)
        {
            this.paymentMethodRepository = paymentMethodRepository ?? throw new ArgumentNullException(nameof(paymentMethodRepository));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<PaymentMethodResult>> GetAllPaymentMethodsAsync()
        {
            var paymentMethodEntity = await paymentMethodRepository.GetAllPaymentMethodsAsync();
            return mapper.Map<IEnumerable<PaymentMethodResult>>(paymentMethodEntity);
        }

        public async Task<PaymentMethodResult?> GetPaymentMethodByIdAsync(int paymentMethodId)
        {
            var paymentMethodEntity = await paymentMethodRepository.GetPaymentMethodByIdAsync(paymentMethodId);

            if (paymentMethodEntity == null)
            {
                return null;
            }

            return mapper.Map<PaymentMethodResult>(paymentMethodEntity);
        }

        public async Task<PaymentMethodResult> AddPaymentMethodAsync(CreatePaymentMethodCommand createPaymentMethodCommand)
        {
            if (createPaymentMethodCommand == null)
            {
                throw new ArgumentNullException(nameof(createPaymentMethodCommand));
            }

            var paymentMethodEntity = mapper.Map<PaymentMethod>(createPaymentMethodCommand);
            await paymentMethodRepository.AddPaymentMethodAsync(paymentMethodEntity);
            await paymentMethodRepository.SaveChangesAsync();

            return mapper.Map<PaymentMethodResult>(paymentMethodEntity);
        }

        public async Task<bool> DeletePaymentMethodAsync(int paymentMethodId)
        {
            var paymentMethodEntity = await paymentMethodRepository.GetPaymentMethodByIdAsync(paymentMethodId);

            if (paymentMethodEntity == null)
            {
                return false;
            }

            paymentMethodRepository.DeletePaymentMethodAsync(paymentMethodEntity);
            await paymentMethodRepository.SaveChangesAsync();
            return true;
        }
    }
}