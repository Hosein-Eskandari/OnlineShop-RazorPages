using AccountManagement.Application;
using AccountManagement.Application.Contracts.Account;
using AccountManagement.Application.Contracts.Role;
using AccountManagement.Domain.AccountAgg;
using AccountManagement.Domain.RoleAgg;
using AccountManagement.Infrastructure.EfCore;
using AccountManagement.Infrastructure.EfCore.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AccountManagement.Configuration;

public class AccountManagementBootstarpper
{
    public static void Configure(IServiceCollection services, string connectionString)
    {
        services.AddTransient<IAccountApplication, AccountApplication>();
        services.AddTransient<IAccountRepository, AccountRepository>();

        services.AddTransient<IRoleApplication, RoleApplication>();
        services.AddTransient<IRoleRepository, RoleRepository>();

        services.AddDbContext<AccountContext>(x =>
            x.UseSqlServer(connectionString));
    }
}