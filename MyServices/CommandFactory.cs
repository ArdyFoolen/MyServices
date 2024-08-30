using MyServices.Commands;
using MyServices.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyServices
{
    public enum CommandEnum
    {
        [Command("sc")]
        ServerCommand,
        [Command("ps")]
        PowershellCommand
    }

    public class CommandFactory
    {
        private readonly IClientHandler _clientHandler;

        public CommandEnum CommandEnum { get; private set; }

        public CommandFactory(IClientHandler clientHandler)
        {
            this._clientHandler = clientHandler;
        }

        private void SetCommandType(string value)
            => CommandEnum = value.ToEnum<CommandEnum>();

        public NetworkCommand? CreateCommand(Guid token, string? value = null)
        {
            if ("ps".Equals(value?.ToLowerInvariant() ?? string.Empty) ||
                "sc".Equals(value?.ToLowerInvariant() ?? string.Empty))
                return CreateToggleCommand(token, value);

            if (CommandEnum == CommandEnum.PowershellCommand)
                return CreatePowershellCommand(token, value);

            return CreateServerCommand(token, value);
        }

        private NetworkCommand? CreateServerCommand(Guid token, string? value)
            => typeof(NetworkCommand)
                .GetDerivedTypeAttributeFrom(value)?
                .Create<NetworkCommand>(_clientHandler, token, value);

        private NetworkCommand CreatePowershellCommand(Guid token, string? value)
        {
            return new PowershellCommand()
            {
                SessionToken = token,
                Command = value ?? string.Empty
            };
        }

        private NetworkCommand CreateToggleCommand(Guid token, string? value)
        {
            SetCommandType(value);
            return new ToggleCommand()
            {
                SessionToken = token,
                Command = value ?? string.Empty
            };
        }
    }
}
