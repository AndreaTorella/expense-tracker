using AutoMapper;
using ExpensesTracker.Entities;
using ExpensesTracker.Models;
using ExpensesTracker.Repositories;

namespace ExpensesTracker.Services
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

        public async Task<IEnumerable<PaymentMethodDto>> GetAllPaymentMethodsAsync()
        {
            var paymentMethodEntity = await paymentMethodRepository.GetAllPaymentMethodsAsync();
            return mapper.Map<IEnumerable<PaymentMethodDto>>(paymentMethodEntity);
        }

        public async Task<PaymentMethodDto?> GetPaymentMethodByIdAsync(int paymentMethodId)
        {
            var paymentMethodEntity = await paymentMethodRepository.GetPaymentMethodByIdAsync(paymentMethodId);

            if (paymentMethodEntity == null)
            {
                return null;
            }

            return mapper.Map<PaymentMethodDto>(paymentMethodEntity);
        }

        public async Task<PaymentMethodDto> AddPaymentMethodAsync(PaymentMethodDto paymentMethodDto)
        {
            if (paymentMethodDto == null)
            {
                throw new ArgumentNullException(nameof(paymentMethodDto));
            }

            var paymentMethodEntity = mapper.Map<PaymentMethod>(paymentMethodDto);
            await paymentMethodRepository.AddPaymentMethodAsync(paymentMethodEntity);
            await paymentMethodRepository.SaveChangesAsync();

            return mapper.Map<PaymentMethodDto>(paymentMethodEntity);
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