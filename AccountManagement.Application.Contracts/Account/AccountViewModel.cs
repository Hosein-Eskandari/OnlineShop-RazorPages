namespace AccountManagement.Application.Contracts.Account;

public class AccountViewModel
{
    public long Id { get; set; }
    public string FullName { get; set; }

    public string UserName { get; set; }

    public string Mobile { get; set; }

    public string Role { get; set; }

    public string Password { get; set; }

    public long RoleId { get; set; }

    public string ProfilePhoto { get; set; }

    public string CreationDate { get; set; }
}