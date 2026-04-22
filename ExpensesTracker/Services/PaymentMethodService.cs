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

        public async Task<IEnumerable<PaymentMethodDto>> GetAllPaymentMethodAsync()
        {
            var paymentMethodEntity = await this.paymentMethodRepository.GetAllPaymentMethodsAsync();
            return this.mapper.Map<IEnumerable<PaymentMethodDto>>(paymentMethodEntity);
        }

        public async Task<PaymentMethodDto?> GetPaymentMethodByIdAsync(int paymentMethodId)
        {
            var paymentMethodEntity = await this.paymentMethodRepository.GetPaymentMethodByIdAsync(paymentMethodId);

            if(paymentMethodEntity == null)
            {
                return null;
            }

            return this.mapper.Map<PaymentMethodDto>(paymentMethodEntity);
        }

        public async Task AddPaymentMethodAsync(PaymentMethodDto paymentMethodDto)
        {
            if(paymentMethodDto == null)
            {
                throw new ArgumentNullException(nameof(paymentMethodDto));
            }
            
            var paymentMethodEntity = this.mapper.Map<PaymentMethod>(paymentMethodDto);
            await this.paymentMethodRepository.AddPaymentMethodAsync(paymentMethodEntity);
            await this.paymentMethodRepository.SaveChangesAsync();
        }

        public async Task<bool> DeletePaymentMethodAsync(int paymentMethodId)
        {
            var paymentMethodEntity = await this.paymentMethodRepository.GetPaymentMethodByIdAsync(paymentMethodId);

            if (paymentMethodEntity == null)
            {
                return false;
            }

            this.paymentMethodRepository.DeletePaymentMethodAsync(paymentMethodEntity);
            await this.paymentMethodRepository.SaveChangesAsync();
            return true;
        }
    }
}