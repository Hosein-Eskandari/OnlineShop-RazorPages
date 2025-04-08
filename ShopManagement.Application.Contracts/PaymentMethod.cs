using System.Collections.Generic;
using System.Linq;

namespace ShopManagement.Application.Contracts;

public class PaymentMethod
{
    private PaymentMethod(int id, string name, string description)
    {
        Id = id;
        Name = name;
        Description = description;
    }

    public int Id { get; }

    public string Name { get; private set; }

    public string Description { get; private set; }

    public static List<PaymentMethod> GetList()
    {
        return new List<PaymentMethod>
        {
            new(1, "پرداخت اینترنتی",
                "شما به درگاه اینترنتی هدایت شده و در لحظه پرداخت وجه را انجام خواهید داد."),
            new(2, "پرداخت نقدی", "در این مدل، سفارش ثبت شده و سپس وجه به صورت نقدی در زمان تحویل کالا، دریافت میشود")
        };
    }

    public static PaymentMethod GetById(int id)
    {
        return GetList().FirstOrDefault(x => x.Id == id);
    }
}