using System.Collections.Generic;
using _0_Framework.Application;

namespace AccountManagement.Application.Contracts.Account;

public interface IAccountApplication
{
    AccountViewModel GetAccountBy(long id);
    OperationResult Register(RegisterAccount command);

    OperationResult Edit(EditAccount command);

    EditAccount GetDetails(long id);

    OperationResult ChangePassword(ChangePassword command);

    List<AccountViewModel> Search(AccountSearchModel searchModel);

    OperationResult Login(Login command);

    void Logout();

    List<AccountViewModel> GetAccounts();
}