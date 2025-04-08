using System.ComponentModel.DataAnnotations;
using _0_Framework.Application;

namespace InventoryManagement.Application.Contracts.Inventory;

public class IncreaseInventory
{
    public long InventoryId { get; set; }

    [Range(1, 100000, ErrorMessage = ValidationMessages.NotInRange)]

    public long Count { get; set; }

    [StringLength(1000, ErrorMessage = ValidationMessages.OutOfRange)]

    public string Description { get; set; }
}