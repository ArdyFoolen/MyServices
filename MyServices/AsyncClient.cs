using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using static System.Runtime.InteropServices.JavaScript.JSType;
using MyServices.Json;
using System.ComponentModel.Design;
using MyServices.Commands;
using MyServices.Models;

namespace MyServices
{
    public class AsyncClient : IClientHandler
    {
        private string ipAddress;
        private int port;
        private bool isQuit = false;
        private Task _task;
        private Guid _sessionToken;
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private JsonSerializerOptions options;
        private CommandFactory _commandFactory;

        public AsyncClient(string ipAddress, int port)
        {
            this.ipAddress = ipAddress;
            this.port = port;
            this._sessionToken = Guid.Empty;
            _task = Task.CompletedTask;

            _commandFactory = new CommandFactory(this);
            options = new JsonSerializerOptions();
            options.Converters.Add(new NetworkCommandConverter(this));

        }

        public async Task RunAsync()
        {
            byte[] bytes = new byte[1024];

            try
            {
                TcpClient tcpClient = new TcpClient();
                tcpClient.Connect(this.ipAddress, this.port);

                Console.WriteLine("Socket connected to {0}",
                    this.ipAddress);

                var stream = tcpClient.GetStream();
                _task = Task.Run(() => Read(stream));

                await foreach (string line in ReadLineAsync())
                {
                    if (!string.IsNullOrWhiteSpace(line))
                        WriteCommandToStream(stream, line);
                }

                await _task;
                tcpClient.Close();
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine("ArgumentNullException : {0}", ex.ToString());
                throw;
            }
            catch (SocketException ex)
            {
                Console.WriteLine("SocketException : {0}", ex.ToString());
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unexpected exception : {0}", ex.ToString());
                throw;
            }
        }

        private NetworkCommand WriteCommandToStream(NetworkStream stream, string value)
            => _commandFactory
                .CreateCommand(_sessionToken, value)
                .WriteTo(stream, options);

        private async IAsyncEnumerable<string?> ReadLineAsync()
        {
            string? line = "";
            var reader = new StreamReader(Console.OpenStandardInput());
            while (!isQuit)
            {
                try
                {
                    line = await reader.ReadLineAsync().WaitAsync(_cancellationTokenSource.Token);
                }
                catch (OperationCanceledException) { }

                if (!isQuit)
                    yield return line;
            }
        }

        private void Read(NetworkStream stream)
        {
            string token = ReadDataFromStream(stream);
            _sessionToken = new Guid(token);
            Console.WriteLine($"Session token: {_sessionToken}");
            WritePrompt();

            while (!isQuit)
            {
                var command = ReadCommand(stream);
                command?.Execute();
            }
        }

        private string ReadDataFromStream(NetworkStream stream)
        {
            int length = WaitAndReadLength(stream);
            return ReadDataFromStream(stream, length);
        }

        private int WaitAndReadLength(NetworkStream stream)
        {
            byte[] bytes = new byte[sizeof(int)];
            stream.Read(bytes, 0, sizeof(int));
            return BitConverter.ToInt32(bytes, 0);
        }

        private string ReadDataFromStream(NetworkStream stream, int length)
        {
            string data = "";
            byte[] bytes = new byte[1024];
            while (length > 0)
            {
                int read = length > bytes.Length ? bytes.Length : length;
                int bytesRead = stream.Read(bytes, 0, read);
                data += Encoding.ASCII.GetString(bytes, 0, bytesRead);
                length -= bytesRead;
            }

            return data;
        }

        private NetworkCommand? ReadCommand(NetworkStream stream)
        {
            int length = WaitAndReadLength(stream);
            var command = ReadCommand(stream, length);
            return command;
        }

        private NetworkCommand? ReadCommand(NetworkStream stream, int length)
        {
            var data = ReadDataFromStream(stream, length);
            return JsonSerializer.Deserialize<NetworkCommand>(data, options);
        }

        private void WritePrompt()
        {
            if (_commandFactory.CommandEnum == CommandEnum.ServerCommand)
                Console.Write("SC> ");
            else
                Console.Write("PS> ");
        }

        #region IClientHandler

        public IEnumerable<Session> Sessions() => Enumerable.Empty<Session>();

        public void Quit()
        {
            isQuit = true;
            _cancellationTokenSource.Cancel();
        }

        public void Shutdown() { }

        #endregion
    }
}
