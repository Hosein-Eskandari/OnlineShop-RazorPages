using _0_Framework.Domain;
using _0_Framework.Infrastructure;
using AccountManagement.Domain.RoleAgg;

//using AccountManagement.Domain.RoleAgg;

namespace AccountManagement.Domain.AccountAgg;

public class Account : EntityBase
{
    public Account(string fullName, string userName, string password,
        string mobile, long roleId, string profilePhoto)
    {
        FullName = fullName;
        UserName = userName;
        Password = password;
        Mobile = mobile;

        RoleId = roleId;

        if (roleId == 0) RoleId = long.Parse(Roles.SystemUser);


        ProfilePhoto = profilePhoto;

    }

    public string FullName { get; private set; }

    public string UserName { get; private set; }

    public string Password { get; private set; }

    public string Mobile { get; private set; }

    public long RoleId { get; private set; }

    public virtual Role Role { get; }

    public string ProfilePhoto { get; private set; }

    public void Edit(string fullName, string userName,
        string mobile, long roleId, string profilePhoto)
    {
        FullName = fullName;
        UserName = userName;
        Mobile = mobile;
        RoleId = roleId;
        if (!string.IsNullOrWhiteSpace(profilePhoto))
             ProfilePhoto = profilePhoto;
    }

    public void ChangePassword(string password)
    {
        Password = password;
    }
}