namespace ExpensesTracker.Application.Models
{
    public class RegisterCommand
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FamilyName { get; set; } = string.Empty;
    }
}
