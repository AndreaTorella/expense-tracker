using ExpensesTracker.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpensesTracker.Data
{
    public class PaymentMethodConfiguration : IEntityTypeConfiguration<PaymentMethod>
    {
        public void Configure(EntityTypeBuilder<PaymentMethod> entity)
        {
            entity.ToTable(nameof(PaymentMethod));

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Name);

            entity.HasIndex(e => e.Name)
                .IsUnique();
        }
    }
}
