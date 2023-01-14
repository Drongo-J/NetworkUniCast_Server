using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var ipAddress = IPAddress.Parse("10.2.11.43");
            var port = 27001;
            using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP))
            {
                var ep = new IPEndPoint(ipAddress, port);
                socket.Bind(ep);
                socket.Listen(10); // maksimum 10 nefer sorgu gondere biler, server queue sinde max 10 nefer ola biler
                Console.WriteLine($"Listen over {socket.LocalEndPoint}");

                while (true)
                {
                    var client = socket.Accept();
                    Task.Run(() =>
                    {
                        Console.WriteLine($"{client.RemoteEndPoint} connected");
                        var length = 0;
                        var bytes = new byte[1024];
                        do
                        {
                            length = client.Receive(bytes);
                            var msg = Encoding.UTF8.GetString(bytes, 0, length);
                            Console.WriteLine($"Client : {client.RemoteEndPoint} : {msg}");
                            if (msg.ToLower() == "exit")
                            {
                                client.Shutdown(SocketShutdown.Both);
                                client.Dispose();
                                break;
                            }
                        } while (true);
                    });
                }
            }
        }
    }
}
