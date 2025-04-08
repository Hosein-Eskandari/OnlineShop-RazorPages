using System.Collections.Generic;
using _0_Framework.Infrastructure;

namespace ShopManagement.Configuration.Permissions;

public class ShopPermissionExposer : IPermissionExposer
{
    public Dictionary<string, List<PermissionDto>> Expose()
    {
        return new Dictionary<string, List<PermissionDto>>
        {
            {
                "---PRODUCT: محصولات---", new List<PermissionDto>
                {
                    new(ShopPermissions.ListProducts, "لیست محصولات"),
                    new(ShopPermissions.SearchProducts, "جستجو در محصولات"),
                    new(ShopPermissions.CreateProduct, "ایجاد محصول"),
                    new(ShopPermissions.EditProduct, "ویرایش محصول")
                }
            },
            {
                "---PRODUCTCATEGORY: گروه محصولات---", new List<PermissionDto>
                {
                    new(ShopPermissions.ListProductCategories, "لیست گروه محصولات"),
                    new(ShopPermissions.SearchProductCategories, "جستجو در گروه محصولات"),
                    new(ShopPermissions.CreateProductCategory, "ایجاد گروه محصول"),
                    new(ShopPermissions.EditProductCategory, "ویرایش گروه محصول")
                }
            }
        };
    }
}