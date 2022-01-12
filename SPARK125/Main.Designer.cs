
namespace SPARK125
{
    partial class Spark125
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Spark125));
			this.btn_serial_toggle = new System.Windows.Forms.Button();
			this.combo_serial_ports = new System.Windows.Forms.ComboBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.btn_serial_auto = new System.Windows.Forms.Button();
			this.btn_serial_refresh = new System.Windows.Forms.Button();
			this.tb_debug = new System.Windows.Forms.RichTextBox();
			this.btn_VirtualControl = new System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// btn_serial_toggle
			// 
			this.btn_serial_toggle.Location = new System.Drawing.Point(5, 44);
			this.btn_serial_toggle.Name = "btn_serial_toggle";
			this.btn_serial_toggle.Size = new System.Drawing.Size(132, 23);
			this.btn_serial_toggle.TabIndex = 0;
			this.btn_serial_toggle.Text = "Connect";
			this.btn_serial_toggle.UseVisualStyleBackColor = true;
			this.btn_serial_toggle.Click += new System.EventHandler(this.btn_serial_toggle_Click);
			// 
			// combo_serial_ports
			// 
			this.combo_serial_ports.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.combo_serial_ports.FormattingEnabled = true;
			this.combo_serial_ports.Location = new System.Drawing.Point(5, 17);
			this.combo_serial_ports.Name = "combo_serial_ports";
			this.combo_serial_ports.Size = new System.Drawing.Size(104, 21);
			this.combo_serial_ports.TabIndex = 1;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.btn_serial_auto);
			this.groupBox1.Controls.Add(this.btn_serial_refresh);
			this.groupBox1.Controls.Add(this.btn_serial_toggle);
			this.groupBox1.Controls.Add(this.combo_serial_ports);
			this.groupBox1.Location = new System.Drawing.Point(10, 10);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(142, 103);
			this.groupBox1.TabIndex = 2;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Connect";
			// 
			// btn_serial_auto
			// 
			this.btn_serial_auto.Enabled = false;
			this.btn_serial_auto.Location = new System.Drawing.Point(5, 73);
			this.btn_serial_auto.Name = "btn_serial_auto";
			this.btn_serial_auto.Size = new System.Drawing.Size(132, 23);
			this.btn_serial_auto.TabIndex = 4;
			this.btn_serial_auto.Text = "Auto Detect";
			this.btn_serial_auto.UseVisualStyleBackColor = true;
			this.btn_serial_auto.Click += new System.EventHandler(this.btn_serial_auto_Click);
			// 
			// btn_serial_refresh
			// 
			this.btn_serial_refresh.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
			this.btn_serial_refresh.Location = new System.Drawing.Point(114, 16);
			this.btn_serial_refresh.Name = "btn_serial_refresh";
			this.btn_serial_refresh.Size = new System.Drawing.Size(23, 23);
			this.btn_serial_refresh.TabIndex = 3;
			this.btn_serial_refresh.Text = "R";
			this.btn_serial_refresh.UseVisualStyleBackColor = true;
			this.btn_serial_refresh.Click += new System.EventHandler(this.btn_serial_refresh_Click);
			// 
			// tb_debug
			// 
			this.tb_debug.Location = new System.Drawing.Point(10, 119);
			this.tb_debug.Name = "tb_debug";
			this.tb_debug.Size = new System.Drawing.Size(416, 166);
			this.tb_debug.TabIndex = 3;
			this.tb_debug.Text = "";
			// 
			// btn_VirtualControl
			// 
			this.btn_VirtualControl.Enabled = false;
			this.btn_VirtualControl.Location = new System.Drawing.Point(158, 12);
			this.btn_VirtualControl.Name = "btn_VirtualControl";
			this.btn_VirtualControl.Size = new System.Drawing.Size(132, 23);
			this.btn_VirtualControl.TabIndex = 4;
			this.btn_VirtualControl.Text = "Virtual Control";
			this.btn_VirtualControl.UseVisualStyleBackColor = true;
			this.btn_VirtualControl.Click += new System.EventHandler(this.btn_VirtualControl_Click);
			// 
			// Spark125
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(438, 296);
			this.Controls.Add(this.btn_VirtualControl);
			this.Controls.Add(this.tb_debug);
			this.Controls.Add(this.groupBox1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.Name = "Spark125";
			this.Text = "SPARK125";
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_serial_toggle;
        private System.Windows.Forms.ComboBox combo_serial_ports;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btn_serial_refresh;
        private System.Windows.Forms.Button btn_serial_auto;
		private System.Windows.Forms.RichTextBox tb_debug;
		private System.Windows.Forms.Button btn_VirtualControl;
	}
}

