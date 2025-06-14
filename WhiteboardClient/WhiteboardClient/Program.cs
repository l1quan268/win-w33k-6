
using System;
using System.Drawing;
using System.Net.Sockets;
using System.Text.Json;
using System.Threading;
using System.Windows.Forms;

namespace WhiteboardClient
{
    public class DrawData
    {
        public int X, Y;
        public string Color;
        public int Width;
    }

    public class ClientForm : Form
    {
        TcpClient client;
        NetworkStream stream;
        Panel panel;
        Pen pen = new Pen(Color.Black, 2);
        bool isDrawing = false;

        public ClientForm()
        {
            this.Text = "Whiteboard Client";
            this.Size = new Size(800, 600);

            panel = new Panel() { Dock = DockStyle.Fill, BackColor = Color.White };
            panel.MouseDown += (s, e) => { isDrawing = true; SendDrawData(e.Location); };
            panel.MouseUp += (s, e) => isDrawing = false;
            panel.MouseMove += (s, e) => { if (isDrawing) { SendDrawData(e.Location); } };

            this.Controls.Add(panel);

            ConnectToServer();
        }

        void ConnectToServer()
        {
            client = new TcpClient("127.0.0.1", 8888);
            stream = client.GetStream();

            Thread t = new Thread(ReceiveData);
            t.IsBackground = true;
            t.Start();
        }

        void SendDrawData(Point p)
        {
            DrawData data = new DrawData()
            {
                X = p.X,
                Y = p.Y,
                Color = pen.Color.ToArgb().ToString(),
                Width = (int)pen.Width
            };
            string json = JsonSerializer.Serialize(data);
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(json);
            stream.Write(bytes, 0, bytes.Length);
        }

        void ReceiveData()
        {
            byte[] buffer = new byte[1024];
            while (true)
            {
                int bytes = stream.Read(buffer, 0, buffer.Length);
                string json = System.Text.Encoding.UTF8.GetString(buffer, 0, bytes);
                DrawData data = JsonSerializer.Deserialize<DrawData>(json);
                Graphics g = panel.CreateGraphics();
                Pen receivedPen = new Pen(Color.FromArgb(int.Parse(data.Color)), data.Width);
                g.DrawEllipse(receivedPen, data.X, data.Y, 2, 2);
            }
        }

        [STAThread]
        static void Main()
        {
            Application.Run(new ClientForm());
        }
    }
}
