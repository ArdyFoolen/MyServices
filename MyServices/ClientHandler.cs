using MyServices.Commands;
using MyServices.Extensions;
using MyServices.Json;
using MyServices.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MyServices
{
    public interface IClientHandler
    {
        IEnumerable<Session> Sessions();
        void Quit();
        void Shutdown();
    }

    public partial class AsyncServer
    {
        private class ClientHandler : IClientHandler
        {
            private readonly AsyncServer _server;
            private CancellationTokenSource _cancellationTokenSource;
            private TcpClient _client;
            private NetworkCommand? _command = null;
            private CommandFactory _commandFactory;
            private Task _task;
            private NetworkStream? _stream = null;
            private bool isQuit = false;
            private JsonSerializerOptions options;

            public Guid SessionToken { get; private set; } = Guid.Empty;

            public ClientHandler(AsyncServer server, TcpClient client, CancellationTokenSource cancellationTokenSource)
            {
                _server = server;
                _client = client;
                _cancellationTokenSource = cancellationTokenSource;
                _task = Task.CompletedTask;

                options = new JsonSerializerOptions();
                options.Converters.Add(new NetworkCommandConverter(this));
                _commandFactory = new CommandFactory(this);
            }

            public void Start()
                => _task = Task.Run(async () => await HandleClientAsync());

            public async Task Await()
                => await _task;

            private async Task HandleClientAsync()
            {
                _stream = _client.GetStream();
                SessionToken = Guid.NewGuid();

                try
                {
                    WriteDataToStream(SessionToken.ToString());
                    Console.WriteLine($"Session accepted: {SessionToken}");

                    do
                    {
                        int length = await WaitAndReadLengthAsync();
                        ReadCommand(length);
                        ExecuteCommand();
                    } while (!isQuit && !_cancellationTokenSource.IsCancellationRequested);
                }
                catch (OperationCanceledException)
                {
                    Quit();
                }
                finally
                {
                    Console.WriteLine($"Session exited: {SessionToken}");
                }

                _client.Close();
                _client.Dispose();
            }

            private async Task<int> WaitAndReadLengthAsync()
            {
                try
                {
                    byte[] bytes = new byte[sizeof(int)];
                    await _stream.ReadAsync(bytes, 0, sizeof(int), _cancellationTokenSource.Token);
                    return BitConverter.ToInt32(bytes, 0);
                }
                catch(IOException) { }
                return 0;
            }

            private void ReadCommand(int length)
            {
                if (length > 0)
                {
                    var data = ReadDataFromStream(length);
                    _command = JsonSerializer.Deserialize<NetworkCommand>(data, options);
                }
            }

            private string ReadDataFromStream(int length)
            {
                string data = "";
                byte[] bytes = new byte[1024];
                while (length > 0)
                {
                    int read = length > bytes.Length ? bytes.Length : length;
                    int bytesRead = _stream.Read(bytes, 0, read);
                    data += Encoding.ASCII.GetString(bytes, 0, bytesRead);
                    length -= bytesRead;
                }

                return data;
            }

            private void ExecuteCommand()
            {
                _command?.Execute();
                WriteOutputToStream();

                if (_command != null)
                    Console.WriteLine(_command?.ToString());
            }

            private void WriteCommandToStream(string data)
            {
                CreateCommand(data);
                var json = JsonSerializer.Serialize<NetworkCommand>(_command, options);
                WriteDataToStream(json);
            }

            private void CreateCommand(string data)
                => _command = _commandFactory.CreateCommand(SessionToken, data);

            private void WriteOutputToStream()
            {
                if (_command != null)
                {
                    var json = JsonSerializer.Serialize<NetworkCommand>(_command?.ToOutput(), options);
                    WriteDataToStream(json);
                }
            }

            private void WriteDataToStream(string data)
            {
                try
                {
                    byte[] msg = Encoding.ASCII.GetBytes(data);
                    WriteLengthToStream(msg.Length);
                    _stream.Write(msg, 0, msg.Length);
                }
                catch (IOException) { }
            }

            private void WriteLengthToStream(int length)
            {
                byte[] lBytes = BitConverter.GetBytes(length);
                _stream.Write(lBytes, 0, lBytes.Length);
            }

            #region IClientHandler

            public IEnumerable<Session> Sessions()
                => _server.handlers.Select(h => new Session() { Token = h.SessionToken, EndPoint = h._client.Client.RemoteEndPoint });

            public void Quit()
            {
                isQuit = true;
                _server.handlers.Remove(this);
                WriteCommandToStream("quit");
            }

            public void Shutdown()
                => _cancellationTokenSource.Cancel();

            #endregion
        }
    }
}
