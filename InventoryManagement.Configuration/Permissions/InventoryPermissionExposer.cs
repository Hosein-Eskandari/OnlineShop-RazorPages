using System.Collections.Generic;
using _0_Framework.Infrastructure;

namespace InventoryManagement.Configuration.Permissions;

public class InventoryPermissionExposer : IPermissionExposer
{
    public Dictionary<string, List<PermissionDto>> Expose()
    {
        return new Dictionary<string, List<PermissionDto>>
        {
            {
                "---INVENTORY: انبار---", new List<PermissionDto>
                {
                    new(InventoryPermissions.ListInventory, "لیست انبار"),
                    new(InventoryPermissions.SearchInventory, "جستجو در انبار"),
                    new(InventoryPermissions.CreateInventory, "ایجاد انبار"),
                    new(InventoryPermissions.EditInventory, "ویرایش انبار"),
                    new(InventoryPermissions.IncreaseInventory, "افزایش موجودی انبار"),
                    new(InventoryPermissions.ReduceInventory, "کاهش موجودی انبار"),
                    new(InventoryPermissions.OperationLogInventory, "مشاهده گردش انبار")
                }
            }
        };
    }
}