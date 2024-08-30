using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MyServices.Json;

namespace MyServices.Commands
{
    public abstract class NetworkCommand
    {
        public NetworkCommand() : this(Guid.Empty) { }
        public NetworkCommand(Guid token) : this(token, string.Empty) { }
        public NetworkCommand(Guid token, string command)
        {
            this.SessionToken = token;
            this.Command = command;
        }

        [JsonGuidParser]
        public Guid SessionToken { get; set; } = Guid.Empty;
        public string Command { get; set; } = string.Empty;
        public string Output { get; set; } = string.Empty;
        public string Error { get; set; } = string.Empty;

        public virtual bool IsQuit { get => false; }
        public virtual bool IsShutdown { get => false; }

        public abstract string Execute();

        public virtual NetworkCommand WriteTo(NetworkStream stream, JsonSerializerOptions options)
        {
            var data = JsonSerializer.Serialize<NetworkCommand>(this, options);
            byte[] msg = Encoding.ASCII.GetBytes(data);
            WriteDataToStream(stream, msg);
            return this;
        }

        private void WriteDataToStream(NetworkStream stream, byte[] bytes)
        {
            byte[] lBytes = BitConverter.GetBytes(bytes.Length);
            stream.Write(lBytes, 0, lBytes.Length);
            stream.Write(bytes, 0, bytes.Length);
        }
    }
}
