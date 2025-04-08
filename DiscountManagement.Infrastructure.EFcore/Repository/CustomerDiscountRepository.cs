using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using _0_Framework.Application;
using _0_Framework.Infrastructure;
using DiscountManagement.Application.Contracts.CustomerDiscount;
using DiscountManagement.Domain.CustomerDiscountAgg;
using ShopManagement.Infrastructure.EFCore;

namespace DiscountManagement.Infrastructure.EFCore.Repository;

public class CustomerDiscountRepository : RepositoryBase<long, CustomerDiscount>, ICostumerDiscountRepository
{
    private readonly DiscountContext _context;
    private readonly ShopContext _shopContext;


    public CustomerDiscountRepository(DiscountContext context, ShopContext shopContext) : base(context)
    {
        _context = context;
        _shopContext = shopContext;
    }

    public EditCustomerDiscount GetDetails(long id)
    {
        return _context.CostumerDiscounts.Select(x => new EditCustomerDiscount
        {
            Id = x.Id,
            ProductId = x.ProductId,
            DiscountRate = x.DiscountRate,
            StartDate = x.StartDate.ToString(CultureInfo.InvariantCulture),
            EndDate = x.EndDate.ToString(CultureInfo.InvariantCulture),
            Reason = x.Reason
        }).FirstOrDefault(x => x.Id == id);
    }

    public List<CustomerDiscountViewModel> Search(CustomerDiscountSearchModel search)
    {
        var product = _shopContext.Products.Select(x => new { x.Id, x.Name }).ToList();

        var query = _context.CostumerDiscounts.Select(x => new CustomerDiscountViewModel
        {
            Id = x.Id,
            ProductId = x.ProductId,
            DiscountRate = x.DiscountRate,
            StartDate = x.StartDate.ToFarsi(),
            EndDate = x.EndDate.ToFarsi(),
            StartDateGr = x.StartDate,
            EndDateGr = x.EndDate,
            Reason = x.Reason,
            CreationDate = x.CreationDate.ToFarsi()
        });

        if (search.ProductId > 0) query = query.Where(x => x.ProductId == search.ProductId);

        if (!string.IsNullOrWhiteSpace(search.StartDate))
            query = query.Where(x => x.StartDateGr > search.StartDate.ToGeorgianDateTime());

        if (!string.IsNullOrWhiteSpace(search.EndDate))
            query = query.Where(x => x.EndDateGr < search.EndDate.ToGeorgianDateTime());

        var discounts = query.OrderByDescending(x => x.Id).ToList();

        discounts.ForEach(discount =>
            discount.Product = product.FirstOrDefault(x => x.Id == discount.ProductId)?.Name);


        return discounts;
    }
}