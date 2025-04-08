using System.Collections.Generic;
using System.Linq;
using _0_Framework.Application;
using _0_Framework.Infrastructure;
using AccountManagement.Application.Contracts.Role;
using AccountManagement.Domain.RoleAgg;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace AccountManagement.Infrastructure.EfCore.Repository;

public class RoleRepository : RepositoryBase<long, Role>, IRoleRepository
{
    private readonly AccountContext _accountContext;

    public RoleRepository(AccountContext accountContext) : base(accountContext)
    {
        _accountContext = accountContext;
    }

    public EditRole GetDetails(long id)
    {
        var role = _accountContext.Roles
            .Select(x => new EditRole
            {
                Id = x.Id,
                Name = x.Name,
                MappedPermissions = MapPermissions(x.Permissions)
            })
            .AsNoTracking()
            .FirstOrDefault(x => x.Id == id);

        role.Permissions = role.MappedPermissions.Select(x => x.Code).ToList();

        return role;
    }

    public List<RoleViewModel> List()
    {
        var result = _accountContext.Roles
            .Select(x => new RoleViewModel
            {
                Id = x.Id,
                Name = x.Name,
                CreationDate = x.CreationDate.ToFarsiFull()
            }).ToList();

        return result;
    }

    public List<int> GetRolePermissions(long id)
    {
        return _accountContext.Roles
            .Include(x => x.Permissions)
            .FirstOrDefault(x => x.Id == id)
            ?.Permissions
            .Select(x => x.Code)
            .ToList();
    }

    private static List<PermissionDto> MapPermissions(ICollection<Permission> permissions)
    {
        return permissions.Select(x => new PermissionDto(x.Code, x.Name)).ToList();
    }
}