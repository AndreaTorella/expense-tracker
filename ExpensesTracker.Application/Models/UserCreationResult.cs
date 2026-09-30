namespace ExpensesTracker.Application.Models
{
    public class UserCreationResult
    {
        public bool Succeeded { get; set; }
        public IEnumerable<UserIdentityError> Errors { get; set; } = [];
    }
}
