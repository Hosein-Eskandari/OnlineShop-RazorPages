using System.Collections.Generic;
using System.Linq;
using _0_Framework.Application;
using _0_Framework.Infrastructure;
using AccountManagement.Infrastructure.EfCore;
using InventoryManagement.Application.Contracts.Inventory;
using InventoryManagement.Domain.InventoryAgg;
using ShopManagement.Infrastructure.EFCore;

namespace InventoryManagement.Infrastructure.EFCore.Repository;

public class InventoryRepository : RepositoryBase<long, Inventory>, IInventoryRepository
{
    private readonly ShopContext _shopContext;
    private readonly InventoryContext _context;
    private readonly AccountContext _accountContext;

    public InventoryRepository(InventoryContext context, ShopContext shopContext, AccountContext accountContext) :
        base(context)
    {
        _context = context;
        _shopContext = shopContext;
        _accountContext = accountContext;
    }

    public Inventory GetBy(long productId)
    {
        return _context.Inventory.FirstOrDefault(x => x.ProductId == productId);
    }

    public EditInventory GetDetails(long id)
    {
        return _context.Inventory.Select(x => new EditInventory
            {
                Id = x.Id,
                ProductId = x.ProductId,
                UnitPrice = x.UnitPrice
            })
            .FirstOrDefault(x => x.Id == id);
    }

    public List<InventoryViewModel> Search(InventorySearchModel searchModel)
    {
        var products = _shopContext.Products.Select(x => new { x.Id, x.Name }).ToList();

        var query = _context.Inventory.Select(x => new InventoryViewModel
        {
            Id = x.Id,
            InStock = x.InStock,
            ProductId = x.ProductId,
            UnitPrice = x.UnitPrice,
            CurrentCount = x.CalculateCurrentCount(),
            CreationDate = x.CreationDate.ToFarsiFull()
        });

        if (searchModel.ProductId > 0) query = query.Where(x => x.ProductId == searchModel.ProductId);

        if (searchModel.InStock) query = query.Where(x => !x.InStock);

        var inventories = query.OrderByDescending(x => x.Id).ToList();


        inventories.ForEach(inventory =>
            inventory.Product = products.FirstOrDefault(x => x.Id == inventory.ProductId)?.Name);

        return inventories;
    }

    public List<InventoryOperationViewModel> GetOperationLog(long inventoryId)
    {
        var accounts = _accountContext.Accounts.Select(x => new { x.Id, x.FullName }).ToList();
        var inventory = _context.Inventory.FirstOrDefault(x => x.Id == inventoryId);

        var operations = inventory.Operations
            .Select(x => new InventoryOperationViewModel
            {
                Id = x.Id,
                Count = x.Count,
                CurrentCount = x.CurrentCount,
                Description = x.Description,
                Operation = x.Operation,
                OperationDate = x.OperationDate.ToFarsiFull(),
                OperatorId = x.OperatorId,
                OrderId = x.OrderId
            }).OrderByDescending(x => x.Id).ToList();

        foreach (var operation in operations)
            operation.Operator = accounts.FirstOrDefault(x => x.Id == operation.OperatorId)?.FullName;

        return operations;
    }
}