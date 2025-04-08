using DiscountManagement.Domain.ColleagueDiscountAgg;
using DiscountManagement.Domain.CustomerDiscountAgg;
using DiscountManagement.Infrastructure.EFCore.Mapping;
using Microsoft.EntityFrameworkCore;

namespace DiscountManagement.Infrastructure.EFCore;

public class DiscountContext : DbContext
{
    public DiscountContext(DbContextOptions<DiscountContext> options) : base(options)
    {
    }

    public DbSet<CustomerDiscount> CostumerDiscounts { get; set; }
    public DbSet<ColleagueDiscount> ColleagueDiscounts { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CustomerDiscount>().Property(x => x.DiscountRate)
            .HasColumnType("decimal").HasPrecision(17, 3);

        //modelBuilder.Entity<ColleagueDiscount>().Property(x => x.DiscountRate)
        //    .HasColumnType("decimal").HasPrecision(17, 3);

        var assembly = typeof(CustomerDiscountMapping).Assembly;
        modelBuilder.ApplyConfigurationsFromAssembly(assembly);


        base.OnModelCreating(modelBuilder);
    }
}