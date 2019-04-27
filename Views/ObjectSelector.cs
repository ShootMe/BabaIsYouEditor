using BabaIsYou.Controls;
using System.Windows.Forms;
namespace BabaIsYou.Views {
	public partial class ObjectSelector : Form {
		public object SelectedItem { get; set; }
		public ObjectSelector() {
			InitializeComponent();
		}
		public void AddItem(ListItem item, bool select) {
			listSelector.Items.Add(item);
			if (select) {
				listSelector.SelectedIndex = listSelector.Items.Count - 1;
			}
		}
		public void SortItems() {
			ListItem item = listSelector.SelectedItem;
			listSelector.Items.Sort();
			listSelector.SelectedItem = item;
			listSelector.EnsureSelectedInView();
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
	}
}