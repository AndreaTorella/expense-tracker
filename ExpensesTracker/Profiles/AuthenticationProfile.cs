using AutoMapper;
using ExpensesTracker.Application.Models;
using ExpensesTracker.Models;

namespace ExpensesTracker.Profiles
{
    public class AuthenticationProfile : Profile
    {
        public AuthenticationProfile()
        {
            this.CreateMap<RegisterDto, RegisterCommand>();
            this.CreateMap<LoginDto, LoginCommand>();
        }
    }
}
