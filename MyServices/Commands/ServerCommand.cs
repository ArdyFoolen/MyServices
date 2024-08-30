using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyServices.Json;

namespace MyServices.Commands
{
    [DerivedType(typeof(ServerCommand), "Server")]
    public class ServerCommand : NetworkCommand
    {
        protected readonly IClientHandler? clientHandler = null;

        public override bool IsQuit { get => "quit".Equals(Command.ToLowerInvariant()) || IsShutdown; }
        public override bool IsShutdown { get => "shutdown".Equals(Command.ToLowerInvariant()); }

        public ServerCommand() : base() { }
        public ServerCommand(Guid token) : base(token, string.Empty) { }
        public ServerCommand(Guid token, string command) : base(token, command) { }
        public ServerCommand(IClientHandler clientHandler) : this(clientHandler, Guid.Empty) { }
        public ServerCommand(IClientHandler clientHandler, Guid token) : this(clientHandler, token, string.Empty) { }
        public ServerCommand(IClientHandler clientHandler, Guid token, string command) : base(token, command)
        {
            this.clientHandler = clientHandler;
        }

        public override string Execute()
        {
            var handled = HandleCommands();
            if (handled)
                Output = "Ok";
            else
            {
                Output = "Error";
                Error = $"Command {Command} not handled";
            }
            return Output;
        }

        private bool HandleCommands()
        {
            if (string.IsNullOrWhiteSpace(Command))
                return false;
            var handled = HandleIfIsQuit(false);
            handled = HandleIfIsShutdown(handled);
            return handled;
        }

        private bool HandleIfIsShutdown(bool handled)
        {
            if (IsShutdown)
            {
                clientHandler?.Shutdown();
                return true;
            }
            return handled;
        }

        private bool HandleIfIsQuit(bool handled)
        {
            if (IsQuit)
            {
                clientHandler?.Quit();
                return true;
            }
            return handled;
        }

        public override string ToString()
        {
            return @$"Command:
  Token:   {SessionToken}
  Command: {Command}
  Output:  {Output}
  Error:   {Error}";
        }
    }
}
