using AutoMapper;
using ExpensesTracker.Application.Models;
using ExpensesTracker.Models;

namespace ExpensesTracker.Profiles
{
    public class TransactionProfile : Profile
    {
        public TransactionProfile()
        {
            CreateMap<TransactionFilterDto, TransactionQuery>();

            CreateMap<CreateTransactionDto, CreateTransactionCommand>();
            CreateMap<UpdateTransactionDto, UpdateTransactionCommand>();

            CreateMap<TransactionResult, TransactionListDto>();
        }
    }
}