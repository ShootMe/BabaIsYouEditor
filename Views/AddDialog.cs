using BabaIsYou.Map;
using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
namespace BabaIsYou.Views {
	public partial class AddDialog : Form {
		public string InputText { get; set; }
		public string InputLabel { get; set; }
		public string OptionText { get; set; }
		public bool OptionSetting { get; set; }
		public AddDialog() {
			InitializeComponent();
			Renderer.SetFonts(this);
		}

		private void AddDialog_Load(object sender, EventArgs e) {
			txtInput.Text = InputText;
			lblInput.Text = InputLabel;
			int left = txtInput.Left;
			txtInput.Left = lblInput.Right + 6;
			txtInput.Width = txtInput.Width - txtInput.Left + left;

			if (!string.IsNullOrEmpty(OptionText)) {
				chkOption.Text = OptionText;
				chkOption.Checked = OptionSetting;
				chkOption.Left = txtInput.Left;
				chkOption.Visible = true;
				ClientSize = new Size(ClientSize.Width, 108);
			}
		}
		private void AddDialog_Shown(object sender, EventArgs e) {
			if (!string.IsNullOrEmpty(InputText) && InputText.Length > 0) {
				txtInput.Select(0, InputText.Length);
			}
		}
		private void btnSave_Click(object sender, EventArgs e) {
			this.DialogResult = DialogResult.OK;
			InputText = txtInput.Text;
			OptionSetting = chkOption.Checked;
			this.Close();
		}
		private void txtInput_Validated(object sender, EventArgs e) {
			ValidateText();
		}
		private void txtInput_TextChanged(object sender, EventArgs e) {
			ValidateText();
		}
		private void ValidateText() {
			string text = txtInput.Text;
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < text.Length; i++) {
				char c = text[i];
				if (char.IsLetterOrDigit(c) || c == ' ') {
					sb.Append(c);
				}
			}
			string newText = sb.ToString();
			if (newText != text) {
				txtInput.Text = newText;
			}
		}
	}
}