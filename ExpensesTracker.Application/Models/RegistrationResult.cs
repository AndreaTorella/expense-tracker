namespace ExpensesTracker.Application.Models
{
    public class RegistrationResult
    {
        public bool Succeeded { get; set; }
        public IEnumerable<RegistrationError> Errors { get; set; } = [];
    }
}
