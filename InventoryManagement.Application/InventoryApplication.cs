using System.Collections.Generic;
using _0_Framework.Application;
using InventoryManagement.Application.Contracts.Inventory;
using InventoryManagement.Domain.InventoryAgg;

namespace InventoryManagement.Application;

public class InventoryApplication : IInventoryApplication
{
    private readonly IAuthHelper _authHelper;
    private readonly IInventoryRepository _inventoryRepository;

    public InventoryApplication(IInventoryRepository inventoryRepository, IAuthHelper authHelper)
    {
        _authHelper = authHelper;
        _inventoryRepository = inventoryRepository;
    }

    public OperationResult Create(CreateInventory command)
    {
        var operation = new OperationResult();
        if (_inventoryRepository.Exists(x => x.ProductId == command.ProductId))
            return operation.Failed(ApplicationMessages.DuplicatedRecord);

        var inventory = new Inventory(command.ProductId, command.UnitPrice);
        _inventoryRepository.Create(inventory);
        _inventoryRepository.SaveChanges();
        return operation.Succeeded();
    }

    public OperationResult Edit(EditInventory command)
    {
        var operation = new OperationResult();
        if (_inventoryRepository.Exists(x => x.ProductId == command.ProductId &&
                                             x.Id != command.Id))
            return operation.Failed(ApplicationMessages.IsExisted);

        var inventory = _inventoryRepository.Get(command.Id);
        if (inventory == null) return operation.Failed(ApplicationMessages.RecordNotFound);

        inventory.Edit(command.ProductId, command.UnitPrice);
        _inventoryRepository.SaveChanges();
        return operation.Succeeded();
    }

    public OperationResult Increase(IncreaseInventory command)
    {
        var operation = new OperationResult();

        var inventory = _inventoryRepository.Get(command.InventoryId);
        if (inventory == null) return operation.Failed(ApplicationMessages.RecordNotFound);

        var operatorId = _authHelper.CurrentAccountId();
        inventory.Increase(command.Count, operatorId, command.Description);
        _inventoryRepository.SaveChanges();
        return operation.Succeeded();
    }

    public OperationResult Reduce(List<ReduceInventory> command)
    {
        var operation = new OperationResult();
        var operatorId = _authHelper.CurrentAccountId();


        foreach (var item in command)
        {
            var inventory = _inventoryRepository.GetBy(item.ProductId);
            if (inventory == null) return operation.Failed(ApplicationMessages.RecordNotFound);

            inventory.Reduce(item.Count, operatorId, item.Description, item.OrderId);
        }

        _inventoryRepository.SaveChanges();
        return operation.Succeeded();
    }

    public OperationResult Reduce(ReduceInventory command)
    {
        var operation = new OperationResult();

        var inventory = _inventoryRepository.Get(command.InventoryId);
        if (inventory == null) return operation.Failed(ApplicationMessages.RecordNotFound);

        var operatorId = _authHelper.CurrentAccountId();
        const long orderId = 0;
        var result = inventory.Reduce(command.Count, operatorId, command.Description, orderId);
        if (result == false) return operation.Failed(ApplicationMessages.LackOfInventory);
        _inventoryRepository.SaveChanges();
        return operation.Succeeded();
    }

    public EditInventory GetDetails(long id)
    {
        return _inventoryRepository.GetDetails(id);
    }

    public List<InventoryViewModel> Search(InventorySearchModel searchModel)
    {
        return _inventoryRepository.Search(searchModel);
    }

    public List<InventoryOperationViewModel> GetOperationLog(long inventoryId)
    {
        return _inventoryRepository.GetOperationLog(inventoryId);
    }
}