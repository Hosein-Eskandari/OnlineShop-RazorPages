using System.Collections.Generic;
using _0_Framework.Infrastructure;
using InventoryManagement.Application.Contracts.Inventory;
using InventoryManagement.Configuration.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShopManagement.Application.Contracts.Product;

namespace ServiceHost.Areas.Administration.Pages.Inventory;

[Authorize(Roles = Roles.Administrator)]
public class IndexModel : PageModel
{
    private readonly IInventoryApplication _inventoryApplication;

    private readonly IProductApplication _productApplication;

    public List<InventoryViewModel> Inventory;

    public SelectList Products;

    public InventorySearchModel SearchModel;

    public IndexModel(IProductApplication productApplication, IInventoryApplication inventoryApplication)
    {
        _productApplication = productApplication;
        _inventoryApplication = inventoryApplication;
    }

    [TempData] public string Message { get; set; }

    [TempData] public string IsSuccedded { get; set; }

    [TempData] public bool Flag { get; set; }

    [NeedsPermission(InventoryPermissions.ListInventory)]
    public void OnGet(InventorySearchModel searchModel)
    {
        Inventory = _inventoryApplication.Search(searchModel);
        Products = new SelectList(_productApplication.GetProducts(),
            "Id", "Name");
    }

    public IActionResult OnGetCreate()
    {
        var command = new CreateInventory
        {
            Products = _productApplication.GetProducts()
        };
        return Partial("./Create", command);
    }

    [NeedsPermission(InventoryPermissions.CreateInventory)]
    public JsonResult OnPostCreate(CreateInventory command)
    {
        var result = _inventoryApplication.Create(command);
        return new JsonResult(result);
    }

    public IActionResult OnGetEdit(long id)
    {
        var inventory = _inventoryApplication.GetDetails(id);
        inventory.Products = _productApplication.GetProducts();
        return Partial("./Edit", inventory);
    }

    [NeedsPermission(InventoryPermissions.EditInventory)]
    public JsonResult OnPostEdit(EditInventory command)
    {
        var result = _inventoryApplication.Edit(command);
        return new JsonResult(result);
    }

    public IActionResult OnGetIncrease(long id)
    {
        var command = new IncreaseInventory
        {
            InventoryId = id
        };
        return Partial("./Increase", command);
    }

    [NeedsPermission(InventoryPermissions.IncreaseInventory)]
    public JsonResult OnPostIncrease(IncreaseInventory command)
    {
        var result = _inventoryApplication.Increase(command);
        return new JsonResult(result);
    }

    public IActionResult OnGetReduce(long id)
    {
        var command = new ReduceInventory
        {
            InventoryId = id
        };
        return Partial("./Reduce", command);
    }

    [NeedsPermission(InventoryPermissions.ReduceInventory)]
    public JsonResult OnPostReduce(ReduceInventory command)
    {
        var result = _inventoryApplication.Reduce(command);

        if (result.IsSuccedded)
        {
            Flag = true;
            Message = result.Message;
            IsSuccedded = result.IsSuccedded.ToString();
            return new JsonResult(result);
        }

        Message = result.Message;
        IsSuccedded = result.IsSuccedded.ToString();
        result.IsSuccedded = true;
        Flag = false;
        return new JsonResult(result);
    }

    [NeedsPermission(InventoryPermissions.OperationLogInventory)]
    public IActionResult OnGetLog(long id)
    {
        var result = _inventoryApplication.GetOperationLog(id);
        return Partial("./OperationLog", result);
    }
}