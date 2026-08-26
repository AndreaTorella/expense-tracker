namespace ExpensesTracker.Entities
{
    public class Household
    {
        public int Id { get; set; }
        public string FamilyName { get; set; } = string.Empty;
        public ICollection<ApplicationUser> ApplicationUsers { get; set; } = new List<ApplicationUser>();
    }
}
