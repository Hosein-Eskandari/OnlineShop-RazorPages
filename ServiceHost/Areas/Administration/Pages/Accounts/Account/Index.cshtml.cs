using System.Collections.Generic;
using AccountManagement.Application.Contracts.Account;
using AccountManagement.Application.Contracts.Role;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ServiceHost.Areas.Administration.Pages.Accounts.Account;

public class IndexModel : PageModel
{
    private readonly IAccountApplication _accountApplication;
    private readonly IRoleApplication _roleApplication;
    public List<AccountViewModel> Accounts;

    public SelectList Roles;

    public AccountSearchModel SearchModel;

    public IndexModel(IAccountApplication accountApplication,
        IRoleApplication roleApplication)
    {
        _accountApplication = accountApplication;
        _roleApplication = roleApplication;
    }

    [TempData] public string Message { get; set; }

    [TempData] public string IsSuccedded { get; set; }

    public void OnGet(AccountSearchModel searchModel)
    {
        Roles = new SelectList(_roleApplication.List(),
            "Id", "Name");
        Accounts = _accountApplication.Search(searchModel);
    }

    public IActionResult OnGetRegister()
    {
        var account = new RegisterAccount
        {
            Roles = _roleApplication.List()
        };

        return Partial("./Create", account);
    }

    public IActionResult OnPostRegister(RegisterAccount command)
    {
        var result = _accountApplication.Register(command);
        return new JsonResult(result);
    }

    public IActionResult OnGetEdit(long id)
    {
        var account = _accountApplication.GetDetails(id);
        account.Roles = _roleApplication.List();
        return Partial("./Edit", account);
    }

    public JsonResult OnPostEdit(EditAccount command)
    {
        var result = _accountApplication.Edit(command);
        return new JsonResult(result);
    }

    public IActionResult OnGetChangePassword(long id)
    {
        var command = new ChangePassword { Id = id };
        return Partial("./ChangePassword", command);
    }

    public JsonResult OnPostChangePassword(ChangePassword command)
    {
        var result = _accountApplication.ChangePassword(command);
        return new JsonResult(result);
    }
}