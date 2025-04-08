using _0_Framework.Domain;

namespace ShopManagement.Domain.OrderAgg;

public class OrderItem : EntityBase
{
    protected OrderItem()
    {
    }

    public OrderItem(long productId, int count, double unitPrice, double discountRate)
    {
        ProductId = productId;
        Count = count;
        UnitPrice = unitPrice;
        DiscountRate = discountRate;
    }

    public long ProductId { get; private set; }

    public int Count { get; private set; }

    public double UnitPrice { get; private set; }

    public double DiscountRate { get; private set; }

    public long OrderId { get; private set; }

    public Order Order { get; private set; }
}