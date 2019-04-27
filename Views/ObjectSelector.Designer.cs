namespace BabaIsYou.Views {
	partial class ObjectSelector {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.listSelector = new BabaIsYou.Controls.ListPanel();
			this.SuspendLayout();
			// 
			// listSelector
			// 
			this.listSelector.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listSelector.DrawText = false;
			this.listSelector.Location = new System.Drawing.Point(0, 0);
			this.listSelector.Name = "listSelector";
			this.listSelector.Size = new System.Drawing.Size(352, 235);
			this.listSelector.TabIndex = 0;
			this.listSelector.ItemClicked += new BabaIsYou.Controls.ListPanel.ItemClickedEvent(this.listSelector_ItemClicked);
			// 
			// ObjectSelector
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Black;
			this.ClientSize = new System.Drawing.Size(352, 235);
			this.Controls.Add(this.listSelector);
			this.DoubleBuffered = true;
			this.ForeColor = System.Drawing.Color.White;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ObjectSelector";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Select";
			this.ResumeLayout(false);

		}

		#endregion

		private Controls.ListPanel listSelector;
	}
}