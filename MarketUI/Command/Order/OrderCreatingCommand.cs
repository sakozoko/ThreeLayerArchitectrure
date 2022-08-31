using BLL;
using MarketUI.Command.Base;
using MarketUI.Util.Interface;

namespace MarketUI.Command.Order;

public class OrderCreatingCommand : BaseCommand
{
    private readonly IServiceManager _serviceManager;
    private string _desc;
    private int _id;
    private int _product;

    public OrderCreatingCommand(IServiceManager serviceManager, IUserInterfaceMapperHandler mapperHandler,
        ICommandsInfoHandler cih) :
        base(mapperHandler, cih)
    {
        _serviceManager = serviceManager;
    }

    public override string Execute(string[] args)
    {
        if (!TryParseParameters(args)) return GetHelp();
        var id = _serviceManager.OrderService.Create(ConsoleUserInterface.AuthenticationData?.Token, _desc,
            _product, _id);
        return id.Result == -1 ? "Something wrong" : $"Successful!Your order id is {id.Result}";
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
}