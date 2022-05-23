using System;
using System.Collections.Generic;
using ConsolesShop.Goods;
using ConsolesShop.User;

namespace ConsolesShop.Order;

public class Order
{
    private readonly Dictionary<int,int> _products;
    private readonly RegisteredUser _owner;

    public string Description { get; private set; }
    public bool IsDelivered { get; private set; }
    public bool IsCancelled { get; private set; }

    public Order(string desc, RegisteredUser owner)
    {
        Description = desc;
        _owner = owner;
        _products = new Dictionary<int, int>();
    }
    
    public void AddProduct(int id)
    {
        _products[id] += 1;
    }

    public bool Cancel(IUser user)
    {
        if (!_owner.Equals(user) && user is Administrator)
        {
            return false;
        }
        IsCancelled = true;
        return true;
    }

    public bool Deliver(IUser user)
    {
        if (!Cancel(user))
        {
            return false;
        }
        IsDelivered = true;
        return true;
    }
}