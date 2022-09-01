using BLL;
using MarketUI.Command.Base;
using MarketUI.Util.Interface;

namespace MarketUI.Command.Category;

public class CategoryModifyingCommand : BaseParameterizedCommand
{
    private readonly IServiceManager _serviceManager;


    public CategoryModifyingCommand(IUserInterfaceMapperHandler mapperHandler, IServiceManager serviceManager,
        ICommandsInfoHandler commandsInfo) : base(mapperHandler, commandsInfo)
    {
        _serviceManager = serviceManager;
    }

    public override string Execute(string[] args)
    {
        if (!TryParseArgs(args, out var dict)) GetHelp();

        if (dict.TryGetValue(Parameters[0], out var strCategoryId) &&
            int.TryParse(strCategoryId, out var categoryId) &&
            dict.TryGetValue(Parameters[1], out var newName))
        {
            var result = _serviceManager.CategoryService.ChangeName(ConsoleUserInterface.AuthenticationData.Token,
                newName, categoryId).Result;
            if (result)
                return $"Category with id {categoryId} has been changed";
        }

        return "Something wrong";
    }
}