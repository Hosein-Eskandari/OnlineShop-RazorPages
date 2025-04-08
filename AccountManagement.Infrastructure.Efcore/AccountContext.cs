using AccountManagement.Domain.AccountAgg;
using AccountManagement.Domain.RoleAgg;

using AccountMangement.Infrastructure.EFCore.Mappings;
using Microsoft.EntityFrameworkCore;
//using AccountManagement.Domain.RoleAgg;

namespace AccountManagement.Infrastructure.EfCore;

public class AccountContext : DbContext
{
    public AccountContext(DbContextOptions<AccountContext> options) : base(options)
    {
    }

    public DbSet<Account> Accounts { get; set; }
    public DbSet<Role> Roles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var assembly = typeof(AccountMapping).Assembly;
        modelBuilder.ApplyConfigurationsFromAssembly(assembly);
        base.OnModelCreating(modelBuilder);
    }
}