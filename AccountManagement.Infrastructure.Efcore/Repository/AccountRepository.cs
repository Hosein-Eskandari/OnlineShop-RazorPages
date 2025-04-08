using System.Collections.Generic;
using System.Linq;
using _0_Framework.Application;
using _0_Framework.Infrastructure;
using AccountManagement.Application.Contracts.Account;
using AccountManagement.Domain.AccountAgg;
using Microsoft.EntityFrameworkCore;

namespace AccountManagement.Infrastructure.EfCore.Repository;

public class AccountRepository : RepositoryBase<long, Account>, IAccountRepository
{
    private readonly AccountContext _accountContext;

    public AccountRepository(AccountContext context) : base(context)
    {
        _accountContext = context;
    }

    public Account GetBy(string userName)
    {
        var result = _accountContext.Accounts
            .FirstOrDefault(a => a.UserName == userName);

        return result;
    }

    public EditAccount GetDetails(long id)
    {
        var result = _accountContext.Accounts
            .Select(x => new EditAccount
            {
                Id = x.Id,
                FullName = x.FullName,
                UserName = x.UserName,
                RoleId = x.RoleId,
                Mobile = x.Mobile
                //ProfilePhoto = x.ProfilePhoto
            }).FirstOrDefault(x => x.Id == id);

        return result;
    }

    public List<AccountViewModel> GetAccounts()
    {
        return _accountContext.Accounts
            .Select(x => new AccountViewModel
            {
                Id = x.Id,
                FullName = x.FullName
            }).ToList();
    }

    public List<AccountViewModel> Search(AccountSearchModel searchModel)
    {
        var query = _accountContext.Accounts
            .Include(x => x.Role)
            .Select(x => new AccountViewModel
            {
                Id = x.Id,
                FullName = x.FullName,
                UserName = x.UserName,
                Mobile = x.Mobile,
                ProfilePhoto = x.ProfilePhoto,
                CreationDate = x.CreationDate.ToFarsiFull(),
                Role = x.Role.Name,
                RoleId = x.RoleId
            });

        if (!string.IsNullOrWhiteSpace(searchModel.FullName))
            query = query.Where(x => x.FullName == searchModel.FullName);

        if (!string.IsNullOrWhiteSpace(searchModel.UserName))
            query = query.Where(x => x.UserName == searchModel.UserName);

        if (!string.IsNullOrWhiteSpace(searchModel.Mobile)) query = query.Where(x => x.Mobile == searchModel.Mobile);

        if (searchModel.RoleId > 0) query = query.Where(x => x.RoleId == searchModel.RoleId);

        return query.OrderByDescending(x => x.Id).ToList();
    }
}