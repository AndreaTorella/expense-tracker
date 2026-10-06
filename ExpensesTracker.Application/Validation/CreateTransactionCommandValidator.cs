using ExpensesTracker.Application.Models;
using FluentValidation;

namespace ExpensesTracker.Application.Validation
{
    public class CreateTransactionCommandValidator : AbstractValidator<CreateTransactionCommand>
    {
        public CreateTransactionCommandValidator()
        {
            this.RuleFor(x => x.Title)
                .NotEmpty()
                .MaximumLength(150);

            this.RuleFor(x => x.Amount)
                .GreaterThan(0);

            RuleFor(x => x.Date)
                .NotEmpty();

            RuleFor(x => x.TransactionType)
                .IsInEnum();

            this.RuleFor(x => x.CategoryId)
                .GreaterThan(0);

            this.RuleFor(x => x.PaymentMethodId)
                .GreaterThan(0);
        }
    }
}
