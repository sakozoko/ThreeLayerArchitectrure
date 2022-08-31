using BLL;
using MarketUI.Command.Base;
using MarketUI.Util.Interface;

namespace MarketUI.Command.Category;

public class CategoryCreatingCommand : BaseCommand
{
    private readonly IServiceManager _serviceManager;
    private string _name;

    public CategoryCreatingCommand(IUserInterfaceMapperHandler mapperHandler, IServiceManager serviceManager,
        ICommandsInfoHandler commandsInfo) : base(mapperHandler, commandsInfo)
    {
        _serviceManager = serviceManager;
    }

    public override string Execute(string[] args)
    {
        if (!TryParseName(args)) GetHelp();
        var resultedId = _serviceManager.CategoryService.Create(ConsoleUserInterface.AuthenticationData.Token, _name)
            .Result;
        return resultedId == -1 ? "Something wrong" : $"Success!New category id is{resultedId}";
    }

    private bool TryParseName(string[] args)
    {
        return TryParseArgs(args, out var dict) && dict.TryGetValue(Parameters[0], out _name);
    }
}