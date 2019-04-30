namespace BabaIsYou.Views {
	partial class SpecialEdit {
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
			this.lblType = new System.Windows.Forms.Label();
			this.grpType = new System.Windows.Forms.GroupBox();
			this.chkControls = new BabaIsYou.Controls.ColorRadioButton();
			this.chkFlower = new BabaIsYou.Controls.ColorRadioButton();
			this.chkLevel = new BabaIsYou.Controls.ColorRadioButton();
			this.lblRadius = new System.Windows.Forms.Label();
			this.lblControl = new System.Windows.Forms.Label();
			this.cboControls = new System.Windows.Forms.ComboBox();
			this.panelLevelEdit = new System.Windows.Forms.Panel();
			this.btnSave = new System.Windows.Forms.Button();
			this.imgColor = new System.Windows.Forms.PictureBox();
			this.lblColor = new System.Windows.Forms.Label();
			this.numRadius = new BabaIsYou.Controls.NumericBox();
			this.grpType.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.imgColor)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numRadius)).BeginInit();
			this.SuspendLayout();
			// 
			// lblType
			// 
			this.lblType.AutoSize = true;
			this.lblType.Location = new System.Drawing.Point(23, 23);
			this.lblType.Name = "lblType";
			this.lblType.Size = new System.Drawing.Size(31, 13);
			this.lblType.TabIndex = 0;
			this.lblType.Text = "Type";
			// 
			// grpType
			// 
			this.grpType.Controls.Add(this.chkControls);
			this.grpType.Controls.Add(this.chkFlower);
			this.grpType.Controls.Add(this.chkLevel);
			this.grpType.Location = new System.Drawing.Point(60, 12);
			this.grpType.Name = "grpType";
			this.grpType.Size = new System.Drawing.Size(191, 31);
			this.grpType.TabIndex = 1;
			this.grpType.TabStop = false;
			// 
			// chkControls
			// 
			this.chkControls.AutoSize = true;
			this.chkControls.BackColor = System.Drawing.Color.Transparent;
			this.chkControls.Location = new System.Drawing.Point(6, 9);
			this.chkControls.Name = "chkControls";
			this.chkControls.OffColour = System.Drawing.Color.Empty;
			this.chkControls.OnColor = System.Drawing.Color.Red;
			this.chkControls.Size = new System.Drawing.Size(63, 17);
			this.chkControls.TabIndex = 0;
			this.chkControls.TabStop = true;
			this.chkControls.Text = "Controls";
			this.chkControls.UseVisualStyleBackColor = true;
			this.chkControls.CheckedChanged += new System.EventHandler(this.chkType_CheckedChanged);
			// 
			// chkFlower
			// 
			this.chkFlower.AutoSize = true;
			this.chkFlower.BackColor = System.Drawing.Color.Transparent;
			this.chkFlower.Location = new System.Drawing.Point(75, 9);
			this.chkFlower.Name = "chkFlower";
			this.chkFlower.OffColour = System.Drawing.Color.Empty;
			this.chkFlower.OnColor = System.Drawing.Color.Red;
			this.chkFlower.Size = new System.Drawing.Size(56, 17);
			this.chkFlower.TabIndex = 1;
			this.chkFlower.TabStop = true;
			this.chkFlower.Text = "Flower";
			this.chkFlower.UseVisualStyleBackColor = true;
			this.chkFlower.CheckedChanged += new System.EventHandler(this.chkType_CheckedChanged);
			// 
			// chkLevel
			// 
			this.chkLevel.AutoSize = true;
			this.chkLevel.BackColor = System.Drawing.Color.Transparent;
			this.chkLevel.Location = new System.Drawing.Point(137, 9);
			this.chkLevel.Name = "chkLevel";
			this.chkLevel.OffColour = System.Drawing.Color.Empty;
			this.chkLevel.OnColor = System.Drawing.Color.Red;
			this.chkLevel.Size = new System.Drawing.Size(51, 17);
			this.chkLevel.TabIndex = 2;
			this.chkLevel.TabStop = true;
			this.chkLevel.Text = "Level";
			this.chkLevel.UseVisualStyleBackColor = true;
			this.chkLevel.CheckedChanged += new System.EventHandler(this.chkType_CheckedChanged);
			// 
			// lblRadius
			// 
			this.lblRadius.AutoSize = true;
			this.lblRadius.Location = new System.Drawing.Point(14, 57);
			this.lblRadius.Name = "lblRadius";
			this.lblRadius.Size = new System.Drawing.Size(40, 13);
			this.lblRadius.TabIndex = 5;
			this.lblRadius.Text = "Radius";
			this.lblRadius.Visible = false;
			// 
			// lblControl
			// 
			this.lblControl.AutoSize = true;
			this.lblControl.Location = new System.Drawing.Point(14, 57);
			this.lblControl.Name = "lblControl";
			this.lblControl.Size = new System.Drawing.Size(40, 13);
			this.lblControl.TabIndex = 3;
			this.lblControl.Text = "Control";
			this.lblControl.Visible = false;
			// 
			// cboControls
			// 
			this.cboControls.BackColor = System.Drawing.Color.White;
			this.cboControls.ForeColor = System.Drawing.Color.Black;
			this.cboControls.FormattingEnabled = true;
			this.cboControls.Location = new System.Drawing.Point(60, 54);
			this.cboControls.Name = "cboControls";
			this.cboControls.Size = new System.Drawing.Size(95, 21);
			this.cboControls.TabIndex = 4;
			this.cboControls.Visible = false;
			this.cboControls.SelectedIndexChanged += new System.EventHandler(this.cboControls_SelectedIndexChanged);
			// 
			// panelLevelEdit
			// 
			this.panelLevelEdit.Location = new System.Drawing.Point(0, 42);
			this.panelLevelEdit.Name = "panelLevelEdit";
			this.panelLevelEdit.Size = new System.Drawing.Size(321, 231);
			this.panelLevelEdit.TabIndex = 2;
			this.panelLevelEdit.Visible = false;
			// 
			// btnSave
			// 
			this.btnSave.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnSave.Location = new System.Drawing.Point(134, 281);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(51, 23);
			this.btnSave.TabIndex = 8;
			this.btnSave.Text = "Save";
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// imgColor
			// 
			this.imgColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.imgColor.Cursor = System.Windows.Forms.Cursors.Hand;
			this.imgColor.Location = new System.Drawing.Point(60, 81);
			this.imgColor.Name = "imgColor";
			this.imgColor.Size = new System.Drawing.Size(48, 48);
			this.imgColor.TabIndex = 25;
			this.imgColor.TabStop = false;
			this.imgColor.Click += new System.EventHandler(this.imgColor_Click);
			// 
			// lblColor
			// 
			this.lblColor.AutoSize = true;
			this.lblColor.Location = new System.Drawing.Point(23, 97);
			this.lblColor.Name = "lblColor";
			this.lblColor.Size = new System.Drawing.Size(31, 13);
			this.lblColor.TabIndex = 7;
			this.lblColor.Text = "Color";
			// 
			// numRadius
			// 
			this.numRadius.BackColor = System.Drawing.Color.White;
			this.numRadius.ForeColor = System.Drawing.Color.Black;
			this.numRadius.Location = new System.Drawing.Point(60, 55);
			this.numRadius.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
			this.numRadius.Name = "numRadius";
			this.numRadius.Size = new System.Drawing.Size(48, 20);
			this.numRadius.TabIndex = 6;
			this.numRadius.Visible = false;
			this.numRadius.ValueChanged += new System.EventHandler(this.numRadius_ValueChanged);
			// 
			// SpecialEdit
			// 
			this.AcceptButton = this.btnSave;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Black;
			this.ClientSize = new System.Drawing.Size(321, 314);
			this.Controls.Add(this.imgColor);
			this.Controls.Add(this.lblColor);
			this.Controls.Add(this.btnSave);
			this.Controls.Add(this.lblControl);
			this.Controls.Add(this.lblRadius);
			this.Controls.Add(this.numRadius);
			this.Controls.Add(this.lblType);
			this.Controls.Add(this.grpType);
			this.Controls.Add(this.cboControls);
			this.Controls.Add(this.panelLevelEdit);
			this.ForeColor = System.Drawing.Color.White;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "SpecialEdit";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Special Edit";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SpecialEdit_FormClosing);
			this.Load += new System.EventHandler(this.SpecialEdit_Load);
			this.grpType.ResumeLayout(false);
			this.grpType.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.imgColor)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numRadius)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblType;
		private System.Windows.Forms.GroupBox grpType;
		private Controls.ColorRadioButton chkControls;
		private Controls.ColorRadioButton chkFlower;
		private Controls.ColorRadioButton chkLevel;
		private System.Windows.Forms.Label lblRadius;
		private Controls.NumericBox numRadius;
		private System.Windows.Forms.Label lblControl;
		private System.Windows.Forms.ComboBox cboControls;
		private System.Windows.Forms.Panel panelLevelEdit;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.PictureBox imgColor;
		private System.Windows.Forms.Label lblColor;
	}
}