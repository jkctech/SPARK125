
namespace SPARK125
{
	partial class VirtualDisplay
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
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
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.cb_always_on_top = new System.Windows.Forms.CheckBox();
			this.tb_volume = new System.Windows.Forms.TrackBar();
			this.lbl_volume = new System.Windows.Forms.Label();
			this.lbl_squelch = new System.Windows.Forms.Label();
			this.tb_squelch = new System.Windows.Forms.TrackBar();
			this.cb_screenonly = new System.Windows.Forms.CheckBox();
			((System.ComponentModel.ISupportInitialize)(this.tb_volume)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.tb_squelch)).BeginInit();
			this.SuspendLayout();
			// 
			// cb_always_on_top
			// 
			this.cb_always_on_top.AutoSize = true;
			this.cb_always_on_top.Location = new System.Drawing.Point(190, 75);
			this.cb_always_on_top.Name = "cb_always_on_top";
			this.cb_always_on_top.Size = new System.Drawing.Size(96, 17);
			this.cb_always_on_top.TabIndex = 0;
			this.cb_always_on_top.Text = "Always on Top";
			this.cb_always_on_top.UseVisualStyleBackColor = true;
			this.cb_always_on_top.CheckedChanged += new System.EventHandler(this.cb_always_on_top_CheckedChanged);
			// 
			// tb_volume
			// 
			this.tb_volume.Location = new System.Drawing.Point(15, 121);
			this.tb_volume.Maximum = 15;
			this.tb_volume.Name = "tb_volume";
			this.tb_volume.Orientation = System.Windows.Forms.Orientation.Vertical;
			this.tb_volume.Size = new System.Drawing.Size(45, 274);
			this.tb_volume.TabIndex = 1;
			this.tb_volume.ValueChanged += new System.EventHandler(this.tb_volume_ValueChanged);
			// 
			// lbl_volume
			// 
			this.lbl_volume.AutoSize = true;
			this.lbl_volume.Location = new System.Drawing.Point(12, 105);
			this.lbl_volume.Name = "lbl_volume";
			this.lbl_volume.Size = new System.Drawing.Size(22, 13);
			this.lbl_volume.TabIndex = 2;
			this.lbl_volume.Text = "Vol";
			// 
			// lbl_squelch
			// 
			this.lbl_squelch.AutoSize = true;
			this.lbl_squelch.Location = new System.Drawing.Point(273, 105);
			this.lbl_squelch.Name = "lbl_squelch";
			this.lbl_squelch.Size = new System.Drawing.Size(22, 13);
			this.lbl_squelch.TabIndex = 4;
			this.lbl_squelch.Text = "Sql";
			// 
			// tb_squelch
			// 
			this.tb_squelch.Location = new System.Drawing.Point(261, 121);
			this.tb_squelch.Maximum = 15;
			this.tb_squelch.Name = "tb_squelch";
			this.tb_squelch.Orientation = System.Windows.Forms.Orientation.Vertical;
			this.tb_squelch.Size = new System.Drawing.Size(45, 274);
			this.tb_squelch.TabIndex = 3;
			this.tb_squelch.ValueChanged += new System.EventHandler(this.tb_squelch_ValueChanged);
			// 
			// cb_screenonly
			// 
			this.cb_screenonly.AutoSize = true;
			this.cb_screenonly.Location = new System.Drawing.Point(12, 75);
			this.cb_screenonly.Name = "cb_screenonly";
			this.cb_screenonly.Size = new System.Drawing.Size(89, 17);
			this.cb_screenonly.TabIndex = 5;
			this.cb_screenonly.Text = "Hide Controls";
			this.cb_screenonly.UseVisualStyleBackColor = true;
			this.cb_screenonly.CheckedChanged += new System.EventHandler(this.cb_screenonly_CheckedChanged);
			// 
			// VirtualDisplay
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(570, 585);
			this.Controls.Add(this.cb_screenonly);
			this.Controls.Add(this.lbl_squelch);
			this.Controls.Add(this.tb_squelch);
			this.Controls.Add(this.lbl_volume);
			this.Controls.Add(this.tb_volume);
			this.Controls.Add(this.cb_always_on_top);
			this.Name = "VirtualDisplay";
			this.Text = "VirtualControl";
			((System.ComponentModel.ISupportInitialize)(this.tb_volume)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.tb_squelch)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.CheckBox cb_always_on_top;
		private System.Windows.Forms.TrackBar tb_volume;
		private System.Windows.Forms.Label lbl_volume;
		private System.Windows.Forms.Label lbl_squelch;
		private System.Windows.Forms.TrackBar tb_squelch;
		private System.Windows.Forms.CheckBox cb_screenonly;
	}
}