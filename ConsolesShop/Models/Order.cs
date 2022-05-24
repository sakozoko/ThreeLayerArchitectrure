using System.Collections.Generic;
using ConsolesShop.User;

namespace ConsolesShop.Models;

public class Order
{
    private readonly Dictionary<int,int> _products;
    public RegisteredUser Owner { get; }

    public int Id { get; }
    public string Description { get; private set; }
    public bool IsDelivered { get; private set; }
    public bool IsCancelled { get; private set; }

    public Order(int id,string desc, RegisteredUser owner)
    {
        Id = id;
        Description = desc;
        Owner = owner;
        _products = new Dictionary<int, int>();
    }

    public Order(string desc, RegisteredUser owner) : this(0, desc, owner)
    {
    }
    
    public void AddProduct(int id)
    {
        _products[id] += 1;
    }

    public bool Cancel(IUser user)
    {
        if (!Owner.Equals(user) && user is Administrator)
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

    public override string ToString()
    {
        return $"{Id} \t {Description} \t {IsCancelled} \t {IsDelivered} ";
    }
}