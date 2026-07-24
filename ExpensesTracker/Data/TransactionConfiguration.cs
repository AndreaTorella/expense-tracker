using ExpensesTracker.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpensesTracker.Data
{
    public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> entity)
        {
            entity.ToTable(nameof(Transaction));

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.Amount)
                .IsRequired()
                .HasPrecision(10, 2);

            entity.Property(e => e.Date)
                .IsRequired();

            entity.Property(e => e.TransactionType)
                .IsRequired();

            entity.HasOne(e => e.Category)
                .WithMany(c => c.Transactions)
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.PaymentMethod)
                .WithMany(pm => pm.Transactions)
                .HasForeignKey(e => e.PaymentMethodId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
