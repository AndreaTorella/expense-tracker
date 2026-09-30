using AutoMapper;
using ExpensesTracker.Application.Models;
using ExpensesTracker.Domain.Entities;

namespace ExpensesTracker.Application.Profiles
{
    public class PaymentMethodProfile : Profile
    {
        public PaymentMethodProfile()
        {
            this.CreateMap<PaymentMethod, PaymentMethodResult>();
            this.CreateMap<CreatePaymentMethodCommand, PaymentMethod>();
        }
    }
}
