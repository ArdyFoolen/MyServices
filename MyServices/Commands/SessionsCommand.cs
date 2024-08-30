using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MyServices.Json;
using MyServices.Models;

namespace MyServices.Commands
{
    [DerivedType(typeof(SessionsCommand), "Sessions")]
    public class SessionsCommand : ServerCommand
    {
        public override bool IsQuit { get => true; }
        public override bool IsShutdown { get => false; }

        public SessionsCommand() : base() { }
        public SessionsCommand(Guid token) : base(token, string.Empty) { }
        public SessionsCommand(Guid token, string command) : base(token, command) { }
        public SessionsCommand(IClientHandler clientHandler) : base(clientHandler) { }
        public SessionsCommand(IClientHandler clientHandler, Guid token) : base(clientHandler, token, string.Empty) { }
        public SessionsCommand(IClientHandler clientHandler, Guid token, string command) : base(token, command) { }

        public override string Execute()
        {
            var sessions = clientHandler?.Sessions() ?? Enumerable.Empty<Session>();

            JsonSerializerOptions options = new JsonSerializerOptions();
            options.Converters.Add(new EndpointConverter());

            Output = JsonSerializer.Serialize(sessions, options);

            return Output;
        }
    }
}
