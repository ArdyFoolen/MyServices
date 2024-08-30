using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyServices.Json;

namespace MyServices.Commands
{
    [DerivedType(typeof(ShutdownCommand), "Shutdown")]
    public class ShutdownCommand : ServerCommand
    {
        public override bool IsQuit { get => true; }
        public override bool IsShutdown { get => true; }

        public ShutdownCommand() : base() { }
        public ShutdownCommand(Guid token) : base(token, string.Empty) { }
        public ShutdownCommand(Guid token, string command) : base(token, command) { }
        public ShutdownCommand(IClientHandler clientHandler) : base(clientHandler) { }
        public ShutdownCommand(IClientHandler clientHandler, Guid token) : base(clientHandler, token, string.Empty) { }
        public ShutdownCommand(IClientHandler clientHandler, Guid token, string command) : base(token, command) { }

        public override string Execute()
        {
            clientHandler?.Quit();
            clientHandler?.Shutdown();
            Output = "Ok";

            return Output;
        }
    }
}
