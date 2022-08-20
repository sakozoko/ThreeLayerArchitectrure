using BLL;
using MarketUI.Command.Base;
using MarketUI.Util.Interface;

namespace MarketUI.Command.Category;

public class CategoryCreatingCommand : BaseCommand
{
    private readonly IServiceContainer _serviceContainer;
    private string _name;

    public CategoryCreatingCommand(IUserInterfaceMapperHandler mapperHandler, IServiceContainer serviceContainer,
        ICommandsInfoHandler commandsInfo) : base(mapperHandler, commandsInfo)
    {
        _serviceContainer = serviceContainer;
    }

    public override string Execute(string[] args)
    {
        if (!TryParseName(args)) GetHelp();
        var resultedId = _serviceContainer.CategoryService.Create(ConsoleUserInterface.AuthenticationData.Token, _name)
            .Result;
        return resultedId == -1 ? "Something wrong" : $"Success!New category id is{resultedId}";
    }

    private bool TryParseName(string[] args)
    {
        return TryParseArgs(args, out var dict) && dict.TryGetValue(Parameters[0], out _name);
    }
}