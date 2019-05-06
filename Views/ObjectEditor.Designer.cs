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
			this.components = new System.ComponentModel.Container();
			this.imgOriginal = new System.Windows.Forms.PictureBox();
			this.lblSelected = new System.Windows.Forms.Label();
			this.imgObject = new System.Windows.Forms.PictureBox();
			this.lblColor = new System.Windows.Forms.Label();
			this.grpProperties = new System.Windows.Forms.GroupBox();
			this.lblObjectText = new System.Windows.Forms.Label();
			this.txtObjectText = new System.Windows.Forms.TextBox();
			this.lblArgType = new System.Windows.Forms.Label();
			this.txtArgType = new System.Windows.Forms.TextBox();
			this.lblArgExtra = new System.Windows.Forms.Label();
			this.txtArgExtra = new System.Windows.Forms.TextBox();
			this.lblOperatorType = new System.Windows.Forms.Label();
			this.txtOperatorType = new System.Windows.Forms.TextBox();
			this.lblImage = new System.Windows.Forms.Label();
			this.imgImage = new System.Windows.Forms.PictureBox();
			this.cboTiling = new System.Windows.Forms.ComboBox();
			this.cboTextType = new System.Windows.Forms.ComboBox();
			this.lblTiling = new System.Windows.Forms.Label();
			this.lblTextType = new System.Windows.Forms.Label();
			this.imgActive = new System.Windows.Forms.PictureBox();
			this.imgNormal = new System.Windows.Forms.PictureBox();
			this.numLayer = new BabaIsYou.Controls.NumericBox();
			this.lblLayer = new System.Windows.Forms.Label();
			this.lblActiveColor = new System.Windows.Forms.Label();
			this.btnReset = new System.Windows.Forms.Button();
			this.btnSave = new System.Windows.Forms.Button();
			this.toolTips = new System.Windows.Forms.ToolTip(this.components);
			((System.ComponentModel.ISupportInitialize)(this.imgOriginal)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.imgObject)).BeginInit();
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
			this.lblSelected.Location = new System.Drawing.Point(24, 22);
			this.lblSelected.Name = "lblSelected";
			this.lblSelected.Size = new System.Drawing.Size(43, 13);
			this.lblSelected.TabIndex = 0;
			this.lblSelected.Text = "Object";
			// 
			// imgObject
			// 
			this.imgObject.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.imgObject.Cursor = System.Windows.Forms.Cursors.Hand;
			this.imgObject.Location = new System.Drawing.Point(21, 42);
			this.imgObject.Name = "imgObject";
			this.imgObject.Size = new System.Drawing.Size(48, 48);
			this.imgObject.TabIndex = 2;
			this.imgObject.TabStop = false;
			this.imgObject.Click += new System.EventHandler(this.imgCurrent_Click);
			// 
			// lblColor
			// 
			this.lblColor.AutoSize = true;
			this.lblColor.Location = new System.Drawing.Point(172, 22);
			this.lblColor.Name = "lblColor";
			this.lblColor.Size = new System.Drawing.Size(37, 13);
			this.lblColor.TabIndex = 2;
			this.lblColor.Text = "Color";
			// 
			// grpProperties
			// 
			this.grpProperties.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.grpProperties.Controls.Add(this.lblObjectText);
			this.grpProperties.Controls.Add(this.txtObjectText);
			this.grpProperties.Controls.Add(this.lblArgType);
			this.grpProperties.Controls.Add(this.txtArgType);
			this.grpProperties.Controls.Add(this.lblArgExtra);
			this.grpProperties.Controls.Add(this.txtArgExtra);
			this.grpProperties.Controls.Add(this.lblOperatorType);
			this.grpProperties.Controls.Add(this.txtOperatorType);
			this.grpProperties.Controls.Add(this.lblImage);
			this.grpProperties.Controls.Add(this.imgImage);
			this.grpProperties.Controls.Add(this.cboTiling);
			this.grpProperties.Controls.Add(this.cboTextType);
			this.grpProperties.Controls.Add(this.lblTiling);
			this.grpProperties.Controls.Add(this.lblTextType);
			this.grpProperties.Controls.Add(this.imgActive);
			this.grpProperties.Controls.Add(this.imgNormal);
			this.grpProperties.Controls.Add(this.numLayer);
			this.grpProperties.Controls.Add(this.lblLayer);
			this.grpProperties.Controls.Add(this.lblActiveColor);
			this.grpProperties.Controls.Add(this.lblSelected);
			this.grpProperties.Controls.Add(this.lblColor);
			this.grpProperties.Controls.Add(this.imgObject);
			this.grpProperties.Location = new System.Drawing.Point(12, 66);
			this.grpProperties.Name = "grpProperties";
			this.grpProperties.Size = new System.Drawing.Size(306, 266);
			this.grpProperties.TabIndex = 0;
			this.grpProperties.TabStop = false;
			// 
			// lblObjectText
			// 
			this.lblObjectText.AutoSize = true;
			this.lblObjectText.Location = new System.Drawing.Point(26, 100);
			this.lblObjectText.Name = "lblObjectText";
			this.lblObjectText.Size = new System.Drawing.Size(43, 13);
			this.lblObjectText.TabIndex = 4;
			this.lblObjectText.Text = "Object";
			// 
			// txtObjectText
			// 
			this.txtObjectText.CharacterCasing = System.Windows.Forms.CharacterCasing.Lower;
			this.txtObjectText.Location = new System.Drawing.Point(21, 116);
			this.txtObjectText.Name = "txtObjectText";
			this.txtObjectText.Size = new System.Drawing.Size(100, 20);
			this.txtObjectText.TabIndex = 5;
			this.txtObjectText.TextChanged += new System.EventHandler(this.txtObjectText_TextChanged);
			this.txtObjectText.Validated += new System.EventHandler(this.txtObjectText_Validated);
			// 
			// lblArgType
			// 
			this.lblArgType.AutoSize = true;
			this.lblArgType.Location = new System.Drawing.Point(131, 180);
			this.lblArgType.Name = "lblArgType";
			this.lblArgType.Size = new System.Drawing.Size(55, 13);
			this.lblArgType.TabIndex = 14;
			this.lblArgType.Text = "Arg Type";
			// 
			// txtArgType
			// 
			this.txtArgType.CharacterCasing = System.Windows.Forms.CharacterCasing.Lower;
			this.txtArgType.Location = new System.Drawing.Point(129, 196);
			this.txtArgType.Name = "txtArgType";
			this.txtArgType.Size = new System.Drawing.Size(156, 20);
			this.txtArgType.TabIndex = 15;
			this.txtArgType.TextChanged += new System.EventHandler(this.txtArgType_TextChanged);
			// 
			// lblArgExtra
			// 
			this.lblArgExtra.AutoSize = true;
			this.lblArgExtra.Location = new System.Drawing.Point(23, 220);
			this.lblArgExtra.Name = "lblArgExtra";
			this.lblArgExtra.Size = new System.Drawing.Size(61, 13);
			this.lblArgExtra.TabIndex = 16;
			this.lblArgExtra.Text = "Arg Extra";
			// 
			// txtArgExtra
			// 
			this.txtArgExtra.CharacterCasing = System.Windows.Forms.CharacterCasing.Lower;
			this.txtArgExtra.Location = new System.Drawing.Point(21, 236);
			this.txtArgExtra.Name = "txtArgExtra";
			this.txtArgExtra.Size = new System.Drawing.Size(264, 20);
			this.txtArgExtra.TabIndex = 17;
			this.txtArgExtra.TextChanged += new System.EventHandler(this.txtArgExtra_TextChanged);
			// 
			// lblOperatorType
			// 
			this.lblOperatorType.AutoSize = true;
			this.lblOperatorType.Location = new System.Drawing.Point(26, 180);
			this.lblOperatorType.Name = "lblOperatorType";
			this.lblOperatorType.Size = new System.Drawing.Size(85, 13);
			this.lblOperatorType.TabIndex = 12;
			this.lblOperatorType.Text = "Operator Type";
			// 
			// txtOperatorType
			// 
			this.txtOperatorType.CharacterCasing = System.Windows.Forms.CharacterCasing.Lower;
			this.txtOperatorType.Location = new System.Drawing.Point(21, 196);
			this.txtOperatorType.Name = "txtOperatorType";
			this.txtOperatorType.Size = new System.Drawing.Size(100, 20);
			this.txtOperatorType.TabIndex = 13;
			this.txtOperatorType.TextChanged += new System.EventHandler(this.txtOperatorType_TextChanged);
			// 
			// lblImage
			// 
			this.lblImage.AutoSize = true;
			this.lblImage.Location = new System.Drawing.Point(99, 22);
			this.lblImage.Name = "lblImage";
			this.lblImage.Size = new System.Drawing.Size(37, 13);
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
			this.cboTiling.BackColor = System.Drawing.Color.White;
			this.cboTiling.ForeColor = System.Drawing.Color.Black;
			this.cboTiling.FormattingEnabled = true;
			this.cboTiling.Location = new System.Drawing.Point(21, 155);
			this.cboTiling.Name = "cboTiling";
			this.cboTiling.Size = new System.Drawing.Size(100, 21);
			this.cboTiling.TabIndex = 9;
			this.cboTiling.SelectedIndexChanged += new System.EventHandler(this.cboTiling_SelectedIndexChanged);
			this.cboTiling.Validated += new System.EventHandler(this.cboTiling_Validated);
			// 
			// cboTextType
			// 
			this.cboTextType.BackColor = System.Drawing.Color.White;
			this.cboTextType.ForeColor = System.Drawing.Color.Black;
			this.cboTextType.FormattingEnabled = true;
			this.cboTextType.Location = new System.Drawing.Point(129, 155);
			this.cboTextType.Name = "cboTextType";
			this.cboTextType.Size = new System.Drawing.Size(100, 21);
			this.cboTextType.TabIndex = 11;
			this.cboTextType.SelectedIndexChanged += new System.EventHandler(this.cboTextType_SelectedIndexChanged);
			this.cboTextType.Validated += new System.EventHandler(this.cboTextType_Validated);
			// 
			// lblTiling
			// 
			this.lblTiling.AutoSize = true;
			this.lblTiling.Location = new System.Drawing.Point(26, 139);
			this.lblTiling.Name = "lblTiling";
			this.lblTiling.Size = new System.Drawing.Size(43, 13);
			this.lblTiling.TabIndex = 8;
			this.lblTiling.Text = "Tiling";
			// 
			// lblTextType
			// 
			this.lblTextType.AutoSize = true;
			this.lblTextType.Location = new System.Drawing.Point(131, 139);
			this.lblTextType.Name = "lblTextType";
			this.lblTextType.Size = new System.Drawing.Size(61, 13);
			this.lblTextType.TabIndex = 10;
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
			// numLayer
			// 
			this.numLayer.BackColor = System.Drawing.Color.White;
			this.numLayer.ForeColor = System.Drawing.Color.Black;
			this.numLayer.Location = new System.Drawing.Point(129, 116);
			this.numLayer.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
			this.numLayer.Name = "numLayer";
			this.numLayer.Size = new System.Drawing.Size(55, 20);
			this.numLayer.TabIndex = 7;
			this.toolTips.SetToolTip(this.numLayer, "Higher values will show overtop lower values");
			this.numLayer.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
			this.numLayer.ValueChanged += new System.EventHandler(this.numLayer_ValueChanged);
			// 
			// lblLayer
			// 
			this.lblLayer.AutoSize = true;
			this.lblLayer.Location = new System.Drawing.Point(131, 96);
			this.lblLayer.Name = "lblLayer";
			this.lblLayer.Size = new System.Drawing.Size(37, 13);
			this.lblLayer.TabIndex = 6;
			this.lblLayer.Text = "Layer";
			// 
			// lblActiveColor
			// 
			this.lblActiveColor.AutoSize = true;
			this.lblActiveColor.Location = new System.Drawing.Point(222, 22);
			this.lblActiveColor.Name = "lblActiveColor";
			this.lblActiveColor.Size = new System.Drawing.Size(79, 13);
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
			// toolTips
			// 
			this.toolTips.AutoPopDelay = 5000;
			this.toolTips.InitialDelay = 100;
			this.toolTips.ReshowDelay = 100;
			// 
			// ObjectEditor
			// 
			this.AcceptButton = this.btnSave;
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.BackColor = System.Drawing.Color.Black;
			this.ClientSize = new System.Drawing.Size(330, 344);
			this.Controls.Add(this.btnSave);
			this.Controls.Add(this.btnReset);
			this.Controls.Add(this.grpProperties);
			this.Controls.Add(this.imgOriginal);
			this.DoubleBuffered = true;
			this.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ForeColor = System.Drawing.Color.White;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ObjectEditor";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Edit - object051 - fungus";
			this.Load += new System.EventHandler(this.ObjectEditor_Load);
			((System.ComponentModel.ISupportInitialize)(this.imgOriginal)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.imgObject)).EndInit();
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
		private System.Windows.Forms.PictureBox imgObject;
		private System.Windows.Forms.Label lblColor;
		private System.Windows.Forms.GroupBox grpProperties;
		private System.Windows.Forms.Label lblActiveColor;
		private BabaIsYou.Controls.NumericBox numLayer;
		private System.Windows.Forms.Label lblLayer;
		private System.Windows.Forms.Button btnReset;
		private System.Windows.Forms.PictureBox imgActive;
		private System.Windows.Forms.PictureBox imgNormal;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.ComboBox cboTextType;
		private System.Windows.Forms.Label lblTiling;
		private System.Windows.Forms.Label lblTextType;
		private System.Windows.Forms.ComboBox cboTiling;
		private System.Windows.Forms.Label lblImage;
		private System.Windows.Forms.PictureBox imgImage;
		private System.Windows.Forms.ToolTip toolTips;
		private System.Windows.Forms.Label lblArgExtra;
		private System.Windows.Forms.TextBox txtArgExtra;
		private System.Windows.Forms.Label lblOperatorType;
		private System.Windows.Forms.TextBox txtOperatorType;
		private System.Windows.Forms.Label lblArgType;
		private System.Windows.Forms.TextBox txtArgType;
		private System.Windows.Forms.Label lblObjectText;
		private System.Windows.Forms.TextBox txtObjectText;
	}
}