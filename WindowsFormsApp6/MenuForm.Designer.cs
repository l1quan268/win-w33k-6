namespace WindowsFormsApp6
{
    partial class MenuForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Button btnClient;
        private System.Windows.Forms.Button btnServer;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.btnClient = new System.Windows.Forms.Button();
            this.btnServer = new System.Windows.Forms.Button();
            this.SuspendLayout();
            
            this.btnClient.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnClient.Location = new System.Drawing.Point(40, 30);
            this.btnClient.Name = "btnClient";
            this.btnClient.Size = new System.Drawing.Size(180, 40);
            this.btnClient.TabIndex = 0;
            this.btnClient.Text = "Run as Client";
            this.btnClient.UseVisualStyleBackColor = true;
            this.btnClient.Click += new System.EventHandler(this.btnClient_Click);
           
            this.btnServer.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnServer.Location = new System.Drawing.Point(40, 90);
            this.btnServer.Name = "btnServer";
            this.btnServer.Size = new System.Drawing.Size(180, 40);
            this.btnServer.TabIndex = 1;
            this.btnServer.Text = "Run as Server";
            this.btnServer.UseVisualStyleBackColor = true;
            this.btnServer.Click += new System.EventHandler(this.btnServer_Click);
            
            this.ClientSize = new System.Drawing.Size(260, 170);
            this.Controls.Add(this.btnServer);
            this.Controls.Add(this.btnClient);
            this.Name = "MenuForm";
            this.Text = "Whiteboard Launcher";
            this.ResumeLayout(false);
        }
    }
}
