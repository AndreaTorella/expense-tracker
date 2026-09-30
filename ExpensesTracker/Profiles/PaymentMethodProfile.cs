using AutoMapper;
using ExpensesTracker.Application.Models;
using ExpensesTracker.Models;

namespace ExpensesTracker.Profiles
{
    public class PaymentMethodProfile : Profile
    {
        public PaymentMethodProfile()
        {
            this.CreateMap<PaymentMethodResult, PaymentMethodDto>();
            this.CreateMap<PaymentMethodDto, CreatePaymentMethodCommand>();
        }
    }
}
