using AutoMapper;

namespace ExpensesTracker.Profiles
{
    public class PaymentMethodProfile : Profile
    {
        public PaymentMethodProfile()
        {
            this.CreateMap<Entities.PaymentMethod, Models.PaymentMethodDto>();
            this.CreateMap<Models.PaymentMethodDto, Entities.PaymentMethod>();
        }
    }
}
