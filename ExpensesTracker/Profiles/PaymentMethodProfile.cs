using AutoMapper;
using ExpensesTracker.Domain.Entities;

namespace ExpensesTracker.Profiles
{
    public class PaymentMethodProfile : Profile
    {
        public PaymentMethodProfile()
        {
            this.CreateMap<PaymentMethod, Models.PaymentMethodDto>();
            this.CreateMap<Models.PaymentMethodDto, PaymentMethod>();
        }
    }
}
