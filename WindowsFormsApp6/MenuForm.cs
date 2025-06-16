using System;
using System.Net.Sockets;
using System.Windows.Forms;

namespace WindowsFormsApp6
{
    public partial class MenuForm : Form
    {
        private bool isServerStarted = false; 

        public MenuForm()
        {
            InitializeComponent();
        }

        private void btnClient_Click(object sender, EventArgs e)
        {
            
            if (!IsServerRunning() && !isServerStarted)
            {
                MessageBox.Show("⚠ Vui lòng khởi động server trước khi mở client!", "Chưa có server", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Form1 clientForm = new Form1();
            clientForm.Show();
        }

        private void btnServer_Click(object sender, EventArgs e)
        {
            if (IsServerRunning())
            {
                MessageBox.Show("⚠ Server đã được khởi động ở phiên bản khác!", "Đã có server", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            ServerForm serverForm = new ServerForm();
            serverForm.FormClosed += (s, args) => isServerStarted = false; 
            isServerStarted = true;
            serverForm.Show();
        }

        private bool IsServerRunning()
        {
            try
            {
                TcpClient check = new TcpClient("127.0.0.1", 5000);
                check.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
