using AutoMapper;
using ExpensesTracker.Application.Models;
using ExpensesTracker.Domain.Entities;
using ExpensesTracker.Models;

namespace ExpensesTracker.Profiles
{
    public class TransactionProfile : Profile
    {
        public TransactionProfile()
        {

            CreateMap<Transaction, Models.CreateTransactionDto>();
            CreateMap<Transaction, Models.UpdateTransactionDto>();

            CreateMap<Models.CreateTransactionDto, Transaction>();
            CreateMap<Models.UpdateTransactionDto, Transaction>();

            //OUTPUT
            CreateMap<Transaction, Models.TransactionListDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(x => x.Category.Name))
                .ForMember(dest => dest.PaymentMethodName, opt => opt.MapFrom(x => x.PaymentMethod.Name));


            CreateMap<TransactionFilterDto, TransactionQuery>();
            CreateMap<CreateTransactionDto, CreateTransactionCommand>();
            CreateMap<UpdateTransactionDto, UpdateTransactionCommand>();
            CreateMap<TransactionResult, TransactionListDto>();
        }
    }
}