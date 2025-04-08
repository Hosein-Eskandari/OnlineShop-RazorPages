using _0_Framework.Domain;

namespace DiscountManagement.Domain.ColleagueDiscountAgg;

public class ColleagueDiscount : EntityBase
{
    public ColleagueDiscount(long productId, double discountRate)
    {
        ProductId = productId;
        DiscountRate = discountRate;
    }

    public long ProductId { get; private set; }

    public double DiscountRate { get; private set; }

    public bool IsRemoved { get; private set; }

    public void Edit(long productId, double discountRate)
    {
        ProductId = productId;
        DiscountRate = discountRate;
        IsRemoved = false;
    }

    public void Remove()
    {
        IsRemoved = true;
    }

    public void Restore()
    {
        IsRemoved = false;
    }
}