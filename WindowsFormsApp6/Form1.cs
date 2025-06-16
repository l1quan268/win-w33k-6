using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Windows.Forms;

namespace WindowsFormsApp6
{
    public partial class Form1 : Form
    {
        private TcpClient client;
        private NetworkStream netStream;
        private BinaryFormatter formatter = new BinaryFormatter();
        private List<Stroke> strokes = new List<Stroke>();
        private Stroke currentStroke;
        private bool isDrawing = false;
        private Pen currentPen = new Pen(Color.Black, 2);

        public Form1()
        {
            InitializeComponent();
            ConnectToServer();
            trackBar1.Value = 2;
            lblWidth.Text = "Width: 2";
            panel1.Paint += panel1_Paint;
        }

        private void ConnectToServer()
        {
            try
            {
                client = new TcpClient("127.0.0.1", 5000);
                netStream = client.GetStream();

                var msg = new SyncMessage { Type = SyncMessage.ActionType.FullSync };
                formatter.Serialize(netStream, msg);

                Thread receiveThread = new Thread(ReceiveData);
                receiveThread.IsBackground = true;
                receiveThread.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể kết nối đến server: " + ex.Message);
            }
        }

        private void ReceiveData()
        {
            try
            {
                while (true)
                {
                    SyncMessage msg = (SyncMessage)formatter.Deserialize(netStream);
                    switch (msg.Type)
                    {
                        case SyncMessage.ActionType.FullSync:
                            strokes = msg.AllStrokes;
                            break;
                        case SyncMessage.ActionType.Draw:
                            strokes.Add(msg.StrokeData);
                            break;
                        case SyncMessage.ActionType.Delete:
                            strokes.RemoveAll(s => s.Id == msg.StrokeIdToDelete);
                            break;
                    }

                    panel1.Invoke(new Action(() => panel1.Invalidate()));
                }
            }
            catch
            {
                MessageBox.Show("⚠ Mất kết nối đến server.");
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.White);

            foreach (var stroke in strokes)
            {
                using (Pen pen = new Pen(stroke.Color, stroke.Width))
                {
                    for (int i = 1; i < stroke.Points.Count; i++)
                    {
                        e.Graphics.DrawLine(pen, stroke.Points[i - 1], stroke.Points[i]);
                    }
                }
            }

            if (isDrawing && currentStroke != null)
            {
                using (Pen pen = new Pen(currentStroke.Color, currentStroke.Width))
                {
                    for (int i = 1; i < currentStroke.Points.Count; i++)
                    {
                        e.Graphics.DrawLine(pen, currentStroke.Points[i - 1], currentStroke.Points[i]);
                    }
                }
            }
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            isDrawing = true;
            currentStroke = new Stroke
            {
                Color = currentPen.Color,
                Width = currentPen.Width,
                Points = new List<Point> { e.Location }
            };
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (!isDrawing) return;
            currentStroke.Points.Add(e.Location);
            panel1.Invalidate();
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            if (!isDrawing) return;
            isDrawing = false;
            currentStroke.Points.Add(e.Location);
            strokes.Add(currentStroke);

            SyncMessage msg = new SyncMessage
            {
                Type = SyncMessage.ActionType.Draw,
                StrokeData = currentStroke
            };
            formatter.Serialize(netStream, msg);
            panel1.Invalidate();
        }

        private void btnColor_Click(object sender, EventArgs e)
        {
            using (ColorDialog dlg = new ColorDialog())
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    currentPen.Color = dlg.Color;
                }
            }
        }

        private void btnEraser_Click(object sender, EventArgs e)
        {
            currentPen.Color = Color.White;
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            currentPen.Width = trackBar1.Value;
            lblWidth.Text = $"Width: {trackBar1.Value}";
        }

        private void btnEnd_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn lưu hình ảnh trước khi thoát?", "Kết thúc", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                using (Bitmap bmp = new Bitmap(panel1.Width, panel1.Height))
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    panel1.DrawToBitmap(bmp, panel1.ClientRectangle);
                    SaveFileDialog sfd = new SaveFileDialog();
                    sfd.Filter = "PNG Image|*.png";
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        bmp.Save(sfd.FileName);
                    }
                }
            }
            Application.Exit();
        }

        private void btnInsertImage_Click(object sender, EventArgs e)
        {
            string url = Microsoft.VisualBasic.Interaction.InputBox("Nhập URL hình ảnh:", "Chèn ảnh", "http://");
            if (!string.IsNullOrWhiteSpace(url))
            {
                try
                {
                    System.Net.WebRequest request = System.Net.WebRequest.Create(url);
                    using (System.Net.WebResponse response = request.GetResponse())
                    using (System.IO.Stream stream = response.GetResponseStream())
                    {
                        Image img = Image.FromStream(stream);
                        Graphics g = panel1.CreateGraphics();
                        g.DrawImage(img, new Rectangle(50, 50, 200, 200));
                        g.Dispose();
                    }
                }
                catch
                {
                    MessageBox.Show("Không thể tải ảnh từ URL.");
                }
            }
        }
    }
}
    