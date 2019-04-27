using System;
using System.Text;
using System.Windows.Forms;
namespace BabaIsYou.Views {
	public partial class AddDialog : Form {
		public string InputText { get; set; }
		public string InputLabel { get; set; }
		public AddDialog() {
			InitializeComponent();
		}

		private void btnSave_Click(object sender, EventArgs e) {
			this.DialogResult = DialogResult.OK;
			InputText = txtInput.Text;
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

		private void AddDialog_Shown(object sender, EventArgs e) {
			txtInput.Text = InputText;
			lblInput.Text = InputLabel;
			int left = txtInput.Left;
			txtInput.Left = lblInput.Right + 6;
			txtInput.Width = txtInput.Width - txtInput.Left + left;
		}
	}
}