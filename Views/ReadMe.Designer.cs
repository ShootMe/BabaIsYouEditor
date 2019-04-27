namespace BabaIsYou.Views {
	partial class ReadMe {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReadMe));
			this.imgBaba = new System.Windows.Forms.PictureBox();
			this.lblReadme = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.imgBaba)).BeginInit();
			this.SuspendLayout();
			// 
			// imgBaba
			// 
			this.imgBaba.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.imgBaba.Image = global::BabaIsYou.Properties.Resources.baba;
			this.imgBaba.Location = new System.Drawing.Point(238, 12);
			this.imgBaba.Name = "imgBaba";
			this.imgBaba.Size = new System.Drawing.Size(373, 88);
			this.imgBaba.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.imgBaba.TabIndex = 0;
			this.imgBaba.TabStop = false;
			// 
			// lblReadme
			// 
			this.lblReadme.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lblReadme.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblReadme.Location = new System.Drawing.Point(12, 103);
			this.lblReadme.Name = "lblReadme";
			this.lblReadme.Size = new System.Drawing.Size(824, 465);
			this.lblReadme.TabIndex = 1;
			this.lblReadme.Text = resources.GetString("lblReadme.Text");
			// 
			// ReadMe
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Black;
			this.ClientSize = new System.Drawing.Size(848, 577);
			this.Controls.Add(this.lblReadme);
			this.Controls.Add(this.imgBaba);
			this.ForeColor = System.Drawing.Color.White;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ReadMe";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Read Me";
			((System.ComponentModel.ISupportInitialize)(this.imgBaba)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.PictureBox imgBaba;
		private System.Windows.Forms.Label lblReadme;
	}
}