using System.Collections.Generic;
using ShopManagement.Application.Contracts.Order;

namespace _01_LampshadeQuery.Contracts.Product;

public interface IProductQuery
{
    List<ProductQueryModel> Search(string value);
    List<ProductQueryModel> GetLatestArrivals();
    ProductQueryModel GetDetails(string slug);
    List<CartItem> CheckInventoryStatus(List<CartItem>? cartItems);
}