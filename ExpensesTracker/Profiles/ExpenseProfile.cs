using AutoMapper;

namespace ExpensesTracker.Profiles
{
    public class ExpenseProfile : Profile
    {
        public ExpenseProfile()
        {

            CreateMap<Entities.Expense, Models.CreateExpenseDto>();
            CreateMap<Entities.Expense, Models.UpdateExpenseDto>();

            CreateMap<Models.CreateExpenseDto, Entities.Expense>();
            CreateMap<Models.UpdateExpenseDto, Entities.Expense>();

            //OUTPUT
            CreateMap<Entities.Expense, Models.ExpenseListDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(x => x.Category.Name))
                .ForMember(dest => dest.PaymentMethodName, opt => opt.MapFrom(x => x.PaymentMethod.Name));
        }
    }
}