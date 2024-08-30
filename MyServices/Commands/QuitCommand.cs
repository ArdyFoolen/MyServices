using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyServices.Json;

namespace MyServices.Commands
{
    [DerivedType(typeof(QuitCommand), "Quit")]
    public class QuitCommand : ServerCommand
    {
        public override bool IsQuit { get => true; }
        public override bool IsShutdown { get => false; }

        public QuitCommand() : base() { }
        public QuitCommand(Guid token) : base(token, string.Empty) { }
        public QuitCommand(Guid token, string command) : base(token, command) { }
        public QuitCommand(IClientHandler clientHandler) : base(clientHandler) { }
        public QuitCommand(IClientHandler clientHandler, Guid token) : base(clientHandler, token, string.Empty) { }
        public QuitCommand(IClientHandler clientHandler, Guid token, string command) : base(token, command) { }

        public override string Execute()
        {
            clientHandler?.Quit();
            Output = "Ok";

            return Output;
        }
    }
}
