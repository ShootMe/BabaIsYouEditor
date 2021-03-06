﻿using BabaIsYou.Controls;
using BabaIsYou.Map;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
namespace BabaIsYou.Views {
	public partial class LevelEdit : Form {
		public ListPanel LevelList { get; set; }
		public Palette Palette { get; set; }
		public Level Edit { get; set; }
		public bool NoIcon { get; set; }
		private Level levelCopy;
		public LevelEdit() {
			InitializeComponent();
			Renderer.SetFonts(this);
		}

		private void LevelEdit_Shown(object sender, EventArgs e) {
			levelCopy = (Level)Edit.Copy();
			txtName.Text = levelCopy.Name;
			txtFile.Text = levelCopy.File;
			numNumber.Value = levelCopy.Number;
			switch ((LevelStyle)levelCopy.Style) {
				case LevelStyle.Dot: chkDot.Checked = true; break;
				case LevelStyle.Letter: chkLetter.Checked = true; break;
				case LevelStyle.Icon:
					UpdateIcon();
					if (NoIcon) {
						chkDot.Checked = true;
					} else {
						chkIcon.Checked = true;
					}
					break;
				default: chkNumber.Checked = true; break;
			}
			if (NoIcon) {
				btnSave.Visible = false;
				chkIcon.Visible = false;
				lblColor.Visible = false;
				imgColor.Visible = false;
				lblClearColor.Visible = false;
				imgClearColor.Visible = false;
				lblIcon.Location = new Point(144, 150);
				imgIcon.Location = new Point(133, 170);
			}
			switch ((LevelState)levelCopy.State) {
				case LevelState.Normal: chkNormal.Checked = true; break;
				case LevelState.Opened: chkOpened.Checked = true; break;
				default: chkHidden.Checked = true; break;
			}
			imgColor.BackColor = Palette.Colors[levelCopy.Color];
			imgClearColor.BackColor = Palette.Colors[levelCopy.ActiveColor];
		}
		private void btnSave_Click(object sender, EventArgs e) {
			SaveLevel();
			this.DialogResult = DialogResult.OK;
			this.Close();
		}
		public void SaveLevel() {
			Edit.Name = levelCopy.Name;
			Edit.File = levelCopy.File;
			Edit.Number = levelCopy.Number;
			Edit.Style = levelCopy.Style;
			Edit.State = levelCopy.State;
			Edit.Color = levelCopy.Color;
			Edit.ActiveColor = levelCopy.ActiveColor;
			Edit.Sprite = levelCopy.Sprite;
			Edit.SpriteInRoot = levelCopy.SpriteInRoot;
		}
		private void btnSetLevel_Click(object sender, EventArgs e) {
			string text = txtFile.Text;
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
					levelCopy.Name = map.Name;
					levelCopy.File = map.FileName;
					txtFile.Text = map.FileName;
					txtName.Text = map.Name;
				}
			}
		}
		private void chkStyle_CheckedChanged(object sender, EventArgs e) {
			if (levelCopy == null) { return; }

			imgIcon.Cursor = sender == chkIcon && chkIcon.Checked ? Cursors.Hand : Cursors.Default;
			levelCopy.Style = chkDot.Checked ? (byte)LevelStyle.Dot : levelCopy.Style;
			levelCopy.Style = chkIcon.Checked ? (byte)LevelStyle.Icon : levelCopy.Style;
			levelCopy.Style = chkLetter.Checked ? (byte)LevelStyle.Letter : levelCopy.Style;
			levelCopy.Style = chkNumber.Checked ? (byte)LevelStyle.Number : levelCopy.Style;
			UpdateIcon();
		}
		private void chkState_CheckedChanged(object sender, EventArgs e) {
			if (levelCopy == null) { return; }

			levelCopy.State = chkNormal.Checked ? (byte)LevelState.Normal : levelCopy.State;
			levelCopy.State = chkHidden.Checked ? (byte)LevelState.Hidden : levelCopy.State;
			levelCopy.State = chkOpened.Checked ? (byte)LevelState.Opened : levelCopy.State;
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
					selector.AddItem(item, pair.Key == (isActive ? levelCopy.ActiveColor : levelCopy.Color));
				}

				selector.SortItems();
				selector.BackColor = Palette.Edge;
				selector.Icon = this.Icon;
				DialogResult result = selector.ShowDialog(this);
				if (result == DialogResult.OK) {
					KeyValuePair<short, Color> pair = (KeyValuePair<short, Color>)selector.SelectedItem;
					if (isActive) {
						levelCopy.ActiveColor = pair.Key;
						imgClearColor.BackColor = pair.Value;
						UpdateIcon();
					} else {
						levelCopy.Color = pair.Key;
						imgColor.BackColor = pair.Value;
						UpdateIcon();
					}
				}
			}
		}
		private void imgIcon_Click(object sender, EventArgs e) {
			if (imgIcon.Cursor != Cursors.Hand) { return; }

			using (ObjectSelector selector = new ObjectSelector()) {
				int spriteCount = 0;
				foreach (Sprite sprite in Reader.Sprites.Values) {
					if (sprite.Name.IndexOf("img_") == 0) { continue; }

					spriteCount++;
				}
				Size size = Renderer.GetSizeForCount(spriteCount);

				int imgSize = WorldViewer.Instance.Width / (size.Width + 1);
				if (imgSize * (size.Height + 2) > WorldViewer.Instance.Height) {
					imgSize = WorldViewer.Instance.Height / (size.Height + 2);
				}
				if (imgSize > 48) { imgSize = 48; }

				Rectangle rect = new Rectangle(0, 0, imgSize, imgSize);
				Color color = Palette.Colors[levelCopy.ActiveColor >= 0 ? levelCopy.ActiveColor : levelCopy.Color];
				foreach (Sprite sprite in Reader.Sprites.Values) {
					if (sprite.Name.IndexOf("img_") == 0) { continue; }

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
					selector.AddItem(item, levelCopy.Sprite == sprite.Name);
				}

				selector.BackColor = Palette.Edge;
				selector.Icon = this.Icon;


				selector.ClientSize = new Size(imgSize * size.Width, imgSize * size.Height);
				selector.SortItems();

				DialogResult result = selector.ShowDialog(this);
				if (result == DialogResult.OK) {
					Sprite sprite = (Sprite)selector.SelectedItem;
					levelCopy.Sprite = sprite.Name;
					levelCopy.SpriteInRoot = sprite.IsRoot;
					UpdateIcon();
				}
			}
		}
		private void numNumber_ValueChanged(object sender, EventArgs e) {
			if (levelCopy == null) { return; }

			levelCopy.Number = (byte)numNumber.Value;
			UpdateIcon();
		}
		private void UpdateIcon() {
			if (imgIcon.Image != null) {
				imgIcon.Image.Dispose();
			}
			imgIcon.Image = Renderer.DrawSprite(levelCopy, imgIcon.Width, Palette);
		}
	}
}