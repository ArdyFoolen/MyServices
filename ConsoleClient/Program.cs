using MyServices;
using System.Text;

// IPCONFIG
// -- or --
// Use localhost

//AsyncClient asyncClient = new AsyncClient("127.0.0.1", 11000);
//AsyncClient asyncClient = new AsyncClient("172.31.64.1", 11000);
AsyncClient asyncClient = new AsyncClient("192.168.56.1", 11000);

byte[] msg = Encoding.ASCII.GetBytes("Get-Date");
await asyncClient.RunAsync();
