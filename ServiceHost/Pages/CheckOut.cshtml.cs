﻿using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using _0_Framework.Application.ZarinPal;
using _01_LampshadeQuery.Contracts;
using _01_LampshadeQuery.Contracts.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Nancy.Json;
using ShopManagement.Application.Contracts.Order;

namespace ServiceHost.Pages;

[Authorize]
public class CheckOutModel : PageModel
{
    public const string CookieName = "cart-items";
    private readonly ICartCalculatorService _cartCalculatorService;

    private readonly ICartService _cartService;
    private readonly IOrderApplication _orderApplication;
    private readonly IProductQuery _productQuery;
    private readonly IZarinPalFactory _zarinPalFactory;
    public Cart Cart;

    public CheckOutModel(ICartCalculatorService cartCalculatorService, ICartService cartService,
        IProductQuery productQuery, IOrderApplication orderApplication,
        IZarinPalFactory zarinPalFactory)
    {
        Cart = new Cart();
        _cartService = cartService;
        _productQuery = productQuery;
        _zarinPalFactory = zarinPalFactory;
        _orderApplication = orderApplication;
        _cartCalculatorService = cartCalculatorService;
    }

    public void OnGet()
    {
        var serializer = new JavaScriptSerializer();
        var value = Request.Cookies[CookieName];

        var cartItems = serializer.Deserialize<List<CartItem>>(value);
        if (cartItems != null)
        {
            foreach (var item in cartItems) item.CalculateTotalItemPrice();

            Cart = _cartCalculatorService.ComputeCart(cartItems);
            _cartService.Set(Cart);
        }
    }

    public IActionResult OnPostPay(int paymentMethod)
    {
        var cart = _cartService.Get();
        cart.SetPaymentMethod(paymentMethod);
        if (cart == null) return RedirectToPage("/Cart");
        var result = _productQuery.CheckInventoryStatus(cart.Items);
        if (result.Any(x => !x.IsInStock)) return RedirectToPage("/Cart");
        var orderId = _orderApplication.PlaceOrder(cart);

        if (paymentMethod == 1)
        {
            var paymentResponse = _zarinPalFactory.CreatePaymentRequest(cart.PayAmount.ToString(), "", "",
                "خرید از درگاه لوازم خانگی و دکوری", orderId);

            // Redirect To Payment Page
            return Redirect(
                $"https://{_zarinPalFactory.Prefix}.zarinpal.com/pg/StartPay/{paymentResponse.Authority}");
        }

        var paymentResult = new PaymentResult();
        Response.Cookies.Delete("cart-items");
        return RedirectToPage("/PaymentResult",
            paymentResult.Succeeded(
                " سفارش شما با موفقیت ثبت شد. پس از تماس کارشناسان ما و پرداخت وجه سفارش ارسال خواهد شد ", null));
    }

    public IActionResult OnGetCallBack([FromQuery] string authority, [FromQuery] string status,
        [FromQuery] long oId)
    {
        var orderAmount = _orderApplication.GetAmountBy(oId);
        var verificationResponse = _zarinPalFactory.CreateVerificationRequest(authority,
            orderAmount.ToString(CultureInfo.InvariantCulture));
        var result = new PaymentResult();
        if (status == "OK" && verificationResponse.Status >= 100)
        {
            var issueTrackingNo = _orderApplication.PaymentSucceeded(oId, verificationResponse.RefID);
            Response.Cookies.Delete("cart-items");
            result = result.Succeeded("پرداخت با موفقیت انجام شد", issueTrackingNo);
            return RedirectToPage("/PaymentResult", result);
        }

        result = result.Failed(
            "پرداخت با موفقیت انجام نشد. در صورت کسر وجه از حساب، مبلغ تا 24 ساعت آینده به حساب شما بازگردانده خواهد شد.");
        return RedirectToPage("/PaymentResult", result);
    }
}