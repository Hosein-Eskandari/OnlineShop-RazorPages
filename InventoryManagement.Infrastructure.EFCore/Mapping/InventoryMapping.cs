using InventoryManagement.Domain.InventoryAgg;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryManagement.Infrastructure.EFCore.Mapping;

public class InventoryMapping : IEntityTypeConfiguration<Inventory>
{
    public void Configure(EntityTypeBuilder<Inventory> builder)
    {
        builder.ToTable("Inventory");

        builder.HasKey(x => x.Id);

        builder.OwnsMany(x => x.Operations, modelBuilder =>
        {
            modelBuilder.HasKey(x => x.Id);
            modelBuilder.ToTable("InventoryOperations");
            modelBuilder.Property(x => x.Description).HasMaxLength(1000);
            modelBuilder.Property(x => x.Operation);
            modelBuilder.Property(x => x.Count);
            modelBuilder.Property(x => x.OperatorId);
            modelBuilder.Property(x => x.OperationDate);
            modelBuilder.Property(x => x.CurrentCount);
            modelBuilder.Property(x => x.OrderId);
            modelBuilder.Property(x => x.InventoryId);


            modelBuilder.WithOwner(x => x.Inventory)
                .HasForeignKey(x => x.InventoryId);
        });
    }
}