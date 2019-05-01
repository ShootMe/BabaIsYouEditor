using BabaIsYou.Controls;
using BabaIsYou.Map;
using System.Windows.Forms;
namespace BabaIsYou.Views {
	public partial class ObjectSelector : Form {
		public object SelectedItem { get; set; }
		public ObjectSelector() {
			InitializeComponent();
			Renderer.SetFonts(this);
		}
		public void AddItem(ListItem item, bool select) {
			listSelector.AddItem(item);
			if (select) {
				listSelector.SelectedIndex = listSelector.Count - 1;
			}
		}
		public bool SortByText {
			get { return listSelector.SortByText; }
			set { listSelector.SortByText = value; }
		}
		public void SortItems() {
			ListItem item = listSelector.SelectedItem;
			listSelector.SortItems();
			listSelector.SelectedItem = item;
		}
		public bool DrawText {
			get { return listSelector.DrawText; }
			set { listSelector.DrawText = value; }
		}
		private void listSelector_ItemClicked(ListItem item, MouseButtons buttons) {
			if (buttons == MouseButtons.Left) {
				SelectedItem = item.Value;
				this.DialogResult = DialogResult.OK;
				this.Close();
			}
		}
		private void ObjectSelector_KeyDown(object sender, KeyEventArgs e) {
			if (e.KeyCode == Keys.Enter && listSelector.SelectedItem != null) {
				SelectedItem = listSelector.SelectedItem.Value;
				this.DialogResult = DialogResult.OK;
				this.Close();
			}
		}
	}
}