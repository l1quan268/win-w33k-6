namespace WindowsFormsApp6
{
    partial class ServerForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.ListBox lstClients;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.btnStart = new System.Windows.Forms.Button();
            this.lstClients = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            
            this.btnStart.Location = new System.Drawing.Point(12, 12);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(120, 30);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "Start Server";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            
            this.lstClients.FormattingEnabled = true;
            this.lstClients.ItemHeight = 16;
            this.lstClients.Location = new System.Drawing.Point(12, 60);
            this.lstClients.Name = "lstClients";
            this.lstClients.Size = new System.Drawing.Size(400, 260);
            this.lstClients.TabIndex = 1;
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.ClientSize = new System.Drawing.Size(430, 340);
            this.Controls.Add(this.lstClients);
            this.Controls.Add(this.btnStart);
            this.Name = "ServerForm";
            this.Text = "Whiteboard Server";
            this.ResumeLayout(false);
        }
    }
}
