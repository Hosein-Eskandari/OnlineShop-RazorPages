using System.Collections.Generic;
using System.Linq;
using _0_Framework.Infrastructure;
using AccountManagement.Application.Contracts.Role;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ServiceHost.Areas.Administration.Pages.Accounts.Role;

public class EditModel : PageModel
{
    private readonly IEnumerable<IPermissionExposer> _exposers;
    private readonly IRoleApplication _roleApplication;
    public EditRole Command;
    public List<SelectListItem> Permissions = new();

    public EditModel(IRoleApplication roleApplication, IEnumerable<IPermissionExposer> exposers)
    {
        _roleApplication = roleApplication;
        _exposers = exposers;
    }

    public void OnGet(long id)
    {
        Command = _roleApplication.GetDetails(id);
        foreach (var exposer in _exposers)
        {
            var exposedPermissions = exposer.Expose();
            //var group = new SelectListGroup { Name = exposedPermissions.Keys.ToString() };

            foreach (var (key, value) in exposedPermissions)
            {
                var group = new SelectListGroup { Name = key };
                foreach (var permission in value)
                {
                    var item = new SelectListItem(permission.Name, permission.Code.ToString())
                    {
                        Group = group
                    };

                    if (Command.MappedPermissions.Any(x => x.Code == permission.Code))
                        item.Selected = true;

                    Permissions.Add(item);
                }
            }
        }

        var c = Permissions;
    }

    public IActionResult OnPost(EditRole command)
    {
        var result = _roleApplication.Edit(command);
        return RedirectToPage("Index");
    }
}