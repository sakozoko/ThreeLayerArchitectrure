using Autofac;
using MarketUI.Command.Base;
using MarketUI.Command.Util;

namespace MarketUI.Command
{
    public class CommandFactory : ICommandFactory
    {
        private readonly IContainer _container;
        private readonly IExecutableCommand _helpCommand;

        public CommandFactory(IContainer container)
        {
            #region SetCommands

            _container = container;
            _helpCommand = new HelpCommand(_container);

            #endregion
        }

        public IExecutableCommand GetCommand(string name)
        {
            return name is "h" || name is "?" || name is "help"
                ? _helpCommand
                : _container.ResolveOptionalNamed<IExecutableCommand>(name) ?? _container.Resolve<IExecutableCommand>();
        }
    }
}