using System.Collections.Generic;
using _0_Framework.Domain;
using AccountManagement.Domain.AccountAgg;

namespace AccountManagement.Domain.RoleAgg;

public class Role : EntityBase
{
    protected Role()
    {
    }

    public Role(string name, List<Permission> permissions)
    {
        Name = name;
        Permissions = permissions;
        Accounts = new List<Account>();
    }

    public string Name { get; private set; }

    public virtual ICollection<Account> Accounts { get; private set; }

    public virtual ICollection<Permission> Permissions { get; private set; }

    public void Edit(string name, List<Permission> permissions)
    {
        Name = name;
        Permissions = permissions;
    }
}