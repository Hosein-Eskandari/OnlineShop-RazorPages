using System.Collections.Generic;
using _0_Framework.Application;
using DiscountManagement.Application.Contracts.CustomerDiscount;
using DiscountManagement.Domain.CustomerDiscountAgg;

namespace DiscountManagement.Application;

public class CustomerDiscountApplication : ICustomerDiscountApplication
{
    private readonly ICostumerDiscountRepository _costumerDiscountRepository;

    public CustomerDiscountApplication(ICostumerDiscountRepository costumerDiscountRepository)
    {
        _costumerDiscountRepository = costumerDiscountRepository;
    }

    public OperationResult Define(DefineCustomerDiscount command)
    {
        var operation = new OperationResult();
        if (_costumerDiscountRepository.Exists(x => x.ProductId == command.ProductId
                                                    && x.DiscountRate == command.DiscountRate))
            return operation.Failed(ApplicationMessages.IsExisted);

        var startDate = command.StartDate.ToGeorgianDateTime();
        var endDate = command.EndDate.ToGeorgianDateTime();


        var costumerDiscount = new CustomerDiscount(command.ProductId, command.DiscountRate,
            startDate, endDate, command.Reason);

        _costumerDiscountRepository.Create(costumerDiscount);
        _costumerDiscountRepository.SaveChanges();
        return operation.Succeeded();
    }

    public OperationResult Edit(EditCustomerDiscount command)
    {
        var operation = new OperationResult();
        var customerDiscount = _costumerDiscountRepository.Get(command.Id);
        if (customerDiscount == null) return operation.Failed(ApplicationMessages.RecordNotFound);

        if (_costumerDiscountRepository.Exists(x => x.ProductId == command.ProductId &&
                                                    x.DiscountRate == command.DiscountRate &&
                                                    x.Id != command.Id))
            return operation.Failed(ApplicationMessages.DuplicatedRecord);


        var startDate = command.StartDate.ToGeorgianDateTime();
        var endDate = command.EndDate.ToGeorgianDateTime();


        customerDiscount.Edit(command.ProductId, command.DiscountRate,
            startDate, endDate, command.Reason);

        _costumerDiscountRepository.SaveChanges();
        return operation.Succeeded();
    }

    public EditCustomerDiscount GetDetails(long id)
    {
        return _costumerDiscountRepository.GetDetails(id);
    }

    public List<CustomerDiscountViewModel> Search(CustomerDiscountSearchModel search)
    {
        return _costumerDiscountRepository.Search(search);
    }
}