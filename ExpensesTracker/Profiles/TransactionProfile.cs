using AutoMapper;

namespace ExpensesTracker.Profiles
{
    public class TransactionProfile : Profile
    {
        public TransactionProfile()
        {

            CreateMap<Entities.Transaction, Models.CreateTransactionDto>();
            CreateMap<Entities.Transaction, Models.UpdateTransactionDto>();

            CreateMap<Models.CreateTransactionDto, Entities.Transaction>();
            CreateMap<Models.UpdateTransactionDto, Entities.Transaction>();

            //OUTPUT
            CreateMap<Entities.Transaction, Models.TransactionListDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(x => x.Category.Name))
                .ForMember(dest => dest.PaymentMethodName, opt => opt.MapFrom(x => x.PaymentMethod.Name));
        }
    }
}