using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using System.Threading.Tasks;
using MyServices.Json;
using MyServices.Models;

namespace MyServices.Commands
{
    [DerivedType(typeof(OutputCommand), "Output")]
    public class OutputCommand : NetworkCommand
    {
        [JsonIntParser]
        public CommandEnum CommandEnum { get; set; }

        public override string Execute()
        {
            var result = Output;
            if (string.IsNullOrWhiteSpace(Error))
            {
                if ("sessions".Equals(Command.ToLowerInvariant()))
                {
                    JsonSerializerOptions options = new JsonSerializerOptions();
                    options.Converters.Add(new SessionConverter(new EndpointConverter()));
                    var sessions = JsonSerializer.Deserialize<Session[]>(Output, options) ?? Array.Empty<Session>();
                    foreach (var item in sessions)
                        Console.WriteLine(item);
                }
                else
                    Console.WriteLine($"Command {Command}: {Output}");
            }
            else
            {
                result = Error;
                Console.WriteLine($"Command {Command}, Error: {Error}");
            }

            WritePrompt();
            return result;
        }

        private void WritePrompt()
        {
            if (CommandEnum == CommandEnum.ServerCommand)
                Console.Write("SC> ");
            else
                Console.Write("PS> ");
        }
    }
}
