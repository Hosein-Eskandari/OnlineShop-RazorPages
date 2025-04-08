using System.Collections.Generic;
using _0_Framework.Domain;
using DiscountManagement.Application.Contracts.CustomerDiscount;

namespace DiscountManagement.Domain.CustomerDiscountAgg;

public interface ICostumerDiscountRepository : IRepository<long, CustomerDiscount>
{
    EditCustomerDiscount GetDetails(long id);

    List<CustomerDiscountViewModel> Search(CustomerDiscountSearchModel search);
}