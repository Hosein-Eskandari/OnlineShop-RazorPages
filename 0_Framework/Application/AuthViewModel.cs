using System.Collections.Generic;

namespace _0_Framework.Application;

public class AuthViewModel
{
    public AuthViewModel()
    {
    }


    public AuthViewModel(long id, long roleId, string fullName, string userName,
        string mobile, List<int> permissions)
    {
        Id = id;
        RoleId = roleId;
        FullName = fullName;
        UserName = userName;
        Mobile = mobile;
        Permissions = permissions;
    }

    public long Id { get; set; }

    public long RoleId { get; set; }

    public string FullName { get; set; }

    public string UserName { get; set; }

    public string Role { get; set; }

    public string Mobile { get; set; }

    public List<int> Permissions { get; set; }
}