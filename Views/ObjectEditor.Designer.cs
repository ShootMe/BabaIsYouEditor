namespace BabaIsYou.Views {
	partial class ObjectEditor {
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
			this.imgOriginal = new System.Windows.Forms.PictureBox();
			this.lblSelected = new System.Windows.Forms.Label();
			this.imgCurrent = new System.Windows.Forms.PictureBox();
			this.lblColor = new System.Windows.Forms.Label();
			this.grpProperties = new System.Windows.Forms.GroupBox();
			this.lblImage = new System.Windows.Forms.Label();
			this.imgImage = new System.Windows.Forms.PictureBox();
			this.cboTiling = new System.Windows.Forms.ComboBox();
			this.cboTextType = new System.Windows.Forms.ComboBox();
			this.lblTiling = new System.Windows.Forms.Label();
			this.lblTextType = new System.Windows.Forms.Label();
			this.imgActive = new System.Windows.Forms.PictureBox();
			this.imgNormal = new System.Windows.Forms.PictureBox();
			this.lblLayerNote = new System.Windows.Forms.Label();
			this.numLayer = new System.Windows.Forms.NumericUpDown();
			this.lblLayer = new System.Windows.Forms.Label();
			this.lblActiveColor = new System.Windows.Forms.Label();
			this.btnReset = new System.Windows.Forms.Button();
			this.btnSave = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.imgOriginal)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.imgCurrent)).BeginInit();
			this.grpProperties.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.imgImage)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.imgActive)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.imgNormal)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numLayer)).BeginInit();
			this.SuspendLayout();
			// 
			// imgOriginal
			// 
			this.imgOriginal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.imgOriginal.Location = new System.Drawing.Point(33, 12);
			this.imgOriginal.Name = "imgOriginal";
			this.imgOriginal.Size = new System.Drawing.Size(48, 48);
			this.imgOriginal.TabIndex = 0;
			this.imgOriginal.TabStop = false;
			// 
			// lblSelected
			// 
			this.lblSelected.AutoSize = true;
			this.lblSelected.Location = new System.Drawing.Point(26, 22);
			this.lblSelected.Name = "lblSelected";
			this.lblSelected.Size = new System.Drawing.Size(38, 13);
			this.lblSelected.TabIndex = 0;
			this.lblSelected.Text = "Object";
			// 
			// imgCurrent
			// 
			this.imgCurrent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.imgCurrent.Cursor = System.Windows.Forms.Cursors.Hand;
			this.imgCurrent.Location = new System.Drawing.Point(21, 42);
			this.imgCurrent.Name = "imgCurrent";
			this.imgCurrent.Size = new System.Drawing.Size(48, 48);
			this.imgCurrent.TabIndex = 2;
			this.imgCurrent.TabStop = false;
			this.imgCurrent.Click += new System.EventHandler(this.imgCurrent_Click);
			// 
			// lblColor
			// 
			this.lblColor.AutoSize = true;
			this.lblColor.Location = new System.Drawing.Point(175, 22);
			this.lblColor.Name = "lblColor";
			this.lblColor.Size = new System.Drawing.Size(31, 13);
			this.lblColor.TabIndex = 2;
			this.lblColor.Text = "Color";
			// 
			// grpProperties
			// 
			this.grpProperties.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.grpProperties.Controls.Add(this.lblImage);
			this.grpProperties.Controls.Add(this.imgImage);
			this.grpProperties.Controls.Add(this.cboTiling);
			this.grpProperties.Controls.Add(this.cboTextType);
			this.grpProperties.Controls.Add(this.lblTiling);
			this.grpProperties.Controls.Add(this.lblTextType);
			this.grpProperties.Controls.Add(this.imgActive);
			this.grpProperties.Controls.Add(this.imgNormal);
			this.grpProperties.Controls.Add(this.lblLayerNote);
			this.grpProperties.Controls.Add(this.numLayer);
			this.grpProperties.Controls.Add(this.lblLayer);
			this.grpProperties.Controls.Add(this.lblActiveColor);
			this.grpProperties.Controls.Add(this.lblSelected);
			this.grpProperties.Controls.Add(this.lblColor);
			this.grpProperties.Controls.Add(this.imgCurrent);
			this.grpProperties.Location = new System.Drawing.Point(12, 66);
			this.grpProperties.Name = "grpProperties";
			this.grpProperties.Size = new System.Drawing.Size(306, 195);
			this.grpProperties.TabIndex = 0;
			this.grpProperties.TabStop = false;
			// 
			// lblImage
			// 
			this.lblImage.AutoSize = true;
			this.lblImage.Location = new System.Drawing.Point(99, 22);
			this.lblImage.Name = "lblImage";
			this.lblImage.Size = new System.Drawing.Size(36, 13);
			this.lblImage.TabIndex = 1;
			this.lblImage.Text = "Image";
			// 
			// imgImage
			// 
			this.imgImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.imgImage.Cursor = System.Windows.Forms.Cursors.Hand;
			this.imgImage.Location = new System.Drawing.Point(93, 42);
			this.imgImage.Name = "imgImage";
			this.imgImage.Size = new System.Drawing.Size(48, 48);
			this.imgImage.TabIndex = 11;
			this.imgImage.TabStop = false;
			this.imgImage.Click += new System.EventHandler(this.imgCurrent_Click);
			// 
			// cboTiling
			// 
			this.cboTiling.FormattingEnabled = true;
			this.cboTiling.Location = new System.Drawing.Point(21, 163);
			this.cboTiling.Name = "cboTiling";
			this.cboTiling.Size = new System.Drawing.Size(87, 21);
			this.cboTiling.TabIndex = 8;
			this.cboTiling.SelectedIndexChanged += new System.EventHandler(this.cboTiling_SelectedIndexChanged);
			this.cboTiling.Validated += new System.EventHandler(this.cboTiling_Validated);
			// 
			// cboTextType
			// 
			this.cboTextType.FormattingEnabled = true;
			this.cboTextType.Location = new System.Drawing.Point(139, 163);
			this.cboTextType.Name = "cboTextType";
			this.cboTextType.Size = new System.Drawing.Size(87, 21);
			this.cboTextType.TabIndex = 10;
			this.cboTextType.SelectedIndexChanged += new System.EventHandler(this.cboTextType_SelectedIndexChanged);
			this.cboTextType.Validated += new System.EventHandler(this.cboTextType_Validated);
			// 
			// lblTiling
			// 
			this.lblTiling.AutoSize = true;
			this.lblTiling.Location = new System.Drawing.Point(48, 147);
			this.lblTiling.Name = "lblTiling";
			this.lblTiling.Size = new System.Drawing.Size(32, 13);
			this.lblTiling.TabIndex = 7;
			this.lblTiling.Text = "Tiling";
			// 
			// lblTextType
			// 
			this.lblTextType.AutoSize = true;
			this.lblTextType.Location = new System.Drawing.Point(155, 147);
			this.lblTextType.Name = "lblTextType";
			this.lblTextType.Size = new System.Drawing.Size(55, 13);
			this.lblTextType.TabIndex = 9;
			this.lblTextType.Text = "Text Type";
			// 
			// imgActive
			// 
			this.imgActive.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.imgActive.Cursor = System.Windows.Forms.Cursors.Hand;
			this.imgActive.Location = new System.Drawing.Point(237, 42);
			this.imgActive.Name = "imgActive";
			this.imgActive.Size = new System.Drawing.Size(48, 48);
			this.imgActive.TabIndex = 9;
			this.imgActive.TabStop = false;
			this.imgActive.Click += new System.EventHandler(this.imgColor_Click);
			// 
			// imgNormal
			// 
			this.imgNormal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.imgNormal.Cursor = System.Windows.Forms.Cursors.Hand;
			this.imgNormal.Location = new System.Drawing.Point(165, 42);
			this.imgNormal.Name = "imgNormal";
			this.imgNormal.Size = new System.Drawing.Size(48, 48);
			this.imgNormal.TabIndex = 8;
			this.imgNormal.TabStop = false;
			this.imgNormal.Click += new System.EventHandler(this.imgColor_Click);
			// 
			// lblLayerNote
			// 
			this.lblLayerNote.AutoSize = true;
			this.lblLayerNote.Location = new System.Drawing.Point(74, 118);
			this.lblLayerNote.Name = "lblLayerNote";
			this.lblLayerNote.Size = new System.Drawing.Size(218, 13);
			this.lblLayerNote.TabIndex = 6;
			this.lblLayerNote.Text = "Higher values will show overtop lower values";
			// 
			// numLayer
			// 
			this.numLayer.Location = new System.Drawing.Point(21, 116);
			this.numLayer.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
			this.numLayer.Name = "numLayer";
			this.numLayer.Size = new System.Drawing.Size(48, 20);
			this.numLayer.TabIndex = 5;
			this.numLayer.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
			this.numLayer.ValueChanged += new System.EventHandler(this.numLayer_ValueChanged);
			this.numLayer.Enter += new System.EventHandler(this.numLayer_Enter);
			// 
			// lblLayer
			// 
			this.lblLayer.AutoSize = true;
			this.lblLayer.Location = new System.Drawing.Point(26, 96);
			this.lblLayer.Name = "lblLayer";
			this.lblLayer.Size = new System.Drawing.Size(33, 13);
			this.lblLayer.TabIndex = 4;
			this.lblLayer.Text = "Layer";
			// 
			// lblActiveColor
			// 
			this.lblActiveColor.AutoSize = true;
			this.lblActiveColor.Location = new System.Drawing.Point(229, 22);
			this.lblActiveColor.Name = "lblActiveColor";
			this.lblActiveColor.Size = new System.Drawing.Size(64, 13);
			this.lblActiveColor.TabIndex = 3;
			this.lblActiveColor.Text = "Active Color";
			// 
			// btnReset
			// 
			this.btnReset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnReset.Location = new System.Drawing.Point(106, 37);
			this.btnReset.Name = "btnReset";
			this.btnReset.Size = new System.Drawing.Size(51, 23);
			this.btnReset.TabIndex = 1;
			this.btnReset.Text = "Reset";
			this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
			// 
			// btnSave
			// 
			this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnSave.Location = new System.Drawing.Point(163, 37);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(51, 23);
			this.btnSave.TabIndex = 2;
			this.btnSave.Text = "Save";
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// ObjectEditor
			// 
			this.AcceptButton = this.btnSave;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Black;
			this.ClientSize = new System.Drawing.Size(330, 273);
			this.Controls.Add(this.btnSave);
			this.Controls.Add(this.btnReset);
			this.Controls.Add(this.grpProperties);
			this.Controls.Add(this.imgOriginal);
			this.DoubleBuffered = true;
			this.ForeColor = System.Drawing.Color.White;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ObjectEditor";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Edit - object051 - fungus";
			this.Shown += new System.EventHandler(this.ObjectEditor_Shown);
			((System.ComponentModel.ISupportInitialize)(this.imgOriginal)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.imgCurrent)).EndInit();
			this.grpProperties.ResumeLayout(false);
			this.grpProperties.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.imgImage)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.imgActive)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.imgNormal)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numLayer)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.PictureBox imgOriginal;
		private System.Windows.Forms.Label lblSelected;
		private System.Windows.Forms.PictureBox imgCurrent;
		private System.Windows.Forms.Label lblColor;
		private System.Windows.Forms.GroupBox grpProperties;
		private System.Windows.Forms.Label lblActiveColor;
		private System.Windows.Forms.NumericUpDown numLayer;
		private System.Windows.Forms.Label lblLayer;
		private System.Windows.Forms.Button btnReset;
		private System.Windows.Forms.Label lblLayerNote;
		private System.Windows.Forms.PictureBox imgActive;
		private System.Windows.Forms.PictureBox imgNormal;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.ComboBox cboTextType;
		private System.Windows.Forms.Label lblTiling;
		private System.Windows.Forms.Label lblTextType;
		private System.Windows.Forms.ComboBox cboTiling;
		private System.Windows.Forms.Label lblImage;
		private System.Windows.Forms.PictureBox imgImage;
	}
}