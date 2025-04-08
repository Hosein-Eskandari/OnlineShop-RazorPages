using System;
using System.Collections.Generic;
using System.Linq;
using _01_LampshadeQuery.Contracts.Product;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Nancy.Json;
using ShopManagement.Application.Contracts.Order;

namespace ServiceHost.Pages;

public class CartModel : PageModel
{
    public const string CookieName = "cart-items";
    private readonly IProductQuery _productQuery;
    public List<CartItem> CartItems;

    public CartModel(IProductQuery productQuery)
    {
        CartItems = new List<CartItem>();
        _productQuery = productQuery;
    }

    public void OnGet()
    {
        var serializer = new JavaScriptSerializer();
        var value = Request.Cookies[CookieName];
        var cartItems = serializer.Deserialize<List<CartItem>>(value);
        if (cartItems != null)
        {
            foreach (var item in cartItems) item.CalculateTotalItemPrice();
            CartItems = _productQuery.CheckInventoryStatus(cartItems);
        }
    }

    public IActionResult OnGetRemoveFromCart(long id)
    {
        var serializer = new JavaScriptSerializer();
        var value = Request.Cookies[CookieName];
        Response.Cookies.Delete(CookieName);
        var cartItems = serializer.Deserialize<List<CartItem>>(value);
        var itemToRemove = cartItems.FirstOrDefault(x => x.Id == id);
        cartItems.Remove(itemToRemove);
        var options = new CookieOptions
            { Expires = DateTime.Now.AddDays(2), Path = "/", IsEssential = true, HttpOnly = false };
        var cookieValue = serializer.Serialize(cartItems);
        Response.Cookies.Append(CookieName, cookieValue, options);
        return RedirectToPage("/Cart");
    }

    public IActionResult OnGetGoToCheckOut()
    {
        var serializer = new JavaScriptSerializer();
        var value = Request.Cookies[CookieName];
        var cartItems = serializer.Deserialize<List<CartItem>>(value);
        if (cartItems != null)
        {
            foreach (var item in cartItems) item.TotalItemPrice = item.UnitPrice * item.Count;
            CartItems = _productQuery.CheckInventoryStatus(cartItems);
            return RedirectToPage(CartItems.Any(x => !x.IsInStock) ? "/Cart" : "/CheckOut");
        }

        return RedirectToPage("/Cart");


        //if (CartItems.Any(x => !x.IsInStock))
        //{
        //    return RedirectToPage("/Cart");
        //}

        //return RedirectToPage("/CheckOut");
    }
}