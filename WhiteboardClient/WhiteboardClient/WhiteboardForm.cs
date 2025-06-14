
using System;
using System.Drawing;
using System.Windows.Forms;

namespace WhiteboardClient
{
    public partial class WhiteboardForm : Form
    {
        private bool isDrawing = false;
        private Point lastPoint;
        private Pen pen = new Pen(Color.Black, 1);

        public WhiteboardForm()
        {
            InitializeComponent();
        }

        private void panelDraw_MouseDown(object sender, MouseEventArgs e)
        {
            isDrawing = true;
            lastPoint = e.Location;
        }

        private void panelDraw_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDrawing)
            {
                using (Graphics g = panelDraw.CreateGraphics())
                {
                    g.DrawLine(pen, lastPoint, e.Location);
                }
                lastPoint = e.Location;
                // TODO: Gửi dữ liệu đến server ở đây
            }
        }

        private void panelDraw_MouseUp(object sender, MouseEventArgs e)
        {
            isDrawing = false;
        }

        private void btnColor_Click(object sender, EventArgs e)
        {
            using (ColorDialog dlg = new ColorDialog())
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    pen.Color = dlg.Color;
                }
            }
        }

        private void rad_CheckedChanged(object sender, EventArgs e)
        {
            if (rad1.Checked) pen.Width = 1;
            else if (rad2.Checked) pen.Width = 2;
            else if (rad3.Checked) pen.Width = 3;
            else if (rad4.Checked) pen.Width = 4;
            else if (rad5.Checked) pen.Width = 5;
        }

        private void btnEnd_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
