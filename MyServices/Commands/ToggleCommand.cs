using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MyServices.Commands
{
    public class ToggleCommand : ServerCommand
    {
        public override string Execute()
        {
            return "Ok";
        }

        public override NetworkCommand WriteTo(NetworkStream stream, JsonSerializerOptions options)
        {
            WritePrompt();
            return this;
        }

        private void WritePrompt()
            => Console.Write($"{Command.ToUpperInvariant()}> ");
    }
}
