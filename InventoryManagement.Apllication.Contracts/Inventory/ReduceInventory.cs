using System.ComponentModel.DataAnnotations;
using _0_Framework.Application;

namespace InventoryManagement.Application.Contracts.Inventory;

public class ReduceInventory
{
    public ReduceInventory()
    {
    }

    public ReduceInventory(long productId, long count, string description, long orderId)
    {
        ProductId = productId;
        Count = count;
        Description = description;
        OrderId = orderId;
    }

    public long InventoryId { get; set; }

    public long ProductId { get; set; }

    [Range(1, 100000, ErrorMessage = ValidationMessages.NotInRange)]
    public long Count { get; set; }

    [StringLength(1000, ErrorMessage = ValidationMessages.OutOfRange)]
    public string Description { get; set; }

    public long OrderId { get; set; }
}