﻿using System.Collections.Generic;

namespace ShopManagement.Application.Contracts.Order;

public class Cart
{
    public Cart()
    {
        Items = new List<CartItem>();
    }

    public double TotalAmount { get; set; }

    public double DiscountAmount { get; set; }

    public double PayAmount { get; set; }

    public int PaymentMethod { get; set; }

    public List<CartItem> Items { get; set; }

    public void Add(CartItem cartItem)
    {
        Items.Add(cartItem);
        TotalAmount += cartItem.TotalItemPrice;
        DiscountAmount += cartItem.DiscountAmount;
        PayAmount += cartItem.ItemPayAmount;
    }

    public void SetPaymentMethod(int methodId)
    {
        PaymentMethod = methodId;
    }

    //public void SetItems(List<CartItem> items)
    //{
    //    items.ForEach(Add);
    //}
}