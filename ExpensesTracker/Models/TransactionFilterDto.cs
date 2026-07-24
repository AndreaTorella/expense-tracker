using ExpensesTracker.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace ExpensesTracker.Models
{
    public class TransactionFilterDto : IValidatableObject
    {

        [StringLength(
            100,
            ErrorMessage = "Il testo di ricerca non può superare i 100 caratteri.")]
        public string? Search { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public TransactionType? TransactionType { get; set; }

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

        public TransactionSortBy TransactionSortBy { get; set; } = Enums.TransactionSortBy.Date;

        public SortDirection SortDirection { get; set; } = Enums.SortDirection.Desc;

        [Range(1, int.MaxValue)]
        public int PageNumber { get; set; } = 1;

        [Range(1, 100)]
        public int PageSize { get; set; } = 10;

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
