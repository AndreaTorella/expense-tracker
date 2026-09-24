using AutoMapper;
using ExpensesTracker.Application.Models;
using ExpensesTracker.Domain.Entities;

namespace ExpensesTracker.Application.Profiles
{
    public class TransactionProfile : Profile
    {
        public TransactionProfile()
        {
            CreateMap<CreateTransactionCommand, Transaction>();
            CreateMap<UpdateTransactionCommand, Transaction>();

            CreateMap<Transaction, TransactionResult>()
                .ForMember(
                    dest => dest.CategoryName,
                    opt => opt.MapFrom(x => x.Category.Name))
                .ForMember(
                    dest => dest.PaymentMethodName,
                    opt => opt.MapFrom(x => x.PaymentMethod.Name));
        }
    }
}
