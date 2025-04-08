using System.Collections.Generic;

namespace ShopManagement.Application.Contracts.Order;

public interface IOrderApplication
{
    long PlaceOrder(Cart cart);

    double GetAmountBy(long id);

    void Cancel(long orderId);

    List<OrderItemViewModel> GetItemsBy(long id);

    string PaymentSucceeded(long orderId, long refId);

    List<OrderViewModel> Search(OrderSearchModel searchModel);
}