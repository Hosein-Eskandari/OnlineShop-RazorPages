using System.Linq;
using _01_LampshadeQuery.Contracts.Inventory;
using InventoryManagement.Infrastructure.EFCore;
using ShopManagement.Infrastructure.EFCore;

namespace _01_LampshadeQuery.Query;

public class InventoryQuery : IInventoryQuery
{
    private readonly InventoryContext _inventoryContext;
    private readonly ShopContext _shopContext;


    public InventoryQuery(InventoryContext inventoryContext, ShopContext shopContext)
    {
        _shopContext = shopContext;
        _inventoryContext = inventoryContext;
    }

    public StockStatus CheckStock(IsInStock command)
    {
        var inventory = _inventoryContext.Inventory
            .FirstOrDefault(x => x.ProductId == command.ProductId);

        if (inventory == null || inventory.CalculateCurrentCount() < command.Count)
        {
            var product = _shopContext.Products
                .Select(x => new { x.Id, x.Name })
                .FirstOrDefault(x => x.Id == command.ProductId);
            return new StockStatus
            {
                IsStock = false,
                ProductName = product?.Name
            };
        }

        return new StockStatus
        {
            IsStock = true
        };
    }
}