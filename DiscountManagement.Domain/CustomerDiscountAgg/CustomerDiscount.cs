using System;
using _0_Framework.Domain;

namespace DiscountManagement.Domain.CustomerDiscountAgg;

public class CustomerDiscount : EntityBase
{
    public CustomerDiscount(long productId, double discountRate,
        DateTime startDate, DateTime endDate, string reason)
    {
        ProductId = productId;
        DiscountRate = discountRate;
        StartDate = startDate;
        EndDate = endDate;
        Reason = reason;
    }

    public long ProductId { get; set; }

    public double DiscountRate { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public string Reason { get; set; }

    public void Edit(long productId, double discountRate,
        DateTime startDate, DateTime endDate, string reason)
    {
        ProductId = productId;
        DiscountRate = discountRate;
        StartDate = startDate;
        EndDate = endDate;
        Reason = reason;
    }
}