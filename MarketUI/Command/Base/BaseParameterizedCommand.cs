using System;
using System.Collections.Generic;
using AutoMapper;
using MarketUI.Util.Interface;

namespace MarketUI.Command.Base
{
    public abstract class BaseParameterizedCommand : BaseDescriptiveCommand, IExecutableCommand
    {
        protected readonly string[] Parameters;

        protected BaseParameterizedCommand(IUserInterfaceMapperHandler mapperHandler,
            ICommandsInfoHandler commandsInfo) :
            base(commandsInfo)
        {
            Parameters = CommandsInfo.GetCommandInfo(GetType().Name).Parameters;
            Mapper = mapperHandler.GetMapper();
        }

        protected IMapper Mapper { get; }

        public abstract string Execute(string[] args);

        /// <summary>
        ///     Args must be as "-key, value, -key, value, etc",then algorithm working
        /// </summary>
        /// <param name="args">Args</param>
        /// <returns></returns>
        private Dictionary<string, string> ParseArgs(string[] args)
        {
            if (args == null)
                throw new ArgumentNullException(nameof(args));

            var dict = new Dictionary<string, string>();

            for (var i = 0; i < args.Length; i++)
            {
                var isParsed = false;

                foreach (var key in Parameters)
                {
                    if (!args[i].Equals(key)) continue;
                    if (i + 1 < args.Length)
                        dict[key] = args[i + 1];
                    else
                        dict[key] = "";
                    isParsed = true;
                }

                if (isParsed) i++;
            }

            return dict;
        }

        protected bool TryParseArgs(string[] args, out Dictionary<string, string> dict)
        {
            dict = null;
            try
            {
                dict = ParseArgs(args);
            }
            catch (NullReferenceException)
            {
                return false;
            }
            catch (IndexOutOfRangeException)
            {
                return false;
            }
            catch (ArgumentNullException)
            {
                return false;
            }

            return dict?.Count > 0;
        }
    }
}