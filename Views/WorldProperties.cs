using BabaIsYou.Controls;
using BabaIsYou.Map;
using System;
using System.Drawing;
using System.Windows.Forms;
namespace BabaIsYou.Views {
	public partial class WorldProperties : Form {
		public ListPanel LevelList { get; set; }
		public Info WorldInfo { get; set; }
		public WorldProperties() {
			InitializeComponent();
			Renderer.SetFonts(this);
		}

		private void WorldProperties_Shown(object sender, EventArgs e) {
			txtName.Text = WorldInfo["general", "name"];
			txtStart.Text = WorldInfo["general", "start"];
			txtFirst.Text = WorldInfo["general", "firstlevel"];
		}
		private void btnSave_Click(object sender, EventArgs e) {
			if (string.IsNullOrEmpty(txtName.Text)) {
				txtName.Focus();
				MessageBox.Show(this, "Please enter a name for the world.", "World Name");
				return;
			} else if (string.IsNullOrEmpty(txtStart.Text) && LevelList.Count > 0) {
				txtStart.Focus();
				MessageBox.Show(this, "Please enter a starting level.", "Starting Level");
				return;
			}

			WorldInfo["general", "name"] = txtName.Text;
			WorldInfo["general", "start"] = txtStart.Text;
			WorldInfo["general", "firstlevel"] = txtFirst.Text;

			this.DialogResult = DialogResult.OK;
			this.Close();
		}
		private void btnSetLevel_Click(object sender, EventArgs e) {
			string text = null;
			if (sender == btnSetFirst) {
				text = txtFirst.Text;
			} else {
				text = txtStart.Text;
			}
			using (ObjectSelector selector = new ObjectSelector()) {
				selector.DrawText = true;

				int imgWidth = WorldViewer.LevelImageWidth;
				int imgHeight = WorldViewer.LevelImageHeight;
				int addedCount = LevelList.Count;
				for (int i = 0; i < addedCount; i++) {
					ListItem level = LevelList[i];
					Grid map = (Grid)level.Value;

					ListItem item = new ListItem(map, map.Name, imgWidth, imgHeight);
					item.BackColor = level.BackColor;
					selector.AddItem(item, map.FileName.Equals(text, StringComparison.OrdinalIgnoreCase));
				}

				selector.BackColor = this.BackColor;
				selector.Icon = this.Icon;

				if (addedCount == 0) {
					MessageBox.Show(this, "No level maps to choose from.");
					return;
				}

				Size size = Renderer.GetSizeForCount(addedCount, 5, 6);
				selector.ClientSize = new Size(imgWidth * size.Width, imgHeight * size.Height);
				selector.SortByText = LevelList.SortByText;
				selector.SortItems();
				selector.SetRenderItem(delegate (ListItem item, Graphics g) {
					Grid grid = (Grid)item.Value;
					Renderer.Render(grid, g, item.Width, item.Height);
				});

				DialogResult result = selector.ShowDialog(this);
				if (result == DialogResult.OK) {
					Grid map = (Grid)selector.SelectedItem;
					if (sender == btnSetFirst) {
						txtFirst.Text = map.FileName;
					} else {
						txtStart.Text = map.FileName;
					}
				}
			}
		}
	}
}