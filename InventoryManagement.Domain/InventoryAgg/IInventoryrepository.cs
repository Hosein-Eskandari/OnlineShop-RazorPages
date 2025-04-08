using System.Collections.Generic;
using _0_Framework.Domain;
using InventoryManagement.Application.Contracts.Inventory;

namespace InventoryManagement.Domain.InventoryAgg;

public interface IInventoryRepository : IRepository<long, Inventory>
{
    Inventory GetBy(long productId);

    EditInventory GetDetails(long id);

    List<InventoryViewModel> Search(InventorySearchModel searchModel);

    List<InventoryOperationViewModel> GetOperationLog(long inventoryId);
}