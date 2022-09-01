using System.Collections.Generic;
using System.Linq;
using BLL;
using ConsoleTable;
using MarketUI.Command.Base;
using MarketUI.Models;
using MarketUI.Util.Interface;

namespace MarketUI.Command.Category;

public class CategoryViewCommand : BaseParameterizedCommand
{
    private readonly IServiceManager _serviceManager;

    public CategoryViewCommand(IUserInterfaceMapperHandler mapperHandler, IServiceManager serviceManager,
        ICommandsInfoHandler commandsInfo) : base(mapperHandler, commandsInfo)
    {
        _serviceManager = serviceManager;
    }

    public override string Execute(string[] args)
    {
        var result = Mapper.Map<IEnumerable<CategoryModel>>(_serviceManager.CategoryService
            .GetAll(ConsoleUserInterface.AuthenticationData.Token).Result).ToArray();
        if (!result.Any()) return "None";

        var table = new Table()
            .AddColumn("#", "Name")
            .AddAlignment(Alignment.Center);
        if (TryParseArgs(args, out var dict) && dict.TryGetValue(Parameters[0], out var name))
            foreach (var category in result.Where(x => x.Name.Contains(name)))
                table.AddRow(category.Id, category.Name);
        else
            foreach (var category in result)
                table.AddRow(category.Id, category.Name);
        return table.ToString();
    }
}