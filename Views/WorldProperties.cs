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

				int imgWidth = 0;
				int imgHeight = 0;
				int addedCount = LevelList.Count;
				for (int i = 0; i < addedCount; i++) {
					ListItem level = LevelList[i];
					Grid map = (Grid)level.Value;
					imgWidth = level.Extra.Width;
					imgHeight = level.Extra.Height;
					ListItem item = new ListItem(map, map.Name, level.Extra);
					item.BackColor = level.BackColor;
					selector.AddItem(item, map.FileName.Equals(text, StringComparison.OrdinalIgnoreCase));
				}

				selector.BackColor = this.BackColor;
				selector.Icon = this.Icon;

				if (addedCount == 0) {
					MessageBox.Show(this, "No level maps to choose from.");
					return;
				}

				int sizeX = (int)Math.Sqrt(addedCount);
				int sizeY = sizeX;
				bool isY = true;
				while (sizeX * sizeY < addedCount) {
					if (isY) {
						sizeY++;
						isY = false;
					} else {
						sizeX++;
						isY = true;
					}
				}
				while (imgWidth * sizeX > 1200) {
					sizeX--;
				}
				while (imgHeight * sizeY > 800) {
					sizeY--;
				}
				selector.ClientSize = new Size(imgWidth * sizeX, imgHeight * sizeY);
				selector.SortByText = LevelList.SortByText;
				selector.SortItems();

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