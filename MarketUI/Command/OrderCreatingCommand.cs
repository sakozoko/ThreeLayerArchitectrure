using BLL;

namespace MarketUI.Util.Command;

public class OrderCreatingCommand : BaseCommand
{
    private static readonly string[] Parameters = { "-d", "-p", "-u" };
    private readonly IServiceContainer _serviceContainer;
    private string _desc;
    private int _id;
    private int _product;

    public OrderCreatingCommand(IServiceContainer serviceContainer) : base(Parameters)
    {
        _serviceContainer = serviceContainer;
    }


    public override string[] Names { get; } = { "cno", "order", "createno" };

    public override string Execute(string[] args)
    {
        if (!TryParseParameters(args)) return GetHelp();
        var productTask =
            _serviceContainer.ProductService.GetById(ConsoleUserInterface.AuthenticationData?.Token, _product);
        var userTask = _serviceContainer.UserService.GetById(ConsoleUserInterface.AuthenticationData?.Token, _id);
        var id = _serviceContainer.OrderService.Create(ConsoleUserInterface.AuthenticationData?.Token, _desc,
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