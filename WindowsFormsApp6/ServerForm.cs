using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Windows.Forms;
using System.Net.Mail;
using System.Drawing;
namespace WindowsFormsApp6
{
    public partial class ServerForm : Form
    {
        private TcpListener server;
        private readonly List<TcpClient> clients = new List<TcpClient>();
        private readonly List<Stroke> strokeList = new List<Stroke>();
        private readonly List<ImageData> imageList = new List<ImageData>();
        private readonly object lockObj = new object();
        private bool isRunning = false;
        private bool emailSent = false;
        public ServerForm()
        {
            InitializeComponent();
        }
        private void BroadcastClientCount()
        {
            SyncMessage countMsg = new SyncMessage
            {
                Type = SyncMessage.ActionType.ClientCount,
                ClientCount = clients.Count
            };
            Broadcast(countMsg, null);
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
                        BroadcastClientCount();
                        CheckClientLimit();
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
                lock (lockObj) clients.Remove(client);
                BroadcastClientCount();
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
                                lock (lockObj)
                                {
                                    foreach (var img in imageList)
                                    {
                                        SyncMessage imgMsg = new SyncMessage
                                        {
                                            Type = SyncMessage.ActionType.Image,
                                            ImageBytes = img.ImageBytes,
                                            ImageBounds = img.Bounds,
                                            ImageId = img.Id
                                        };
                                        fmt.Serialize(ns, imgMsg);
                                    }
                                }
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
                        case SyncMessage.ActionType.Image:
                            lock (lockObj)
                            {
                                var imgData = new ImageData
                                {
                                    Id = msg.ImageId,
                                    ImageBytes = msg.ImageBytes,
                                    Bounds = msg.ImageBounds
                                };
                                imageList.Add(imgData);
                            }
                            Broadcast(msg, client);
                            break;
                    }
                }
            }
            catch
            {
                lock (lockObj)
                {
                    clients.Remove(client);
                    if (clients.Count < 6) emailSent = false; 
                }
                BroadcastClientCount();
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
        private void CheckClientLimit()
        {
            if (clients.Count >=6 && !emailSent) 
            {
                try
                {
                    Log("📧 Đang gửi email cảnh báo...");
                    
                    using (var mail = new System.Net.Mail.MailMessage())
                    {
                        mail.From = new MailAddress("test@gmail.com");
                        mail.To.Add("admin@company.com");
                        mail.Subject = "Whiteboard Client Limit Warning";
                        mail.Body = $"Client limit reached: {clients.Count} clients connected";

                        using (var smtp = new System.Net.Mail.SmtpClient("smtp.gmail.com", 587))
                        {
                            smtp.Credentials = new System.Net.NetworkCredential("your-email@gmail.com", "your-password");
                            smtp.EnableSsl = true;
                            smtp.Send(mail);
                        }
                    }
                    emailSent = true;
                    Log("📧 Email cảnh báo đã được gửi!");
                }
                catch (Exception ex)
                {
                    Log($"❌ Lỗi gửi email: {ex.Message}");
                    Log($"⚠ Số lượng client đã đạt giới hạn {clients.Count}!");
                }
            }
        }
    }
    [Serializable]
    public class ImageData
    {
        public Guid Id { get; set; }
        public byte[] ImageBytes { get; set; }
        public Rectangle Bounds { get; set; }
    }
}
