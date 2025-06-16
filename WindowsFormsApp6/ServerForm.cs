using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Windows.Forms;

namespace WindowsFormsApp6
{
    public partial class ServerForm : Form
    {
        private TcpListener server;
        private readonly List<TcpClient> clients = new List<TcpClient>();
        private readonly List<Stroke> strokeList = new List<Stroke>();
        private readonly object lockObj = new object();
        private bool isRunning = false;

        public ServerForm()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (isRunning) return;

            server = new TcpListener(IPAddress.Any, 5000);
            server.Start();
            isRunning = true;
            Log("✅ Server started on port 5000");

            Thread listenerThread = new Thread(() =>
            {
                while (true)
                {
                    try
                    {
                        TcpClient client = server.AcceptTcpClient();
                        lock (lockObj)
                            clients.Add(client);

                        Thread clientThread = new Thread(() => HandleClient(client));
                        clientThread.IsBackground = true;
                        clientThread.Start();
                    }
                    catch (Exception ex)
                    {
                        Log($"❌ Server error: {ex.Message}");
                    }
                }
            });

            listenerThread.IsBackground = true;
            listenerThread.Start();
        }

        private void HandleClient(TcpClient client)
        {
            NetworkStream ns;
            try
            {
                ns = client.GetStream();
            }
            catch (Exception ex)
            {
                Log($"❌ Lỗi khi lấy stream từ client: {ex.Message}");
                lock (lockObj) clients.Remove(client);
                return;
            }

            BinaryFormatter fmt = new BinaryFormatter();
            bool hasLoggedConnected = false;

            try
            {
                while (true)
                {
                    SyncMessage msg = (SyncMessage)fmt.Deserialize(ns);

                    switch (msg.Type)
                    {
                        case SyncMessage.ActionType.Draw:
                            lock (lockObj) strokeList.Add(msg.StrokeData);
                            Broadcast(msg, client);
                            break;

                        case SyncMessage.ActionType.Delete:
                            lock (lockObj) strokeList.RemoveAll(s => s.Id == msg.StrokeIdToDelete);
                            Broadcast(msg, client);
                            break;

                        case SyncMessage.ActionType.FullSync:
                            Thread.Sleep(100);
                            SyncMessage fullSync = new SyncMessage
                            {
                                Type = SyncMessage.ActionType.FullSync,
                                AllStrokes = new List<Stroke>(strokeList)
                            };

                            try
                            {
                                fmt.Serialize(ns, fullSync);
                                if (!hasLoggedConnected)
                                {
                                    Log($"➡ Client connected: {client.Client.RemoteEndPoint}");
                                    hasLoggedConnected = true;
                                }
                            }
                            catch (Exception ex)
                            {
                                Log($"❌ Lỗi khi gửi FullSync: {ex.Message}");
                            }
                            break;
                    }
                }
            }
            catch
            {
                lock (lockObj) clients.Remove(client);
                Log($"⚠ Client disconnected: {client.Client.RemoteEndPoint}");
                client.Close();
            }
        }

        private void Broadcast(SyncMessage msg, TcpClient sender)
        {
            lock (lockObj)
            {
                foreach (TcpClient client in clients)
                {
                    if (client != sender && client.Connected)
                    {
                        try
                        {
                            NetworkStream ns = client.GetStream();
                            BinaryFormatter fmt = new BinaryFormatter();
                            fmt.Serialize(ns, msg);
                        }
                        catch
                        {
                            // Ignore broken client
                        }
                    }
                }
            }
        }

        private void Log(string message)
        {
            if (lstClients.InvokeRequired)
            {
                lstClients.Invoke(new Action(() => lstClients.Items.Add(message)));
            }
            else
            {
                lstClients.Items.Add(message);
            }
        }
    }
}
