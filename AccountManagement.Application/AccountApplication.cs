using System.Collections.Generic;
using System.Linq;
using _0_Framework.Application;
using AccountManagement.Application.Contracts.Account;
using AccountManagement.Domain.AccountAgg;
using AccountManagement.Domain.RoleAgg;

namespace AccountManagement.Application;

public class AccountApplication : IAccountApplication
{
    private readonly IAccountRepository _accountRepository;
    private readonly IAuthHelper _authHelper;
    private readonly IFileUploader _fileUploader;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IRoleRepository _roleRepository;

    public AccountApplication(IAccountRepository accountRepository, IPasswordHasher passwordHasher,
        IFileUploader fileUploader, IAuthHelper authHelper, IRoleRepository roleRepository)
    {
        _accountRepository = accountRepository;
        _passwordHasher = passwordHasher;
        _fileUploader = fileUploader;
        _authHelper = authHelper;
        _roleRepository = roleRepository;
    }

    public AccountViewModel GetAccountBy(long id)
    {
        var account = _accountRepository.Get(id);
        return new AccountViewModel
        {
            FullName = account.FullName,
            Mobile = account.Mobile
        };
    }

    public OperationResult Register(RegisterAccount command)
    {
        var operation = new OperationResult();
        if (_accountRepository.Exists(x => x.UserName == command.UserName ||
                                           x.Mobile == command.Mobile))
            return operation.Failed(ApplicationMessages.DuplicatedRecord);

        var password = _passwordHasher.Hash(command.Password);

        var path = $"ProfilePhoto/{command.FullName}";
        var pictureName = _fileUploader.Upload(command.ProfilePhoto, path);

        if(command.RoleId == 0)
        {
            command.RoleId = 1;
        }

        var account = new Account(command.FullName, command.UserName, password,
            command.Mobile, command.RoleId, pictureName);

        _accountRepository.Create(account);
        _accountRepository.SaveChanges();
        return operation.Succeeded();
    }

    public OperationResult Edit(EditAccount command)
    {
        var operation = new OperationResult();
        var account = _accountRepository.Get(command.Id);
        if (account == null) return operation.Failed(ApplicationMessages.RecordNotFound);

        if (_accountRepository.Exists(x => (x.UserName == command.UserName
                                            || x.Mobile == command.Mobile)
                                           && x.Id != command.Id))
            return operation.Failed(ApplicationMessages.IsExisted);

        var path = $"ProfilePhoto/{command.FullName}";
        var pictureName = _fileUploader.Upload(command.ProfilePhoto, path);
        account.Edit(command.FullName, command.UserName, command.Mobile, command.RoleId,
            pictureName);

        _accountRepository.SaveChanges();
        return operation.Succeeded();
    }

    public EditAccount GetDetails(long id)
    {
        return _accountRepository.GetDetails(id);
    }

    public OperationResult ChangePassword(ChangePassword command)
    {
        var operation = new OperationResult();
        var account = _accountRepository.Get(command.Id);

        if (account == null) return operation.Failed(ApplicationMessages.RecordNotFound);

        if (command.Password != command.RePassword) return operation.Failed(ApplicationMessages.PasswordNotMatch);

        var password = _passwordHasher.Hash(command.Password);
        account.ChangePassword(password);

        _accountRepository.SaveChanges();
        return operation.Succeeded();
    }

    public List<AccountViewModel> Search(AccountSearchModel searchModel)
    {
        return _accountRepository.Search(searchModel);
    }

    public OperationResult Login(Login command)
    {
        var operation = new OperationResult();
        var account = _accountRepository.GetBy(command.UserName);
        if (account == null) return operation.Failed(ApplicationMessages.WrongUserName);

        (bool verified, bool needsUpgrade) result = _passwordHasher.Check(account.Password, command.Password);

        if (!result.verified) return operation.Failed(ApplicationMessages.WrongPassword);

        //var permissions = _roleRepository.Get(account.RoleId)
        //    .Permissions
        //    .Select(x => x.Code)
        //    .ToList();

        var permissions = _roleRepository.GetRolePermissions(account.RoleId);

         var authViewModel = new AuthViewModel(account.Id, account.RoleId, account.FullName,
            account.UserName, account.Mobile, permissions);

        _authHelper.SignIn(authViewModel);

        return operation.Succeeded();
    }


    public void Logout()
    {
        _authHelper.SignOut();
    }

    public List<AccountViewModel> GetAccounts()
    {
        return _accountRepository.GetAccounts();
    }
}