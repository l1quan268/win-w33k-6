
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections.Generic;

namespace WhiteboardServer
{
    class Program
    {
        static List<TcpClient> clients = new List<TcpClient>();

        static void Main(string[] args)
        {
            TcpListener server = new TcpListener(IPAddress.Any, 8888);
            server.Start();
            Console.WriteLine("Server started on port 8888.");

            while (true)
            {
                TcpClient client = server.AcceptTcpClient();
                clients.Add(client);
                Console.WriteLine("Client connected.");

                Thread clientThread = new Thread(() => HandleClient(client));
                clientThread.Start();
            }
        }

        static void HandleClient(TcpClient client)
        {
            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[1024];
            int bytesRead;

            while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
            {
                foreach (TcpClient c in clients)
                {
                    if (c != client)
                    {
                        NetworkStream s = c.GetStream();
                        s.Write(buffer, 0, bytesRead);
                    }
                }
            }

            clients.Remove(client);
            Console.WriteLine("Client disconnected.");
            client.Close();
        }
    }
}
