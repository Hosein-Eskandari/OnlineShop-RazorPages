using DiscountManagement.Domain.CustomerDiscountAgg;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiscountManagement.Infrastructure.EFCore.Mapping;

public class CustomerDiscountMapping : IEntityTypeConfiguration<CustomerDiscount>
{
    public void Configure(EntityTypeBuilder<CustomerDiscount> builder)
    {
        builder.ToTable("CostumerDiscounts");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.DiscountRate).HasPrecision(4, 2);

        builder.Property(x => x.Reason).HasMaxLength(500).IsRequired();
    }
}