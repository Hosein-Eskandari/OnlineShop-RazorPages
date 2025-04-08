using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopManagement.Domain.OrderAgg;

namespace ShopManagement.Infrastructure.EFCore.Mapping;

public class OrderMapping : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.AccountId);
        builder.Property(x => x.PaymentMethod);
        builder.Property(x => x.TotalAmount);
        builder.Property(x => x.DiscountAmount);
        builder.Property(x => x.PayAmount);
        builder.Property(x => x.IsPaid);
        builder.Property(x => x.IsCanceled);
        builder.Property(x => x.RefId);
        builder.Property(x => x.IssueTrackingNo).HasMaxLength(8);

        builder.OwnsMany(x => x.Items, navigationBuilder =>
        {
            navigationBuilder.ToTable("OrderItems");
            navigationBuilder.HasKey(x => x.Id);
            navigationBuilder.Property(x => x.ProductId);
            navigationBuilder.Property(x => x.Count);
            navigationBuilder.Property(x => x.UnitPrice);
            navigationBuilder.Property(x => x.DiscountRate);
            navigationBuilder.Property(x => x.OrderId);


            navigationBuilder.WithOwner(x => x.Order)
                .HasForeignKey(x => x.OrderId);
        });
    }
}