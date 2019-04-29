using BabaIsYou.Controls;
using BabaIsYou.Map;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
namespace BabaIsYou.Views {
	public partial class LevelEdit : Form {
		public ListPanel LevelList { get; set; }
		public Palette Palette { get; set; }
		public Level Level { get; set; }
		public LevelEdit() {
			InitializeComponent();
		}

		private void LevelEdit_Shown(object sender, EventArgs e) {
			txtName.Text = Level.Name;
			txtFile.Text = Level.File;
			numNumber.Value = Level.Number;
			switch ((LevelStyle)Level.Style) {
				case LevelStyle.Number: chkNumber.Checked = true; break;
				case LevelStyle.Dot: chkDot.Checked = true; break;
				case LevelStyle.Letter: chkLetter.Checked = true; break;
				case LevelStyle.Icon:
					UpdateIcon();
					chkIcon.Checked = true;
					break;
			}
			switch ((LevelState)Level.State) {
				case LevelState.Hidden: chkHidden.Checked = true; break;
				case LevelState.Normal: chkNormal.Checked = true; break;
				case LevelState.Opened: chkOpened.Checked = true; break;
			}
			imgColor.BackColor = Palette.Colors[Level.Color];
			imgClearColor.BackColor = Palette.Colors[Level.ActiveColor];

		}
		private void btnSave_Click(object sender, EventArgs e) {
			this.DialogResult = DialogResult.OK;
			this.Close();
		}
		private void btnSetLevel_Click(object sender, EventArgs e) {
			string text = txtFile.Text;
			using (ObjectSelector selector = new ObjectSelector()) {
				selector.DrawText = true;

				int imgWidth = 0;
				int imgHeight = 0;
				int addedCount = LevelList.Items.Count;
				for (int i = 0; i < addedCount; i++) {
					ListItem level = LevelList.Items[i];
					Grid map = (Grid)level.Value;
					imgWidth = level.Extra.Width;
					imgHeight = level.Extra.Height;
					ListItem item = new ListItem(map, map.Name, level.Extra);
					item.BackColor = level.BackColor;
					selector.AddItem(item, map.FileName.Equals(text, StringComparison.OrdinalIgnoreCase));
				}

				selector.BackColor = this.BackColor;
				selector.SortItems();
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

				DialogResult result = selector.ShowDialog(this);
				if (result == DialogResult.OK) {
					Grid map = (Grid)selector.SelectedItem;
					Level.Name = map.Name;
					Level.File = map.FileName;
					txtFile.Text = map.FileName;
					txtName.Text = map.Name;
				}
			}
		}
		private void chkStyle_CheckedChanged(object sender, EventArgs e) {
			imgIcon.Cursor = sender == chkIcon && chkIcon.Checked ? Cursors.Hand : Cursors.Default;
			Level.Style = chkDot.Checked ? (byte)LevelStyle.Dot : Level.Style;
			Level.Style = chkIcon.Checked ? (byte)LevelStyle.Icon : Level.Style;
			Level.Style = chkLetter.Checked ? (byte)LevelStyle.Letter : Level.Style;
			Level.Style = chkNumber.Checked ? (byte)LevelStyle.Number : Level.Style;
			UpdateIcon();
		}
		private void chkState_CheckedChanged(object sender, EventArgs e) {
			Level.State = chkNormal.Checked ? (byte)LevelState.Normal : Level.Style;
			Level.State = chkHidden.Checked ? (byte)LevelState.Hidden : Level.Style;
			Level.State = chkOpened.Checked ? (byte)LevelState.Opened : Level.Style;
		}
		private void imgColor_Click(object sender, EventArgs e) {
			bool isActive = sender == imgClearColor;
			using (ObjectSelector selector = new ObjectSelector()) {
				int imgSize = 48;
				selector.ClientSize = new Size(imgSize * 7, imgSize * 5);
				foreach (KeyValuePair<short, Color> pair in Palette.Colors) {
					if (pair.Value.A == 0) { continue; }

					Bitmap img = new Bitmap(imgSize, imgSize);
					using (Graphics g = Graphics.FromImage(img)) {
						using (SolidBrush brush = new SolidBrush(pair.Value)) {
							g.FillRectangle(brush, 0, 0, imgSize, imgSize);
						}
					}
					ListItem item = new ListItem(new KeyValuePair<short, Color>(pair.Key, pair.Value), pair.Key.ToString("00000"), img);
					selector.AddItem(item, pair.Key == (isActive ? Level.ActiveColor : Level.Color));
				}

				selector.SortItems();
				selector.BackColor = Palette.Edge;
				selector.Icon = this.Icon;
				DialogResult result = selector.ShowDialog(this);
				if (result == DialogResult.OK) {
					KeyValuePair<short, Color> pair = (KeyValuePair<short, Color>)selector.SelectedItem;
					if (isActive) {
						Level.ActiveColor = pair.Key;
						imgClearColor.BackColor = pair.Value;
						UpdateIcon();
					} else {
						Level.Color = pair.Key;
						imgColor.BackColor = pair.Value;
						UpdateIcon();
					}
				}
			}
		}
		private void imgIcon_Click(object sender, EventArgs e) {
			if (imgIcon.Cursor != Cursors.Hand) { return; }

			using (ObjectSelector selector = new ObjectSelector()) {
				int imgSize = 36;
				Rectangle rect = new Rectangle(0, 0, imgSize, imgSize);
				Color color = Palette.Colors[Level.ActiveColor >= 0 ? Level.ActiveColor : Level.Color];
				int spriteCount = 0;
				foreach (Sprite sprite in Reader.Sprites.Values) {
					if (sprite.Name.IndexOf("img_") == 0) { continue; }

					spriteCount++;
					string name = sprite.Name;
					int index = name.IndexOf("text");
					if (index == 0) {
						name = name.Substring(5) + "_text";
					}

					Bitmap img = new Bitmap(imgSize, imgSize);
					using (Graphics g = Graphics.FromImage(img)) {
						Renderer.DrawSprite(g, rect, sprite, color);
					}
					ListItem item = new ListItem(sprite, name, img);
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
					Sprite sprite = (Sprite)selector.SelectedItem;
					Level.Sprite = sprite.Name;
					Level.SpriteInRoot = sprite.IsRoot;
					UpdateIcon();
				}
			}
		}
		private void numNumber_ValueChanged(object sender, EventArgs e) {
			Level.Number = (byte)numNumber.Value;
			UpdateIcon();
		}
		private void numNumber_Enter(object sender, EventArgs e) {
			NumericUpDown num = (NumericUpDown)sender;
			if (num.Text.Length > 0) {
				num.Select(0, num.Text.Length);
			}
		}
		private void UpdateIcon() {
			if (imgIcon.Image != null) {
				imgIcon.Image.Dispose();
			}
			imgIcon.Image = Renderer.DrawSprite(Level, imgIcon.Width, Palette);
		}
	}
}