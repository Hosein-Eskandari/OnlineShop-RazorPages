using System.Collections.Generic;
using _0_Framework.Domain;
using ShopManagement.Application.Contracts.Product;

namespace ShopManagement.Domain.ProductAgg;

public interface IProductRepository : IRepository<long, Product>
{
    List<ProductViewModel> Search(ProductSearchModel search);

    EditProduct GetDetails(long id);

    List<ProductViewModel> GetProducts();

    Product GetProductWithCategory(long id);
}