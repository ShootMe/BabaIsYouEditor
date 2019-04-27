using BabaIsYou.Controls;
using BabaIsYou.Map;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
namespace BabaIsYou.Views {
	public partial class ObjectEditor : Form {
		public Palette Palette { get; set; }
		public Item Sprite { get; set; }
		public Grid Map { get; set; }
		private Item DefaultItem;
		public ObjectEditor() {
			InitializeComponent();
			foreach (TextType type in Enum.GetValues(typeof(TextType))) {
				cboTextType.Items.Add(type);
			}
			foreach (Tiling type in Enum.GetValues(typeof(Tiling))) {
				cboTiling.Items.Add(type);
			}
		}

		private void ObjectEditor_Shown(object sender, EventArgs e) {
			DefaultItem = Reader.DefaultsByID[Sprite.ID].Copy();
			Text = $"Edit - {DefaultItem.Object} - {DefaultItem.Name}";

			imgOriginal.Image = DrawSprite(DefaultItem, imgOriginal.Width);

			UpdateImages();
		}
		private Bitmap DrawSprite(Item item, int imgSize) {
			Bitmap img = new Bitmap(imgSize, imgSize);
			Rectangle rect = new Rectangle(0, 0, img.Width, img.Height);
			using (Graphics g = Graphics.FromImage(img)) {
				using (SolidBrush brush = new SolidBrush(Palette.Background)) {
					g.FillRectangle(brush, rect);
				}
				Renderer.DrawSprite(null, g, rect, item, Palette);
			}
			return img;
		}
		private void numLayer_ValueChanged(object sender, EventArgs e) {
			Sprite.Layer = (byte)numLayer.Value;
		}
		private void cboTiling_SelectedIndexChanged(object sender, EventArgs e) {
			if (cboTiling.SelectedIndex >= 0) {
				Sprite.Tiling = (byte)(Tiling)cboTiling.Items[cboTiling.SelectedIndex];
			} else {
				cboTiling.SelectedItem = (Tiling)DefaultItem.Tiling;
			}
		}
		private void cboTiling_Validated(object sender, EventArgs e) {
			if (string.IsNullOrEmpty(cboTiling.Text)) {
				cboTiling.SelectedItem = (Tiling)DefaultItem.Tiling;
			}
		}
		private void cboTextType_SelectedIndexChanged(object sender, EventArgs e) {
			if (cboTextType.SelectedIndex >= 0) {
				Sprite.Type = (byte)(TextType)cboTextType.Items[cboTextType.SelectedIndex];
			} else {
				cboTextType.SelectedItem = (TextType)DefaultItem.Type;
			}
		}
		private void cboTextType_Validated(object sender, EventArgs e) {
			if (string.IsNullOrEmpty(cboTextType.Text)) {
				cboTextType.SelectedItem = (TextType)DefaultItem.Type;
			}
		}
		private void imgCurrent_Click(object sender, EventArgs e) {
			using (ObjectSelector selector = new ObjectSelector()) {
				int imgSize = 36;
				Rectangle rect = new Rectangle(0, 0, imgSize, imgSize);
				Color color = Palette.Colors[Sprite.ActiveColor >= 0 ? Sprite.ActiveColor : Sprite.Color];
				int spriteCount = 0;
				foreach (Sprite sprite in Reader.Sprites.Values) {
					if (sprite.Name.IndexOf("img_") == 0 || sprite.Name.IndexOf("icon_") == 0) { continue; }

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
					Sprite.Name = sender == imgImage ? Sprite.Name : sprite.Name;
					Sprite.Sprite = sender == imgImage ? sprite.Name : Sprite.Sprite;
					Sprite.SpriteInRoot = sender == imgImage ? sprite.IsRoot : Sprite.SpriteInRoot;
					Sprite.IsObject = Sprite.Name.IndexOf("text_") < 0;
					if (Sprite.IsObject) {
						Sprite.ActiveColor = -1;
					}

					Item item;
					if (Reader.DefaultsByName.TryGetValue(Sprite.Name, out item)) {
						Sprite.Type = item.Type;
						if (!Sprite.IsObject && Sprite.ActiveColor == -1) {
							Sprite.ActiveColor = item.ActiveColor;
						}
					} else {
						Sprite.Type = 0;
					}
					if (Reader.DefaultsByName.TryGetValue(Sprite.Sprite, out item)) {
						Sprite.Tiling = item.Tiling;
					} else {
						switch (sprite.MaxIndex) {
							case 31: Sprite.Tiling = (byte)Tiling.Character; break;
							case 27: Sprite.Tiling = (byte)Tiling.Animated; break;
							case 24: Sprite.Tiling = (byte)Tiling.Directional; break;
							case 15: Sprite.Tiling = (byte)Tiling.Tiled; break;
							case 3: Sprite.Tiling = (byte)Tiling.SingleAnimated; break;
							default: Sprite.Tiling = (byte)Tiling.None; break;
						}
					}

					UpdateImages();
				}
			}
		}
		private void imgColor_Click(object sender, EventArgs e) {
			bool isActive = sender == imgActive;
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
					selector.AddItem(item, pair.Key == (isActive ? Sprite.ActiveColor : Sprite.Color));
				}

				selector.SortItems();
				selector.BackColor = Palette.Edge;
				selector.Icon = this.Icon;
				DialogResult result = selector.ShowDialog(this);
				if (result == DialogResult.OK) {
					KeyValuePair<short, Color> pair = (KeyValuePair<short, Color>)selector.SelectedItem;
					if (isActive) {
						Sprite.ActiveColor = pair.Key;
					} else {
						Sprite.Color = pair.Key;
					}

					UpdateImages();
				}
			}
		}
		private void UpdateImages() {
			Sprite.Active = false;
			DrawImage(imgNormal);
			Sprite.Active = true;

			imgActive.Visible = !Sprite.IsObject;
			lblActiveColor.Visible = !Sprite.IsObject;
			lblTextType.Visible = !Sprite.IsObject;
			cboTextType.Visible = !Sprite.IsObject;
			if (!Sprite.IsObject) {
				DrawImage(imgActive);
			}

			DrawImage(imgCurrent, true);
			DrawImage(imgImage);

			numLayer.Value = Sprite.Layer;
			cboTextType.SelectedItem = (TextType)Sprite.Type;
			cboTiling.SelectedItem = (Tiling)Sprite.Tiling;
		}
		private void DrawImage(PictureBox img, bool useNameOnly = false) {
			if (img.Image != null) {
				img.Image.Dispose();
			}
			if (useNameOnly) {
				string image = Sprite.Sprite;
				Sprite.Sprite = Sprite.Name;
				img.Image = DrawSprite(Sprite, img.Width);
				Sprite.Sprite = image;
			} else {
				img.Image = DrawSprite(Sprite, img.Width);
			}
		}
		private void btnReset_Click(object sender, EventArgs e) {
			Sprite = DefaultItem.Copy();
			numLayer.Value = DefaultItem.Layer;
			cboTextType.SelectedValue = (TextType)DefaultItem.Type;
			cboTiling.SelectedValue = (Tiling)DefaultItem.Tiling;
			UpdateImages();
		}
		private void btnSave_Click(object sender, EventArgs e) {
			bool hasChanges = true;
			hasChanges = Map.UpdateChanges(Sprite.ID, "activecolour", DefaultItem.ActiveColor != Sprite.ActiveColor ? Reader.ShortToCoordinate(Sprite.ActiveColor) : null);
			hasChanges = Map.UpdateChanges(Sprite.ID, "colour", DefaultItem.Color != Sprite.Color ? Reader.ShortToCoordinate(Sprite.Color) : null);
			hasChanges = Map.UpdateChanges(Sprite.ID, "layer", DefaultItem.Layer != Sprite.Layer ? Sprite.Layer.ToString() : null);
			hasChanges = Map.UpdateChanges(Sprite.ID, "type", DefaultItem.Type != Sprite.Type ? Sprite.Type.ToString() : null);
			hasChanges = Map.UpdateChanges(Sprite.ID, "tiling", DefaultItem.Tiling != Sprite.Tiling ? (Sprite.Tiling == 255 ? -1 : (int)Sprite.Tiling).ToString() : null);
			hasChanges = Map.UpdateChanges(Sprite.ID, "name", DefaultItem.Name != Sprite.Name ? Sprite.Name : null);
			hasChanges = Map.UpdateChanges(Sprite.ID, "image", DefaultItem.Sprite != Sprite.Sprite ? Sprite.Sprite : null);
			hasChanges = Map.UpdateChanges(Sprite.ID, "unittype", DefaultItem.IsObject != Sprite.IsObject ? Sprite.IsObject ? "object" : "text" : null);
			hasChanges = Map.UpdateChanges(Sprite.ID, "root", DefaultItem.SpriteInRoot != Sprite.SpriteInRoot ? Sprite.SpriteInRoot ? "1" : "0" : null);
			Sprite.Changed = hasChanges;
			Map.ApplyChanges();
			this.DialogResult = DialogResult.OK;
			this.Close();
		}
		private void numLayer_Enter(object sender, EventArgs e) {
			NumericUpDown num = (NumericUpDown)sender;
			if (num.Text.Length > 0) {
				num.Select(0, num.Text.Length);
			}
		}
	}
}