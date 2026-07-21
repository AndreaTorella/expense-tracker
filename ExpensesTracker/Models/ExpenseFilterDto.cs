using ExpensesTracker.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace ExpensesTracker.Models
{
    public class ExpenseFilterDto : IValidatableObject
    {

        [StringLength(
            100,
            ErrorMessage = "Il testo di ricerca non può superare i 100 caratteri.")]
        public string? Search { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        [Range(
          1,
          int.MaxValue,
          ErrorMessage = "L'identificativo della categoria deve essere maggiore di zero.")]
        public int? CategoryId { get; set; }

        [Range(
            1,
            int.MaxValue,
            ErrorMessage = "L'identificativo del metodo di pagamento deve essere maggiore di zero.")]
        public int? PaymentMethodId { get; set; }

        public ExpenseSortBy ExpenseSortBy { get; set; } = Enums.ExpenseSortBy.Date;

        public SortDirection SortDirection { get; set; } = Enums.SortDirection.Desc;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (FromDate.HasValue && ToDate.HasValue && FromDate > ToDate)
            {
                yield return new ValidationResult(
                    "La data iniziale non può essere successiva alla data finale.",
                    [nameof(FromDate), nameof(ToDate)]);
            }
        }
    }
}
