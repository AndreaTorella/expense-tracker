using ExpensesTracker.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpensesTracker.Data
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> entity)
        {
            entity.ToTable(nameof(Category));

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Name);

            entity.HasIndex(e => e.Name)
                .IsUnique();
        }
    }
}
