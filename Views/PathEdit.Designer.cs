namespace BabaIsYou.Views {
	partial class PathEdit {
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
			this.imgObject = new System.Windows.Forms.PictureBox();
			this.lblObject = new System.Windows.Forms.Label();
			this.lblGate = new System.Windows.Forms.Label();
			this.grpGate = new System.Windows.Forms.GroupBox();
			this.chkOrbs = new BabaIsYou.Controls.ColorRadioButton();
			this.chkNope = new BabaIsYou.Controls.ColorRadioButton();
			this.chkLevels = new BabaIsYou.Controls.ColorRadioButton();
			this.chkAreas = new BabaIsYou.Controls.ColorRadioButton();
			this.lblStyle = new System.Windows.Forms.Label();
			this.grpStyle = new System.Windows.Forms.GroupBox();
			this.chkHidden = new BabaIsYou.Controls.ColorRadioButton();
			this.chkVisible = new BabaIsYou.Controls.ColorRadioButton();
			this.btnSave = new System.Windows.Forms.Button();
			this.lblRequirement = new System.Windows.Forms.Label();
			this.numRequirement = new BabaIsYou.Controls.NumericBox();
			((System.ComponentModel.ISupportInitialize)(this.imgObject)).BeginInit();
			this.grpGate.SuspendLayout();
			this.grpStyle.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numRequirement)).BeginInit();
			this.SuspendLayout();
			// 
			// imgObject
			// 
			this.imgObject.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.imgObject.Cursor = System.Windows.Forms.Cursors.Hand;
			this.imgObject.Location = new System.Drawing.Point(17, 38);
			this.imgObject.Name = "imgObject";
			this.imgObject.Size = new System.Drawing.Size(48, 48);
			this.imgObject.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.imgObject.TabIndex = 23;
			this.imgObject.TabStop = false;
			this.imgObject.Click += new System.EventHandler(this.imgObject_Click);
			// 
			// lblObject
			// 
			this.lblObject.AutoSize = true;
			this.lblObject.Location = new System.Drawing.Point(20, 18);
			this.lblObject.Name = "lblObject";
			this.lblObject.Size = new System.Drawing.Size(43, 13);
			this.lblObject.TabIndex = 0;
			this.lblObject.Text = "Object";
			// 
			// lblGate
			// 
			this.lblGate.AutoSize = true;
			this.lblGate.Location = new System.Drawing.Point(102, 49);
			this.lblGate.Name = "lblGate";
			this.lblGate.Size = new System.Drawing.Size(43, 13);
			this.lblGate.TabIndex = 3;
			this.lblGate.Text = "Locked";
			// 
			// grpGate
			// 
			this.grpGate.Controls.Add(this.chkOrbs);
			this.grpGate.Controls.Add(this.chkNope);
			this.grpGate.Controls.Add(this.chkLevels);
			this.grpGate.Controls.Add(this.chkAreas);
			this.grpGate.Location = new System.Drawing.Point(151, 38);
			this.grpGate.Name = "grpGate";
			this.grpGate.Size = new System.Drawing.Size(242, 31);
			this.grpGate.TabIndex = 4;
			this.grpGate.TabStop = false;
			// 
			// chkOrbs
			// 
			this.chkOrbs.AutoSize = true;
			this.chkOrbs.BackColor = System.Drawing.Color.Transparent;
			this.chkOrbs.Location = new System.Drawing.Point(186, 9);
			this.chkOrbs.Name = "chkOrbs";
			this.chkOrbs.OffColour = System.Drawing.Color.Empty;
			this.chkOrbs.OnColor = System.Drawing.Color.Red;
			this.chkOrbs.Size = new System.Drawing.Size(49, 17);
			this.chkOrbs.TabIndex = 3;
			this.chkOrbs.TabStop = true;
			this.chkOrbs.Text = "Orbs";
			this.chkOrbs.UseVisualStyleBackColor = true;
			// 
			// chkNope
			// 
			this.chkNope.AutoSize = true;
			this.chkNope.BackColor = System.Drawing.Color.Transparent;
			this.chkNope.Location = new System.Drawing.Point(6, 9);
			this.chkNope.Name = "chkNope";
			this.chkNope.OffColour = System.Drawing.Color.Empty;
			this.chkNope.OnColor = System.Drawing.Color.Red;
			this.chkNope.Size = new System.Drawing.Size(49, 17);
			this.chkNope.TabIndex = 0;
			this.chkNope.TabStop = true;
			this.chkNope.Text = "Nope";
			this.chkNope.UseVisualStyleBackColor = true;
			this.chkNope.CheckedChanged += new System.EventHandler(this.chkGate_CheckedChanged);
			// 
			// chkLevels
			// 
			this.chkLevels.AutoSize = true;
			this.chkLevels.BackColor = System.Drawing.Color.Transparent;
			this.chkLevels.Location = new System.Drawing.Point(59, 9);
			this.chkLevels.Name = "chkLevels";
			this.chkLevels.OffColour = System.Drawing.Color.Empty;
			this.chkLevels.OnColor = System.Drawing.Color.Red;
			this.chkLevels.Size = new System.Drawing.Size(61, 17);
			this.chkLevels.TabIndex = 1;
			this.chkLevels.TabStop = true;
			this.chkLevels.Text = "Levels";
			this.chkLevels.UseVisualStyleBackColor = true;
			this.chkLevels.CheckedChanged += new System.EventHandler(this.chkGate_CheckedChanged);
			// 
			// chkAreas
			// 
			this.chkAreas.AutoSize = true;
			this.chkAreas.BackColor = System.Drawing.Color.Transparent;
			this.chkAreas.Location = new System.Drawing.Point(125, 9);
			this.chkAreas.Name = "chkAreas";
			this.chkAreas.OffColour = System.Drawing.Color.Empty;
			this.chkAreas.OnColor = System.Drawing.Color.Red;
			this.chkAreas.Size = new System.Drawing.Size(55, 17);
			this.chkAreas.TabIndex = 2;
			this.chkAreas.TabStop = true;
			this.chkAreas.Text = "Areas";
			this.chkAreas.UseVisualStyleBackColor = true;
			this.chkAreas.CheckedChanged += new System.EventHandler(this.chkGate_CheckedChanged);
			// 
			// lblStyle
			// 
			this.lblStyle.AutoSize = true;
			this.lblStyle.Location = new System.Drawing.Point(108, 19);
			this.lblStyle.Name = "lblStyle";
			this.lblStyle.Size = new System.Drawing.Size(37, 13);
			this.lblStyle.TabIndex = 1;
			this.lblStyle.Text = "Style";
			// 
			// grpStyle
			// 
			this.grpStyle.Controls.Add(this.chkHidden);
			this.grpStyle.Controls.Add(this.chkVisible);
			this.grpStyle.Location = new System.Drawing.Point(151, 8);
			this.grpStyle.Name = "grpStyle";
			this.grpStyle.Size = new System.Drawing.Size(141, 31);
			this.grpStyle.TabIndex = 2;
			this.grpStyle.TabStop = false;
			// 
			// chkHidden
			// 
			this.chkHidden.AutoSize = true;
			this.chkHidden.BackColor = System.Drawing.Color.Transparent;
			this.chkHidden.Location = new System.Drawing.Point(6, 9);
			this.chkHidden.Name = "chkHidden";
			this.chkHidden.OffColour = System.Drawing.Color.Empty;
			this.chkHidden.OnColor = System.Drawing.Color.Red;
			this.chkHidden.Size = new System.Drawing.Size(61, 17);
			this.chkHidden.TabIndex = 0;
			this.chkHidden.TabStop = true;
			this.chkHidden.Text = "Hidden";
			this.chkHidden.UseVisualStyleBackColor = true;
			this.chkHidden.CheckedChanged += new System.EventHandler(this.chkStyle_CheckedChanged);
			// 
			// chkVisible
			// 
			this.chkVisible.AutoSize = true;
			this.chkVisible.BackColor = System.Drawing.Color.Transparent;
			this.chkVisible.Location = new System.Drawing.Point(71, 9);
			this.chkVisible.Name = "chkVisible";
			this.chkVisible.OffColour = System.Drawing.Color.Empty;
			this.chkVisible.OnColor = System.Drawing.Color.Red;
			this.chkVisible.Size = new System.Drawing.Size(67, 17);
			this.chkVisible.TabIndex = 1;
			this.chkVisible.TabStop = true;
			this.chkVisible.Text = "Visible";
			this.chkVisible.UseVisualStyleBackColor = true;
			this.chkVisible.CheckedChanged += new System.EventHandler(this.chkStyle_CheckedChanged);
			// 
			// btnSave
			// 
			this.btnSave.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnSave.Location = new System.Drawing.Point(184, 105);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(51, 23);
			this.btnSave.TabIndex = 7;
			this.btnSave.Text = "Save";
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// lblRequirement
			// 
			this.lblRequirement.AutoSize = true;
			this.lblRequirement.Location = new System.Drawing.Point(72, 77);
			this.lblRequirement.Name = "lblRequirement";
			this.lblRequirement.Size = new System.Drawing.Size(73, 13);
			this.lblRequirement.TabIndex = 5;
			this.lblRequirement.Text = "Requirement";
			// 
			// numRequirement
			// 
			this.numRequirement.BackColor = System.Drawing.Color.White;
			this.numRequirement.ForeColor = System.Drawing.Color.Black;
			this.numRequirement.Location = new System.Drawing.Point(151, 75);
			this.numRequirement.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
			this.numRequirement.Name = "numRequirement";
			this.numRequirement.Size = new System.Drawing.Size(48, 20);
			this.numRequirement.TabIndex = 6;
			this.numRequirement.ValueChanged += new System.EventHandler(this.numRequirement_ValueChanged);
			// 
			// PathEdit
			// 
			this.AcceptButton = this.btnSave;
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.BackColor = System.Drawing.Color.Black;
			this.ClientSize = new System.Drawing.Size(419, 140);
			this.Controls.Add(this.lblRequirement);
			this.Controls.Add(this.numRequirement);
			this.Controls.Add(this.btnSave);
			this.Controls.Add(this.lblGate);
			this.Controls.Add(this.grpGate);
			this.Controls.Add(this.lblStyle);
			this.Controls.Add(this.grpStyle);
			this.Controls.Add(this.imgObject);
			this.Controls.Add(this.lblObject);
			this.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ForeColor = System.Drawing.Color.White;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "PathEdit";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Path Edit";
			this.Shown += new System.EventHandler(this.PathEdit_Shown);
			((System.ComponentModel.ISupportInitialize)(this.imgObject)).EndInit();
			this.grpGate.ResumeLayout(false);
			this.grpGate.PerformLayout();
			this.grpStyle.ResumeLayout(false);
			this.grpStyle.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.numRequirement)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.PictureBox imgObject;
		private System.Windows.Forms.Label lblObject;
		private System.Windows.Forms.Label lblGate;
		private System.Windows.Forms.GroupBox grpGate;
		private Controls.ColorRadioButton chkNope;
		private Controls.ColorRadioButton chkLevels;
		private Controls.ColorRadioButton chkAreas;
		private System.Windows.Forms.Label lblStyle;
		private System.Windows.Forms.GroupBox grpStyle;
		private Controls.ColorRadioButton chkHidden;
		private Controls.ColorRadioButton chkVisible;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.Label lblRequirement;
		private Controls.NumericBox numRequirement;
		private Controls.ColorRadioButton chkOrbs;
	}
}