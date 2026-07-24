using System.ComponentModel.DataAnnotations;

namespace ExpensesTracker.Models
{
    public class CreateTransactionDto
    {
        [Required]
        [MaxLength(150)]
        public string Title { get; set; } = string.Empty;

        [Range(0.01, Double.MaxValue)]
        public decimal Amount { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public TransactionType TransactionType { get; set; }

        [Range(1, int.MaxValue)]
        public int CategoryId { get; set; }

        [Range(1, int.MaxValue)]
        public int PaymentMethodId { get; set; }
    }
}
