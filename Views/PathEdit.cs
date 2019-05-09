using BabaIsYou.Controls;
using BabaIsYou.Map;
using System;
using System.Drawing;
using System.Windows.Forms;
namespace BabaIsYou.Views {
	public partial class PathEdit : Form {
		public Palette Palette { get; set; }
		public LevelPath Edit { get; set; }
		public Grid Map { get; set; }
		private LevelPath pathCopy;
		public PathEdit() {
			InitializeComponent();
			Renderer.SetFonts(this);
		}

		private void PathEdit_Shown(object sender, EventArgs e) {
			pathCopy = (LevelPath)Edit.Copy();
			numRequirement.Value = pathCopy.Requirement;
			switch ((PathGate)pathCopy.Gate) {
				case PathGate.Level: chkLevels.Checked = true; break;
				case PathGate.Area: chkAreas.Checked = true; break;
				case PathGate.Orb: chkOrbs.Checked = true; break;
				default: chkNope.Checked = true; break;
			}
			switch ((PathStyle)pathCopy.Style) {
				case PathStyle.Visible: chkVisible.Checked = true; break;
				default: chkHidden.Checked = true; break;
			}
			UpdateObject();
		}

		private void chkStyle_CheckedChanged(object sender, EventArgs e) {
			if (pathCopy == null) { return; }

			pathCopy.Style = chkVisible.Checked ? (byte)PathStyle.Visible : pathCopy.Style;
			pathCopy.Style = chkHidden.Checked ? (byte)PathStyle.Hidden : pathCopy.Style;
		}
		private void chkGate_CheckedChanged(object sender, EventArgs e) {
			if (pathCopy == null) { return; }

			pathCopy.Gate = chkNope.Checked ? (byte)PathGate.None : pathCopy.Gate;
			pathCopy.Gate = chkLevels.Checked ? (byte)PathGate.Level : pathCopy.Gate;
			pathCopy.Gate = chkAreas.Checked ? (byte)PathGate.Area : pathCopy.Gate;
			pathCopy.Gate = chkOrbs.Checked ? (byte)PathGate.Orb : pathCopy.Gate;
		}
		private void numRequirement_ValueChanged(object sender, EventArgs e) {
			if (pathCopy == null) { return; }

			pathCopy.Requirement = (byte)numRequirement.Value;
		}
		private void imgObject_Click(object sender, EventArgs e) {
			using (ObjectSelector selector = new ObjectSelector()) {
				int spriteCount = 0;
				foreach (Item spriteItem in Reader.DefaultsByObject.Values) {
					if (spriteItem.ID <= 0 || string.IsNullOrEmpty(spriteItem.Sprite)) { continue; }

					spriteCount++;
				}
				Size size = Renderer.GetSizeForCount(spriteCount);

				int imgSize = WorldViewer.Instance.Width / (size.Width + 1);
				if (imgSize * (size.Height + 2) > WorldViewer.Instance.Height) {
					imgSize = WorldViewer.Instance.Height / (size.Height + 2);
				}
				if (imgSize > 48) { imgSize = 48; }

				Rectangle rect = new Rectangle(0, 0, imgSize, imgSize);
				foreach (Item spriteItem in Reader.DefaultsByObject.Values) {
					if (spriteItem.ID <= 0 || string.IsNullOrEmpty(spriteItem.Sprite)) { continue; }

					ItemChange change;
					Item copy = spriteItem;
					if (Map.Changes.TryGetValue(spriteItem.ID, out change)) {
						copy = copy.Copy();
						change.Apply(copy);
					}

					string name = copy.Name;
					int index = name.IndexOf("text");
					if (index == 0) {
						name = name.Substring(5) + "_text";
					}

					Bitmap img = new Bitmap(imgSize, imgSize);
					using (Graphics g = Graphics.FromImage(img)) {
						Renderer.DrawSprite(null, g, rect, copy, Palette);
					}
					ListItem item = new ListItem(copy, name, img);
					item.BackColor = Palette.Background;
					selector.AddItem(item, false);
				}

				selector.BackColor = Palette.Edge;
				selector.Icon = this.Icon;
				int sizeX = (int)Math.Sqrt(spriteCount);
				int sizeY = sizeX;
				bool isY = true;
				while (sizeX * sizeY < spriteCount) {
					if (isY) {
						sizeY++;
						isY = false;
					} else {
						sizeX++;
						isY = true;
					}
				}
				selector.ClientSize = new Size(imgSize * sizeX, imgSize * sizeY);
				selector.SortItems();

				DialogResult result = selector.ShowDialog(this);
				if (result == DialogResult.OK) {
					Item item = (Item)selector.SelectedItem;
					pathCopy.Object = item.Object;
					UpdateObject();
				}
			}
		}
		private void UpdateObject() {
			if (imgObject.Image != null) {
				imgObject.Image.Dispose();
			}

			ItemChange change;
			Item copy = Reader.DefaultsByObject[pathCopy.Object].Copy();
			if (Map.Changes.TryGetValue(copy.ID, out change)) {
				change.Apply(copy);
			}
			imgObject.Image = Renderer.DrawSprite(copy, imgObject.Width, Palette);
		}
		private void btnSave_Click(object sender, EventArgs e) {
			Edit.Style = pathCopy.Style;
			Edit.Gate = pathCopy.Gate;
			Edit.Requirement = pathCopy.Requirement;
			Edit.Object = pathCopy.Object;
			Edit.UpdatePath();
			ItemChange change;
			if (Map.Changes.TryGetValue(Edit.ID, out change)) {
				change.Apply(Edit);
			}
			this.DialogResult = DialogResult.OK;
			this.Close();
		}
	}
}