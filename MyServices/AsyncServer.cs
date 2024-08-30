using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MyServices
{
    public partial class AsyncServer
    {
        private int port;
        private CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private List<ClientHandler> handlers = new List<ClientHandler>();

        public AsyncServer(int port)
        {
            this.port = port;
        }

        public async Task RunAsync()
        {
            TcpListener tcpListener = new TcpListener(IPAddress.Any, this.port);
            tcpListener.Start();

            try
            {
                Console.WriteLine("Waiting for connections...");
                while (true)
                {
                    var client = await tcpListener.AcceptTcpClientAsync(cancellationTokenSource.Token);

                    var handler = new ClientHandler(this, client, cancellationTokenSource);
                    handler.Start();
                    handlers.Add(handler);
                }
            }
            catch (OperationCanceledException) { }

            await Task.WhenAll(handlers.Select(async h => await h.Await()));
            tcpListener.Stop();
        }
    }
}
