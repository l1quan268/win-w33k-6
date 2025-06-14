
namespace WhiteboardClient
{
    partial class WhiteboardForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel panelDraw;
        private System.Windows.Forms.Button btnEnd;
        private System.Windows.Forms.Button btnColor;
        private System.Windows.Forms.GroupBox groupBoxWidth;
        private System.Windows.Forms.RadioButton rad1;
        private System.Windows.Forms.RadioButton rad2;
        private System.Windows.Forms.RadioButton rad3;
        private System.Windows.Forms.RadioButton rad4;
        private System.Windows.Forms.RadioButton rad5;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.panelDraw = new System.Windows.Forms.Panel();
            this.btnEnd = new System.Windows.Forms.Button();
            this.btnColor = new System.Windows.Forms.Button();
            this.groupBoxWidth = new System.Windows.Forms.GroupBox();
            this.rad1 = new System.Windows.Forms.RadioButton();
            this.rad2 = new System.Windows.Forms.RadioButton();
            this.rad3 = new System.Windows.Forms.RadioButton();
            this.rad4 = new System.Windows.Forms.RadioButton();
            this.rad5 = new System.Windows.Forms.RadioButton();
            this.groupBoxWidth.SuspendLayout();
            this.SuspendLayout();

            // panelDraw
            this.panelDraw.BackColor = System.Drawing.Color.White;
            this.panelDraw.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelDraw.Location = new System.Drawing.Point(0, 0);
            this.panelDraw.Name = "panelDraw";
            this.panelDraw.Size = new System.Drawing.Size(584, 350);
            this.panelDraw.TabIndex = 0;
            this.panelDraw.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panelDraw_MouseDown);
            this.panelDraw.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panelDraw_MouseMove);
            this.panelDraw.MouseUp += new System.Windows.Forms.MouseEventHandler(this.panelDraw_MouseUp);

            // btnEnd
            this.btnEnd.Location = new System.Drawing.Point(20, 370);
            this.btnEnd.Name = "btnEnd";
            this.btnEnd.Size = new System.Drawing.Size(75, 30);
            this.btnEnd.TabIndex = 1;
            this.btnEnd.Text = "END";
            this.btnEnd.UseVisualStyleBackColor = true;
            this.btnEnd.Click += new System.EventHandler(this.btnEnd_Click);

            // btnColor
            this.btnColor.Location = new System.Drawing.Point(110, 370);
            this.btnColor.Name = "btnColor";
            this.btnColor.Size = new System.Drawing.Size(75, 30);
            this.btnColor.TabIndex = 2;
            this.btnColor.Text = "COLOR";
            this.btnColor.UseVisualStyleBackColor = true;
            this.btnColor.Click += new System.EventHandler(this.btnColor_Click);

            // groupBoxWidth
            this.groupBoxWidth.Controls.Add(this.rad1);
            this.groupBoxWidth.Controls.Add(this.rad2);
            this.groupBoxWidth.Controls.Add(this.rad3);
            this.groupBoxWidth.Controls.Add(this.rad4);
            this.groupBoxWidth.Controls.Add(this.rad5);
            this.groupBoxWidth.Location = new System.Drawing.Point(200, 360);
            this.groupBoxWidth.Name = "groupBoxWidth";
            this.groupBoxWidth.Size = new System.Drawing.Size(300, 50);
            this.groupBoxWidth.TabIndex = 3;
            this.groupBoxWidth.TabStop = false;
            this.groupBoxWidth.Text = "Width";

            // rad1
            this.rad1.Text = "1";
            this.rad1.Checked = true;
            this.rad1.Location = new System.Drawing.Point(10, 20);
            this.rad1.CheckedChanged += new System.EventHandler(this.rad_CheckedChanged);

            // rad2
            this.rad2.Text = "2";
            this.rad2.Location = new System.Drawing.Point(60, 20);
            this.rad2.CheckedChanged += new System.EventHandler(this.rad_CheckedChanged);

            // rad3
            this.rad3.Text = "3";
            this.rad3.Location = new System.Drawing.Point(110, 20);
            this.rad3.CheckedChanged += new System.EventHandler(this.rad_CheckedChanged);

            // rad4
            this.rad4.Text = "4";
            this.rad4.Location = new System.Drawing.Point(160, 20);
            this.rad4.CheckedChanged += new System.EventHandler(this.rad_CheckedChanged);

            // rad5
            this.rad5.Text = "5";
            this.rad5.Location = new System.Drawing.Point(210, 20);
            this.rad5.CheckedChanged += new System.EventHandler(this.rad_CheckedChanged);

            // WhiteboardForm
            this.ClientSize = new System.Drawing.Size(584, 421);
            this.Controls.Add(this.groupBoxWidth);
            this.Controls.Add(this.btnColor);
            this.Controls.Add(this.btnEnd);
            this.Controls.Add(this.panelDraw);
            this.Name = "WhiteboardForm";
            this.Text = "Whiteboard Client";
            this.groupBoxWidth.ResumeLayout(false);
            this.ResumeLayout(false);
        }
    }
}
