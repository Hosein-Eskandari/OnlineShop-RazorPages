using System.Collections.Generic;
using AccountManagement.Application.Contracts.Role;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Areas.Administration.Pages.Accounts.Role;

public class IndexModel : PageModel
{
    private readonly IRoleApplication _roleApplication;

    public List<RoleViewModel> Roles;

    public IndexModel(IRoleApplication roleApplication)
    {
        _roleApplication = roleApplication;
    }

    [TempData] public string Message { get; set; }

    [TempData] public string IsSuccedded { get; set; }

    public void OnGet()
    {
        Roles = _roleApplication.List();
    }
}