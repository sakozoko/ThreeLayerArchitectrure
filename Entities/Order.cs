using Entities.User;

namespace Entities;

public class Order
{
    private readonly Dictionary<int, int> _products;

    public Order(int id, string desc, IUser owner)
    {
        Id = id;
        Description = desc;
        _products = new Dictionary<int, int>();
        Owner = owner ?? throw new ArgumentNullException(nameof(owner));
    }

    public Order(string desc, IUser owner) : this(0, desc, owner)
    {
    }

    public IUser Owner { get; }

    public int Id { get; }
    public string Description { get; }
    public bool IsDelivered { get; private set; }
    public bool IsCancelled { get; private set; }

    public void AddProduct(int id)
    {
        _products.TryGetValue(id, out var value);
        _products[id] = 1 + value;
    }

    public bool Cancel(IUser user)
    {
        if (!Owner.Equals(user) && user is Administrator) return false;
        IsCancelled = true;
        return true;
    }

    public bool Deliver(IUser user)
    {
        if (!Cancel(user)) return false;
        IsDelivered = true;
        return true;
    }

    public override string ToString()
    {
        return $"{Id} \t {Description} \t {IsCancelled} \t {IsDelivered} ";
    }
}