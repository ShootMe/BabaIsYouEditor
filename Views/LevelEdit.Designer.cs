namespace BabaIsYou.Views {
	partial class LevelEdit {
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
			this.txtFile = new System.Windows.Forms.TextBox();
			this.lblFile = new System.Windows.Forms.Label();
			this.imgClearColor = new System.Windows.Forms.PictureBox();
			this.imgColor = new System.Windows.Forms.PictureBox();
			this.lblClearColor = new System.Windows.Forms.Label();
			this.lblColor = new System.Windows.Forms.Label();
			this.numNumber = new BabaIsYou.Controls.NumericBox();
			this.lblNumber = new System.Windows.Forms.Label();
			this.chkNumber = new BabaIsYou.Controls.ColorRadioButton();
			this.chkLetter = new BabaIsYou.Controls.ColorRadioButton();
			this.chkDot = new BabaIsYou.Controls.ColorRadioButton();
			this.chkIcon = new BabaIsYou.Controls.ColorRadioButton();
			this.imgIcon = new System.Windows.Forms.PictureBox();
			this.lblIcon = new System.Windows.Forms.Label();
			this.grpStyle = new System.Windows.Forms.GroupBox();
			this.lblStyle = new System.Windows.Forms.Label();
			this.lblState = new System.Windows.Forms.Label();
			this.grpState = new System.Windows.Forms.GroupBox();
			this.chkHidden = new BabaIsYou.Controls.ColorRadioButton();
			this.chkNormal = new BabaIsYou.Controls.ColorRadioButton();
			this.chkOpened = new BabaIsYou.Controls.ColorRadioButton();
			this.btnSetLevel = new System.Windows.Forms.Button();
			this.btnSave = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.imgClearColor)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.imgColor)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numNumber)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.imgIcon)).BeginInit();
			this.grpStyle.SuspendLayout();
			this.grpState.SuspendLayout();
			this.SuspendLayout();
			// 
			// lblName
			// 
			this.lblName.AutoSize = true;
			this.lblName.Location = new System.Drawing.Point(19, 15);
			this.lblName.Name = "lblName";
			this.lblName.Size = new System.Drawing.Size(35, 13);
			this.lblName.TabIndex = 0;
			this.lblName.Text = "Name";
			// 
			// txtName
			// 
			this.txtName.BackColor = System.Drawing.Color.Gray;
			this.txtName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtName.CharacterCasing = System.Windows.Forms.CharacterCasing.Lower;
			this.txtName.ForeColor = System.Drawing.Color.Black;
			this.txtName.Location = new System.Drawing.Point(60, 12);
			this.txtName.Name = "txtName";
			this.txtName.ReadOnly = true;
			this.txtName.Size = new System.Drawing.Size(245, 20);
			this.txtName.TabIndex = 1;
			this.txtName.TabStop = false;
			// 
			// txtFile
			// 
			this.txtFile.BackColor = System.Drawing.Color.Gray;
			this.txtFile.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtFile.ForeColor = System.Drawing.Color.Black;
			this.txtFile.Location = new System.Drawing.Point(60, 38);
			this.txtFile.Name = "txtFile";
			this.txtFile.ReadOnly = true;
			this.txtFile.Size = new System.Drawing.Size(171, 20);
			this.txtFile.TabIndex = 3;
			this.txtFile.TabStop = false;
			// 
			// lblFile
			// 
			this.lblFile.AutoSize = true;
			this.lblFile.Location = new System.Drawing.Point(31, 41);
			this.lblFile.Name = "lblFile";
			this.lblFile.Size = new System.Drawing.Size(23, 13);
			this.lblFile.TabIndex = 2;
			this.lblFile.Text = "File";
			// 
			// imgClearColor
			// 
			this.imgClearColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.imgClearColor.Cursor = System.Windows.Forms.Cursors.Hand;
			this.imgClearColor.Location = new System.Drawing.Point(133, 170);
			this.imgClearColor.Name = "imgClearColor";
			this.imgClearColor.Size = new System.Drawing.Size(48, 48);
			this.imgClearColor.TabIndex = 13;
			this.imgClearColor.TabStop = false;
			this.imgClearColor.Click += new System.EventHandler(this.imgColor_Click);
			// 
			// imgColor
			// 
			this.imgColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.imgColor.Cursor = System.Windows.Forms.Cursors.Hand;
			this.imgColor.Location = new System.Drawing.Point(60, 170);
			this.imgColor.Name = "imgColor";
			this.imgColor.Size = new System.Drawing.Size(48, 48);
			this.imgColor.TabIndex = 12;
			this.imgColor.TabStop = false;
			this.imgColor.Click += new System.EventHandler(this.imgColor_Click);
			// 
			// lblClearColor
			// 
			this.lblClearColor.AutoSize = true;
			this.lblClearColor.Location = new System.Drawing.Point(128, 150);
			this.lblClearColor.Name = "lblClearColor";
			this.lblClearColor.Size = new System.Drawing.Size(58, 13);
			this.lblClearColor.TabIndex = 12;
			this.lblClearColor.Text = "Clear Color";
			// 
			// lblColor
			// 
			this.lblColor.AutoSize = true;
			this.lblColor.Location = new System.Drawing.Point(70, 150);
			this.lblColor.Name = "lblColor";
			this.lblColor.Size = new System.Drawing.Size(31, 13);
			this.lblColor.TabIndex = 11;
			this.lblColor.Text = "Color";
			// 
			// numNumber
			// 
			this.numNumber.BackColor = System.Drawing.Color.White;
			this.numNumber.ForeColor = System.Drawing.Color.Black;
			this.numNumber.Location = new System.Drawing.Point(60, 64);
			this.numNumber.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
			this.numNumber.Name = "numNumber";
			this.numNumber.Size = new System.Drawing.Size(48, 20);
			this.numNumber.TabIndex = 6;
			this.numNumber.ValueChanged += new System.EventHandler(this.numNumber_ValueChanged);
			// 
			// lblNumber
			// 
			this.lblNumber.AutoSize = true;
			this.lblNumber.Location = new System.Drawing.Point(10, 66);
			this.lblNumber.Name = "lblNumber";
			this.lblNumber.Size = new System.Drawing.Size(44, 13);
			this.lblNumber.TabIndex = 5;
			this.lblNumber.Text = "Number";
			// 
			// chkNumber
			// 
			this.chkNumber.AutoSize = true;
			this.chkNumber.BackColor = System.Drawing.Color.Transparent;
			this.chkNumber.Location = new System.Drawing.Point(6, 9);
			this.chkNumber.Name = "chkNumber";
			this.chkNumber.OffColour = System.Drawing.Color.Empty;
			this.chkNumber.OnColor = System.Drawing.Color.Red;
			this.chkNumber.Size = new System.Drawing.Size(62, 17);
			this.chkNumber.TabIndex = 0;
			this.chkNumber.TabStop = true;
			this.chkNumber.Text = "Number";
			this.chkNumber.UseVisualStyleBackColor = true;
			this.chkNumber.CheckedChanged += new System.EventHandler(this.chkStyle_CheckedChanged);
			// 
			// chkLetter
			// 
			this.chkLetter.AutoSize = true;
			this.chkLetter.BackColor = System.Drawing.Color.Transparent;
			this.chkLetter.Location = new System.Drawing.Point(78, 9);
			this.chkLetter.Name = "chkLetter";
			this.chkLetter.OffColour = System.Drawing.Color.Empty;
			this.chkLetter.OnColor = System.Drawing.Color.Red;
			this.chkLetter.Size = new System.Drawing.Size(52, 17);
			this.chkLetter.TabIndex = 1;
			this.chkLetter.TabStop = true;
			this.chkLetter.Text = "Letter";
			this.chkLetter.UseVisualStyleBackColor = true;
			this.chkLetter.CheckedChanged += new System.EventHandler(this.chkStyle_CheckedChanged);
			// 
			// chkDot
			// 
			this.chkDot.AutoSize = true;
			this.chkDot.BackColor = System.Drawing.Color.Transparent;
			this.chkDot.Location = new System.Drawing.Point(140, 9);
			this.chkDot.Name = "chkDot";
			this.chkDot.OffColour = System.Drawing.Color.Empty;
			this.chkDot.OnColor = System.Drawing.Color.Red;
			this.chkDot.Size = new System.Drawing.Size(42, 17);
			this.chkDot.TabIndex = 2;
			this.chkDot.TabStop = true;
			this.chkDot.Text = "Dot";
			this.chkDot.UseVisualStyleBackColor = true;
			this.chkDot.CheckedChanged += new System.EventHandler(this.chkStyle_CheckedChanged);
			// 
			// chkIcon
			// 
			this.chkIcon.AutoSize = true;
			this.chkIcon.BackColor = System.Drawing.Color.Transparent;
			this.chkIcon.Location = new System.Drawing.Point(192, 9);
			this.chkIcon.Name = "chkIcon";
			this.chkIcon.OffColour = System.Drawing.Color.Empty;
			this.chkIcon.OnColor = System.Drawing.Color.Red;
			this.chkIcon.Size = new System.Drawing.Size(46, 17);
			this.chkIcon.TabIndex = 3;
			this.chkIcon.TabStop = true;
			this.chkIcon.Text = "Icon";
			this.chkIcon.UseVisualStyleBackColor = true;
			this.chkIcon.CheckedChanged += new System.EventHandler(this.chkStyle_CheckedChanged);
			// 
			// imgIcon
			// 
			this.imgIcon.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.imgIcon.Cursor = System.Windows.Forms.Cursors.Hand;
			this.imgIcon.Location = new System.Drawing.Point(202, 170);
			this.imgIcon.Name = "imgIcon";
			this.imgIcon.Size = new System.Drawing.Size(48, 48);
			this.imgIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.imgIcon.TabIndex = 21;
			this.imgIcon.TabStop = false;
			this.imgIcon.Click += new System.EventHandler(this.imgIcon_Click);
			// 
			// lblIcon
			// 
			this.lblIcon.AutoSize = true;
			this.lblIcon.Location = new System.Drawing.Point(212, 150);
			this.lblIcon.Name = "lblIcon";
			this.lblIcon.Size = new System.Drawing.Size(28, 13);
			this.lblIcon.TabIndex = 13;
			this.lblIcon.Text = "Icon";
			// 
			// grpStyle
			// 
			this.grpStyle.Controls.Add(this.chkNumber);
			this.grpStyle.Controls.Add(this.chkLetter);
			this.grpStyle.Controls.Add(this.chkDot);
			this.grpStyle.Controls.Add(this.chkIcon);
			this.grpStyle.Location = new System.Drawing.Point(60, 84);
			this.grpStyle.Name = "grpStyle";
			this.grpStyle.Size = new System.Drawing.Size(245, 31);
			this.grpStyle.TabIndex = 8;
			this.grpStyle.TabStop = false;
			// 
			// lblStyle
			// 
			this.lblStyle.AutoSize = true;
			this.lblStyle.Location = new System.Drawing.Point(24, 95);
			this.lblStyle.Name = "lblStyle";
			this.lblStyle.Size = new System.Drawing.Size(30, 13);
			this.lblStyle.TabIndex = 7;
			this.lblStyle.Text = "Style";
			// 
			// lblState
			// 
			this.lblState.AutoSize = true;
			this.lblState.Location = new System.Drawing.Point(22, 125);
			this.lblState.Name = "lblState";
			this.lblState.Size = new System.Drawing.Size(32, 13);
			this.lblState.TabIndex = 9;
			this.lblState.Text = "State";
			// 
			// grpState
			// 
			this.grpState.Controls.Add(this.chkHidden);
			this.grpState.Controls.Add(this.chkNormal);
			this.grpState.Controls.Add(this.chkOpened);
			this.grpState.Location = new System.Drawing.Point(60, 114);
			this.grpState.Name = "grpState";
			this.grpState.Size = new System.Drawing.Size(245, 31);
			this.grpState.TabIndex = 10;
			this.grpState.TabStop = false;
			// 
			// chkHidden
			// 
			this.chkHidden.AutoSize = true;
			this.chkHidden.BackColor = System.Drawing.Color.Transparent;
			this.chkHidden.Location = new System.Drawing.Point(6, 9);
			this.chkHidden.Name = "chkHidden";
			this.chkHidden.OffColour = System.Drawing.Color.Empty;
			this.chkHidden.OnColor = System.Drawing.Color.Red;
			this.chkHidden.Size = new System.Drawing.Size(59, 17);
			this.chkHidden.TabIndex = 0;
			this.chkHidden.TabStop = true;
			this.chkHidden.Text = "Hidden";
			this.chkHidden.UseVisualStyleBackColor = true;
			this.chkHidden.CheckedChanged += new System.EventHandler(this.chkState_CheckedChanged);
			// 
			// chkNormal
			// 
			this.chkNormal.AutoSize = true;
			this.chkNormal.BackColor = System.Drawing.Color.Transparent;
			this.chkNormal.Location = new System.Drawing.Point(91, 9);
			this.chkNormal.Name = "chkNormal";
			this.chkNormal.OffColour = System.Drawing.Color.Empty;
			this.chkNormal.OnColor = System.Drawing.Color.Red;
			this.chkNormal.Size = new System.Drawing.Size(58, 17);
			this.chkNormal.TabIndex = 1;
			this.chkNormal.TabStop = true;
			this.chkNormal.Text = "Normal";
			this.chkNormal.UseVisualStyleBackColor = true;
			this.chkNormal.CheckedChanged += new System.EventHandler(this.chkState_CheckedChanged);
			// 
			// chkOpened
			// 
			this.chkOpened.AutoSize = true;
			this.chkOpened.BackColor = System.Drawing.Color.Transparent;
			this.chkOpened.Location = new System.Drawing.Point(175, 9);
			this.chkOpened.Name = "chkOpened";
			this.chkOpened.OffColour = System.Drawing.Color.Empty;
			this.chkOpened.OnColor = System.Drawing.Color.Red;
			this.chkOpened.Size = new System.Drawing.Size(63, 17);
			this.chkOpened.TabIndex = 2;
			this.chkOpened.TabStop = true;
			this.chkOpened.Text = "Opened";
			this.chkOpened.UseVisualStyleBackColor = true;
			this.chkOpened.CheckedChanged += new System.EventHandler(this.chkState_CheckedChanged);
			// 
			// btnSetLevel
			// 
			this.btnSetLevel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnSetLevel.Location = new System.Drawing.Point(237, 36);
			this.btnSetLevel.Name = "btnSetLevel";
			this.btnSetLevel.Size = new System.Drawing.Size(68, 23);
			this.btnSetLevel.TabIndex = 4;
			this.btnSetLevel.TabStop = false;
			this.btnSetLevel.Text = "Set Level";
			this.btnSetLevel.Click += new System.EventHandler(this.btnSetLevel_Click);
			// 
			// btnSave
			// 
			this.btnSave.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnSave.Location = new System.Drawing.Point(132, 227);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(51, 23);
			this.btnSave.TabIndex = 22;
			this.btnSave.Text = "Save";
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// LevelEdit
			// 
			this.AcceptButton = this.btnSave;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Black;
			this.ClientSize = new System.Drawing.Size(321, 262);
			this.Controls.Add(this.btnSave);
			this.Controls.Add(this.btnSetLevel);
			this.Controls.Add(this.lblState);
			this.Controls.Add(this.grpState);
			this.Controls.Add(this.lblStyle);
			this.Controls.Add(this.grpStyle);
			this.Controls.Add(this.imgIcon);
			this.Controls.Add(this.lblIcon);
			this.Controls.Add(this.lblNumber);
			this.Controls.Add(this.numNumber);
			this.Controls.Add(this.imgClearColor);
			this.Controls.Add(this.imgColor);
			this.Controls.Add(this.lblClearColor);
			this.Controls.Add(this.lblColor);
			this.Controls.Add(this.txtFile);
			this.Controls.Add(this.lblFile);
			this.Controls.Add(this.txtName);
			this.Controls.Add(this.lblName);
			this.ForeColor = System.Drawing.Color.White;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "LevelEdit";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Level Edit";
			this.Shown += new System.EventHandler(this.LevelEdit_Shown);
			((System.ComponentModel.ISupportInitialize)(this.imgClearColor)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.imgColor)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numNumber)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.imgIcon)).EndInit();
			this.grpStyle.ResumeLayout(false);
			this.grpStyle.PerformLayout();
			this.grpState.ResumeLayout(false);
			this.grpState.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblName;
		private System.Windows.Forms.TextBox txtName;
		private System.Windows.Forms.TextBox txtFile;
		private System.Windows.Forms.Label lblFile;
		private System.Windows.Forms.PictureBox imgClearColor;
		private System.Windows.Forms.PictureBox imgColor;
		private System.Windows.Forms.Label lblClearColor;
		private System.Windows.Forms.Label lblColor;
		private BabaIsYou.Controls.NumericBox numNumber;
		private System.Windows.Forms.Label lblNumber;
		private BabaIsYou.Controls.ColorRadioButton chkNumber;
		private BabaIsYou.Controls.ColorRadioButton chkLetter;
		private BabaIsYou.Controls.ColorRadioButton chkDot;
		private BabaIsYou.Controls.ColorRadioButton chkIcon;
		private System.Windows.Forms.PictureBox imgIcon;
		private System.Windows.Forms.Label lblIcon;
		private System.Windows.Forms.GroupBox grpStyle;
		private System.Windows.Forms.Label lblStyle;
		private System.Windows.Forms.Label lblState;
		private System.Windows.Forms.GroupBox grpState;
		private BabaIsYou.Controls.ColorRadioButton chkHidden;
		private BabaIsYou.Controls.ColorRadioButton chkNormal;
		private BabaIsYou.Controls.ColorRadioButton chkOpened;
		private System.Windows.Forms.Button btnSetLevel;
		private System.Windows.Forms.Button btnSave;
	}
}