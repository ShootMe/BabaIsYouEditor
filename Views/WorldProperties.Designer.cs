namespace BabaIsYou.Views {
	partial class WorldProperties {
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
			this.lblName = new System.Windows.Forms.Label();
			this.txtName = new System.Windows.Forms.TextBox();
			this.txtStart = new System.Windows.Forms.TextBox();
			this.lblStart = new System.Windows.Forms.Label();
			this.txtFirst = new System.Windows.Forms.TextBox();
			this.lblFirst = new System.Windows.Forms.Label();
			this.btnSave = new System.Windows.Forms.Button();
			this.btnSetFirst = new System.Windows.Forms.Button();
			this.btnSetStart = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// lblName
			// 
			this.lblName.AutoSize = true;
			this.lblName.Location = new System.Drawing.Point(13, 17);
			this.lblName.Name = "lblName";
			this.lblName.Size = new System.Drawing.Size(66, 13);
			this.lblName.TabIndex = 0;
			this.lblName.Text = "World Name";
			// 
			// txtName
			// 
			this.txtName.BackColor = System.Drawing.Color.White;
			this.txtName.ForeColor = System.Drawing.Color.Black;
			this.txtName.Location = new System.Drawing.Point(85, 14);
			this.txtName.Name = "txtName";
			this.txtName.Size = new System.Drawing.Size(243, 20);
			this.txtName.TabIndex = 1;
			// 
			// txtStart
			// 
			this.txtStart.BackColor = System.Drawing.Color.Gray;
			this.txtStart.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtStart.ForeColor = System.Drawing.Color.Black;
			this.txtStart.Location = new System.Drawing.Point(85, 40);
			this.txtStart.Name = "txtStart";
			this.txtStart.ReadOnly = true;
			this.txtStart.Size = new System.Drawing.Size(243, 20);
			this.txtStart.TabIndex = 3;
			// 
			// lblStart
			// 
			this.lblStart.AutoSize = true;
			this.lblStart.Location = new System.Drawing.Point(21, 43);
			this.lblStart.Name = "lblStart";
			this.lblStart.Size = new System.Drawing.Size(58, 13);
			this.lblStart.TabIndex = 2;
			this.lblStart.Text = "Start Level";
			// 
			// txtFirst
			// 
			this.txtFirst.BackColor = System.Drawing.Color.Gray;
			this.txtFirst.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtFirst.ForeColor = System.Drawing.Color.Black;
			this.txtFirst.Location = new System.Drawing.Point(85, 66);
			this.txtFirst.Name = "txtFirst";
			this.txtFirst.ReadOnly = true;
			this.txtFirst.Size = new System.Drawing.Size(243, 20);
			this.txtFirst.TabIndex = 6;
			// 
			// lblFirst
			// 
			this.lblFirst.AutoSize = true;
			this.lblFirst.Location = new System.Drawing.Point(24, 69);
			this.lblFirst.Name = "lblFirst";
			this.lblFirst.Size = new System.Drawing.Size(55, 13);
			this.lblFirst.TabIndex = 5;
			this.lblFirst.Text = "First Level";
			// 
			// btnSave
			// 
			this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnSave.Location = new System.Drawing.Point(169, 100);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(51, 23);
			this.btnSave.TabIndex = 8;
			this.btnSave.Text = "Save";
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// btnSetFirst
			// 
			this.btnSetFirst.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnSetFirst.Location = new System.Drawing.Point(334, 64);
			this.btnSetFirst.Name = "btnSetFirst";
			this.btnSetFirst.Size = new System.Drawing.Size(68, 23);
			this.btnSetFirst.TabIndex = 7;
			this.btnSetFirst.TabStop = false;
			this.btnSetFirst.Text = "Set Level";
			this.btnSetFirst.Click += new System.EventHandler(this.btnSetLevel_Click);
			// 
			// btnSetStart
			// 
			this.btnSetStart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnSetStart.Location = new System.Drawing.Point(334, 38);
			this.btnSetStart.Name = "btnSetStart";
			this.btnSetStart.Size = new System.Drawing.Size(68, 23);
			this.btnSetStart.TabIndex = 4;
			this.btnSetStart.TabStop = false;
			this.btnSetStart.Text = "Set Level";
			this.btnSetStart.Click += new System.EventHandler(this.btnSetLevel_Click);
			// 
			// WorldProperties
			// 
			this.AcceptButton = this.btnSave;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Black;
			this.ClientSize = new System.Drawing.Size(414, 136);
			this.Controls.Add(this.btnSetStart);
			this.Controls.Add(this.btnSetFirst);
			this.Controls.Add(this.btnSave);
			this.Controls.Add(this.txtFirst);
			this.Controls.Add(this.lblFirst);
			this.Controls.Add(this.txtStart);
			this.Controls.Add(this.lblStart);
			this.Controls.Add(this.txtName);
			this.Controls.Add(this.lblName);
			this.ForeColor = System.Drawing.Color.White;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "WorldProperties";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "World Properties";
			this.Shown += new System.EventHandler(this.WorldProperties_Shown);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblName;
		private System.Windows.Forms.TextBox txtName;
		private System.Windows.Forms.TextBox txtStart;
		private System.Windows.Forms.Label lblStart;
		private System.Windows.Forms.TextBox txtFirst;
		private System.Windows.Forms.Label lblFirst;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.Button btnSetFirst;
		private System.Windows.Forms.Button btnSetStart;
	}
}