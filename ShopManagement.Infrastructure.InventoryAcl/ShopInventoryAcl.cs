using System.Collections.Generic;
using InventoryManagement.Application.Contracts.Inventory;
using ShopManagement.Domain.OrderAgg;
using ShopManagement.Domain.Services;

namespace ShopManagement.Infrastructure.InventoryAcl;

public class ShopInventoryAcl : IShopInventoryAcl
{
    private readonly IInventoryApplication _inventoryApplication;

    public ShopInventoryAcl(IInventoryApplication inventoryApplication)
    {
        _inventoryApplication = inventoryApplication;
    }

    public bool ReduceFromInventory(List<OrderItem> items)
    {
        var command = new List<ReduceInventory>();
        foreach (var orderItem in items)
        {
            var item = new ReduceInventory(orderItem.ProductId, orderItem.Count,
                "خرید مشتری", orderItem.OrderId);
            command.Add(item);
        }

        return _inventoryApplication.Reduce(command).IsSuccedded;
    }
}