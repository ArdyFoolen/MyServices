using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyServices.Commands;

namespace MyServices.Extensions
{
    public static class CommandExtensions
    {
        public static OutputCommand? ToOutput(this NetworkCommand command)
        {
            var output = new OutputCommand()
            {
                SessionToken = command.SessionToken,
                Command = command.Command,
                Output = command.Output,
                Error = command.Error,
                CommandEnum = command is PowershellCommand ? CommandEnum.PowershellCommand : CommandEnum.ServerCommand
            };
            return output;
        }
    }
}
