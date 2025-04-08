 using System.Collections.Generic;
using System.Linq;
using _0_Framework.Application;
using _0_Framework.Infrastructure;
using AccountManagement.Infrastructure.EfCore;
using ShopManagement.Application.Contracts;
using ShopManagement.Application.Contracts.Order;
using ShopManagement.Domain.OrderAgg;

namespace ShopManagement.Infrastructure.EFCore.Repository;

public class OrderRepository : RepositoryBase<long, Order>, IOrderRepository
{
    private readonly AccountContext _accountContext;
    private readonly ShopContext _context;

    public OrderRepository(ShopContext context, AccountContext accountContext) : base(context)
    {
        _context = context;
        _accountContext = accountContext;
    }

    public double GetAmountBy(long id)
    {
        var order = _context.Orders.Select(x => new { x.Id, x.PayAmount })
            .FirstOrDefault(x => x.Id == id);

        if (order != null) return order.PayAmount;
        return 0;
    }

    public List<OrderItemViewModel> GetItemsBy(long id)
    {
        var products = _context.Products.Select(x => new { x.Id, x.Name }).ToList();
        var orders = _context.Orders.FirstOrDefault(x => x.Id == id);

        if (orders == null) return new List<OrderItemViewModel>();

        var items = orders.Items.Select(x => new OrderItemViewModel
        {
            Id = x.Id,
            Count = x.Count,
            DiscountRate = x.DiscountRate,
            OrderId = x.OrderId,
            ProductId = x.ProductId,
            UnitPrice = x.UnitPrice
        }).ToList();

        foreach (var item in items) item.Product = products.FirstOrDefault(x => x.Id == item.ProductId)?.Name;


        return items;
    }

    public List<OrderViewModel> Search(OrderSearchModel searchModel)
    {
        var accounts = _accountContext.Accounts
            .Select(x => new { x.Id, x.FullName }).ToList();
        var query = _context.Orders.Select(x => new OrderViewModel
        {
            Id = x.Id,
            AccountId = x.AccountId,
            DiscountAmount = x.DiscountAmount,
            IsCanceled = x.IsCanceled,
            IsPaid = x.IsPaid,
            IssueTrackingNo = x.IssueTrackingNo,
            PayAmount = x.PayAmount,
            PaymentMethodId = x.PaymentMethod,
            RefId = x.RefId,
            TotalAmount = x.TotalAmount,
            CreationDate = x.CreationDate.ToFarsiFull()
        });

        query = query.Where(x => x.IsCanceled == searchModel.IsCanceled);

        if (searchModel.AccountId > 0) query = query.Where(x => x.AccountId == searchModel.AccountId);

        var orders = query.OrderByDescending(x => x.Id).ToList();

        foreach (var order in orders)
        {
            order.AccountFullName = accounts
                .FirstOrDefault(x => x.Id == order.AccountId)?.FullName;

            order.PaymentMethod = PaymentMethod.GetById(order.PaymentMethodId).Name;
        }

        return orders;
    }
}