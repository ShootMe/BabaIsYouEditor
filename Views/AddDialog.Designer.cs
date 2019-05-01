namespace BabaIsYou.Views {
	partial class AddDialog {
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
			this.lblInput = new System.Windows.Forms.Label();
			this.txtInput = new System.Windows.Forms.TextBox();
			this.btnSave = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// lblInput
			// 
			this.lblInput.AutoSize = true;
			this.lblInput.Location = new System.Drawing.Point(16, 22);
			this.lblInput.Name = "lblInput";
			this.lblInput.Size = new System.Drawing.Size(31, 13);
			this.lblInput.TabIndex = 0;
			this.lblInput.Text = "Name";
			// 
			// txtInput
			// 
			this.txtInput.BackColor = System.Drawing.Color.White;
			this.txtInput.CharacterCasing = System.Windows.Forms.CharacterCasing.Lower;
			this.txtInput.ForeColor = System.Drawing.Color.Black;
			this.txtInput.Location = new System.Drawing.Point(53, 19);
			this.txtInput.MaxLength = 100;
			this.txtInput.Name = "txtInput";
			this.txtInput.Size = new System.Drawing.Size(247, 20);
			this.txtInput.TabIndex = 1;
			this.txtInput.TextChanged += new System.EventHandler(this.txtInput_TextChanged);
			this.txtInput.Validated += new System.EventHandler(this.txtInput_Validated);
			// 
			// btnSave
			// 
			this.btnSave.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnSave.Location = new System.Drawing.Point(126, 48);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(51, 23);
			this.btnSave.TabIndex = 2;
			this.btnSave.Text = "Save";
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// AddDialog
			// 
			this.AcceptButton = this.btnSave;
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.BackColor = System.Drawing.Color.Black;
			this.ClientSize = new System.Drawing.Size(312, 84);
			this.Controls.Add(this.btnSave);
			this.Controls.Add(this.txtInput);
			this.Controls.Add(this.lblInput);
			this.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ForeColor = System.Drawing.Color.White;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "AddDialog";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Add";
			this.Shown += new System.EventHandler(this.AddDialog_Shown);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblInput;
		private System.Windows.Forms.TextBox txtInput;
		private System.Windows.Forms.Button btnSave;
	}
}