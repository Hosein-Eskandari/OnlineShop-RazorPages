using System.Collections.Generic;
using _0_Framework.Application;
using _0_Framework.Application.Sms;
using Microsoft.Extensions.Configuration;
using ShopManagement.Application.Contracts.Order;
using ShopManagement.Domain.OrderAgg;
using ShopManagement.Domain.Services;

namespace ShopManagement.Application;

public class OrderApplication : IOrderApplication
{
    private readonly IAuthHelper _authHelper;
    private readonly IConfiguration _configuration;
    private readonly IOrderRepository _orderRepository;
    private readonly IShopAccountAcl _shopAccountAcl;
    private readonly IShopInventoryAcl _shopInventoryAcl;
    private readonly ISmsService _smsService;

    public OrderApplication(IOrderRepository orderRepository, IAuthHelper authHelper,
        IConfiguration configuration, IShopInventoryAcl shopInventoryAcl,
        ISmsService smsService, IShopAccountAcl shopAccountAcl)
    {
        _authHelper = authHelper;
        _smsService = smsService;
        _configuration = configuration;
        _shopAccountAcl = shopAccountAcl;
        _orderRepository = orderRepository;
        _shopInventoryAcl = shopInventoryAcl;
    }

    public long PlaceOrder(Cart cart)
    {
        var currentAccountId = _authHelper.CurrentAccountId();

        var order = new Order(currentAccountId, cart.PaymentMethod, cart.TotalAmount, cart.DiscountAmount,
            cart.PayAmount);

        foreach (var cartItem in cart.Items)
        {
            var orderItem = new OrderItem(cartItem.Id, cartItem.Count,
                cartItem.UnitPrice, cartItem.DiscountRate);
            order.AddItem(orderItem);
        }

        _orderRepository.Create(order);
        _orderRepository.SaveChanges();
        return order.Id;
    }

    public double GetAmountBy(long id)
    {
        return _orderRepository.GetAmountBy(id);
    }

    public void Cancel(long orderId)
    {
        var order = _orderRepository.Get(orderId);
        order.Cancel();
        _orderRepository.SaveChanges();
    }

    public List<OrderItemViewModel> GetItemsBy(long id)
    {
        return _orderRepository.GetItemsBy(id);
    }

    public string PaymentSucceeded(long orderId, long refId)
    {
        var order = _orderRepository.Get(orderId);
        order.PaymentSucceeded(refId);
        var symbol = _configuration.GetSection("Symbol").Value;
        var issueTrackingNo = CodeGenerator.Generate(symbol);
        order.SetIssueTrackingNo(issueTrackingNo);
        // Reduce orderItems From Inventory
        if (!_shopInventoryAcl.ReduceFromInventory(order.Items)) return "";

        var account = _shopAccountAcl.GetAccountBy(order.AccountId);

        _smsService.Send(account.mobile,
            $" گرامی سفارش شما با شماره پیگیری {account.name} {issueTrackingNo} موفقیت ثبت شد و ارسال خواهد شد");
        return issueTrackingNo;
    }

    public List<OrderViewModel> Search(OrderSearchModel searchModel)
    {
        return _orderRepository.Search(searchModel);
    }
}