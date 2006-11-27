namespace GeekTool
{
	partial class Main
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
			this.components = new System.ComponentModel.Container();
			this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.settingsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.exitMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.textLabel = new System.Windows.Forms.Label();
			this.contextMenuStrip.SuspendLayout();
			this.SuspendLayout();
			// 
			// contextMenuStrip
			// 
			this.contextMenuStrip.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.settingsMenuItem,
            this.exitMenuItem});
			this.contextMenuStrip.Name = "contextMenuStrip";
			this.contextMenuStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
			this.contextMenuStrip.ShowImageMargin = false;
			this.contextMenuStrip.ShowItemToolTips = false;
			this.contextMenuStrip.Size = new System.Drawing.Size(100, 48);
			// 
			// settingsMenuItem
			// 
			this.settingsMenuItem.Enabled = false;
			this.settingsMenuItem.Name = "settingsMenuItem";
			this.settingsMenuItem.Size = new System.Drawing.Size(99, 22);
			this.settingsMenuItem.Text = "Settings";
			this.settingsMenuItem.Visible = false;
			// 
			// exitMenuItem
			// 
			this.exitMenuItem.Name = "exitMenuItem";
			this.exitMenuItem.Size = new System.Drawing.Size(99, 22);
			this.exitMenuItem.Text = "Exit";
			this.exitMenuItem.Click += new System.EventHandler(this.exitMenuItem_Click);
			// 
			// textLabel
			// 
			this.textLabel.Location = new System.Drawing.Point(0, 0);
			this.textLabel.Margin = new System.Windows.Forms.Padding(0);
			this.textLabel.Name = "textLabel";
			this.textLabel.Size = new System.Drawing.Size(1, 1);
			this.textLabel.TabIndex = 1;
			this.textLabel.Text = "Refreshing...";
			// 
			// Main
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.White;
			this.ClientSize = new System.Drawing.Size(1, 1);
			this.ControlBox = false;
			this.Controls.Add(this.textLabel);
			this.DoubleBuffered = true;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "Main";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "Form1";
			this.TransparencyKey = System.Drawing.Color.Fuchsia;
			this.contextMenuStrip.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
		private System.Windows.Forms.ToolStripMenuItem settingsMenuItem;
		private System.Windows.Forms.ToolStripMenuItem exitMenuItem;
		private System.Windows.Forms.Label textLabel;
	}
}

