using BabaIsYou.Controls;
using BabaIsYou.Map;
using System;
using System.Drawing;
using System.Windows.Forms;
namespace BabaIsYou.Views {
	public partial class PathEdit : Form {
		public Palette Palette { get; set; }
		public Line Line { get; set; }
		public Grid Map { get; set; }
		private Line lineCopy;
		public PathEdit() {
			InitializeComponent();
		}

		private void PathEdit_Shown(object sender, EventArgs e) {
			lineCopy = (Line)Line.Copy();
			numRequirement.Value = lineCopy.Requirement;
			switch ((LineGate)lineCopy.Gate) {
				case LineGate.LevelClears: chkLevelClears.Checked = true; break;
				case LineGate.MapClears: chkMapClears.Checked = true; break;
				default: chkNope.Checked = true; break;
			}
			switch ((LineStyle)lineCopy.Style) {
				case LineStyle.Visible: chkVisible.Checked = true; break;
				default: chkHidden.Checked = true; break;
			}
			UpdateObject();
		}

		private void chkStyle_CheckedChanged(object sender, EventArgs e) {
			if (lineCopy == null) { return; }

			lineCopy.Style = chkVisible.Checked ? (byte)LineStyle.Visible : lineCopy.Style;
			lineCopy.Style = chkHidden.Checked ? (byte)LineStyle.Hidden : lineCopy.Style;
		}
		private void chkGate_CheckedChanged(object sender, EventArgs e) {
			if (lineCopy == null) { return; }

			lineCopy.Gate = chkNope.Checked ? (byte)LineGate.None : lineCopy.Gate;
			lineCopy.Gate = chkLevelClears.Checked ? (byte)LineGate.LevelClears : lineCopy.Gate;
			lineCopy.Gate = chkMapClears.Checked ? (byte)LineGate.MapClears : lineCopy.Gate;
		}
		private void numRequirement_ValueChanged(object sender, EventArgs e) {
			if (lineCopy == null) { return; }

			lineCopy.Requirement = (byte)numRequirement.Value;
		}
		private void imgObject_Click(object sender, EventArgs e) {
			using (ObjectSelector selector = new ObjectSelector()) {
				int imgSize = 36;
				Rectangle rect = new Rectangle(0, 0, imgSize, imgSize);
				int spriteCount = Reader.DefaultsByName.Count;
				foreach (Item spriteItem in Reader.DefaultsByName.Values) {
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

				selector.SortItems();
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
				DialogResult result = selector.ShowDialog(this);
				if (result == DialogResult.OK) {
					Item item = (Item)selector.SelectedItem;
					lineCopy.Object = item.Object;
					UpdateObject();
				}
			}
		}
		private void UpdateObject() {
			if (imgObject.Image != null) {
				imgObject.Image.Dispose();
			}

			ItemChange change;
			Item copy = Reader.DefaultsByName[lineCopy.Object].Copy();
			if (Map.Changes.TryGetValue(copy.ID, out change)) {
				change.Apply(copy);
			}
			imgObject.Image = Renderer.DrawSprite(copy, imgObject.Width, Palette);
		}
		private void btnSave_Click(object sender, EventArgs e) {
			Line.Style = lineCopy.Style;
			Line.Gate = lineCopy.Gate;
			Line.Requirement = lineCopy.Requirement;
			Line.Object = lineCopy.Object;
			Line.UpdateLine();
			ItemChange change;
			if (Map.Changes.TryGetValue(Line.ID, out change)) {
				change.Apply(Line);
			}
			this.DialogResult = DialogResult.OK;
			this.Close();
		}
	}
}