using AutoMapper;
using ExpensesTracker.Application.Models;
using ExpensesTracker.Models;

namespace ExpensesTracker.Profiles
{
    public class RegistrationProfile : Profile
    {
        public RegistrationProfile()
        {
            this.CreateMap<RegisterDto, RegisterCommand>();
        }
    }
}
