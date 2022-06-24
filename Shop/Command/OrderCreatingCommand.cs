using BLL;

namespace Shop.Command;

public class OrderCreatingCommand : BaseCommand
{
    private static readonly string[] Names = { "cno", "order", "createno" };
    private static readonly string[] Parameters = { "-d", "-p", "-u" };
    private readonly Service _service;
    private string _desc;
    private int _id;
    private int _product;

    public OrderCreatingCommand(Service service) : base(Names, Parameters)
    {
        _service = service;
    }


    public override string Execute(string[] args)
    {
        if (!TryParseParameters(args)) return GetHelp();
        var productTask =
            _service.Factory.ProductService.GetById(Shop.AuthenticationData?.Token, _product);
        var userTask = _service.Factory.UserService.GetById(Shop.AuthenticationData?.Token, _id);
        var id = _service.Factory.OrderService.Create(Shop.AuthenticationData?.Token, _desc,
            productTask.Result, userTask.Result);
        return $"Successful! Id your order is {id.Result}";
    }

    private bool TryParseParameters(string[] args)
    {
        if (!TryParseArgs(args, out var dict)) return false;
        var firstFlag = dict.TryGetValue(Parameters[0], out _desc);
        var secondFlag = dict.TryGetValue(Parameters[1], out var indexOfProduct)
                         && int.TryParse(indexOfProduct, out _product);
        var thirdFlag = dict.TryGetValue(Parameters[2], out var userId)
                        && int.TryParse(userId, out _id);
        return firstFlag || secondFlag || thirdFlag;
    }

    public override string GetHelp()
    {
        return "Create new order \t cno or order or createno \t -d , -p, -u";
    }
}