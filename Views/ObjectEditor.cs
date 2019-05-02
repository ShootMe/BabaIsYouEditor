using BabaIsYou.Controls;
using BabaIsYou.Map;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
namespace BabaIsYou.Views {
	public partial class ObjectEditor : Form {
		public Palette Palette { get; set; }
		public Item Edit { get; set; }
		public Grid Map { get; set; }
		private Item DefaultItem;
		public ObjectEditor() {
			InitializeComponent();
			Renderer.SetFonts(this);

			foreach (TextType type in Enum.GetValues(typeof(TextType))) {
				cboTextType.Items.Add(type);
			}
			foreach (Tiling type in Enum.GetValues(typeof(Tiling))) {
				cboTiling.Items.Add(type);
			}
		}

		private void ObjectEditor_Load(object sender, EventArgs e) {
			DefaultItem = Reader.DefaultsByID[Edit.ID].Copy();
			Text = $"Edit - {DefaultItem.Object} - {DefaultItem.Name}";

			imgOriginal.Image = Renderer.DrawSprite(DefaultItem, imgOriginal.Width, Palette);

			UpdateImages();
		}
		private void numLayer_ValueChanged(object sender, EventArgs e) {
			Edit.Layer = (byte)numLayer.Value;
		}
		private void cboTiling_SelectedIndexChanged(object sender, EventArgs e) {
			if (cboTiling.SelectedIndex >= 0) {
				Edit.Tiling = (byte)(Tiling)cboTiling.Items[cboTiling.SelectedIndex];
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
				Edit.Type = (byte)(TextType)cboTextType.Items[cboTextType.SelectedIndex];
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
				Color color = Palette.Colors[Edit.ActiveColor >= 0 ? Edit.ActiveColor : Edit.Color];
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
					selector.AddItem(item, sender == imgObject ? Edit.Name.Equals(sprite.Name, StringComparison.OrdinalIgnoreCase) : Edit.Sprite.Equals(sprite.Name, StringComparison.OrdinalIgnoreCase));
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
					Sprite sprite = (Sprite)selector.SelectedItem;
					Edit.Name = sender == imgImage ? Edit.Name : sprite.Name;
					Edit.Sprite = sender == imgImage ? sprite.Name : Edit.Sprite;
					Edit.SpriteInRoot = sender == imgImage ? sprite.IsRoot : Edit.SpriteInRoot;
					Edit.IsObject = Edit.Name.IndexOf("text_") < 0;
					if (Edit.IsObject) {
						Edit.ActiveColor = -1;
						Edit.OperatorType = null;
						Edit.ArgExtra = null;
					}

					Item item;
					if (sender == imgObject) {
						if (Reader.DefaultsByName.TryGetValue(Edit.Name, out item)) {
							Edit.Type = item.Type;
							if (!Edit.IsObject) {
								if (Edit.ActiveColor < 0) {
									Edit.ActiveColor = Edit.Color;
								}
								Edit.OperatorType = item.OperatorType;
								Edit.ArgExtra = item.ArgExtra;
							}
						} else {
							Edit.Type = 0;
						}
					}

					if (sender == imgImage) {
						if (Reader.DefaultsByName.TryGetValue(Edit.Sprite, out item)) {
							Edit.Tiling = item.Tiling;
							if (!Edit.IsObject && Edit.ActiveColor < 0) {
								Edit.ActiveColor = Edit.Color;
							}
						} else {
							switch (sprite.MaxIndex) {
								case 31: Edit.Tiling = (byte)Tiling.Character; break;
								case 27: Edit.Tiling = (byte)Tiling.Animated; break;
								case 24: Edit.Tiling = (byte)Tiling.Directional; break;
								case 15: Edit.Tiling = (byte)Tiling.Tiled; break;
								case 3: Edit.Tiling = (byte)Tiling.SingleAnimated; break;
								default: Edit.Tiling = (byte)Tiling.None; break;
							}
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
					selector.AddItem(item, pair.Key == (isActive ? Edit.ActiveColor : Edit.Color));
				}

				selector.SortItems();
				selector.BackColor = Palette.Edge;
				selector.Icon = this.Icon;
				DialogResult result = selector.ShowDialog(this);
				if (result == DialogResult.OK) {
					KeyValuePair<short, Color> pair = (KeyValuePair<short, Color>)selector.SelectedItem;
					if (isActive) {
						Edit.ActiveColor = pair.Key;
					} else {
						Edit.Color = pair.Key;
					}

					UpdateImages();
				}
			}
		}
		private void UpdateImages() {
			Edit.Active = false;
			DrawImage(imgNormal);
			Edit.Active = true;

			imgActive.Visible = !Edit.IsObject;
			lblActiveColor.Visible = !Edit.IsObject;
			lblTextType.Visible = !Edit.IsObject;
			cboTextType.Visible = !Edit.IsObject;
			lblArgExtra.Visible = !Edit.IsObject;
			txtArgExtra.Visible = !Edit.IsObject;
			lblOperatorType.Visible = !Edit.IsObject;
			txtOperatorType.Visible = !Edit.IsObject;
			if (!Edit.IsObject) {
				ClientSize = new Size(ClientSize.Width, 315);
				DrawImage(imgActive);
			} else {
				ClientSize = new Size(ClientSize.Width, 274);
			}

			toolTips.SetToolTip(imgObject, Edit.Name);
			DrawImage(imgObject, true);
			toolTips.SetToolTip(imgImage, Edit.Sprite);
			DrawImage(imgImage);

			numLayer.Value = Edit.Layer;
			cboTextType.SelectedItem = (TextType)Edit.Type;
			cboTiling.SelectedItem = (Tiling)Edit.Tiling;
			txtArgExtra.Text = Edit.ArgExtra;
			txtOperatorType.Text = Edit.OperatorType;
		}
		private void DrawImage(PictureBox img, bool useNameOnly = false) {
			if (img.Image != null) {
				img.Image.Dispose();
			}
			if (useNameOnly) {
				if (!Reader.Sprites.ContainsKey(Edit.Name)) {
					img.Image = null;
				} else {
					string image = Edit.Sprite;
					Edit.Sprite = Edit.Name;
					img.Image = Renderer.DrawSprite(Edit, img.Width, Palette);
					Edit.Sprite = image;
				}
			} else {
				img.Image = Renderer.DrawSprite(Edit, img.Width, Palette);
			}
		}
		private void btnReset_Click(object sender, EventArgs e) {
			Edit = DefaultItem.Copy();
			numLayer.Value = DefaultItem.Layer;
			cboTextType.SelectedValue = (TextType)DefaultItem.Type;
			cboTiling.SelectedValue = (Tiling)DefaultItem.Tiling;
			UpdateImages();
		}
		private void btnSave_Click(object sender, EventArgs e) {
			bool hasChanges = Map.UpdateChanges(Edit);
			Edit.Changed = hasChanges;
			Map.ApplyChanges();
			this.DialogResult = DialogResult.OK;
			this.Close();
		}
		private void txtOperatorType_TextChanged(object sender, EventArgs e) {
			Edit.OperatorType = string.IsNullOrEmpty(txtOperatorType.Text) ? null : txtOperatorType.Text;
		}
		private void txtArgExtra_TextChanged(object sender, EventArgs e) {
			Edit.ArgExtra = string.IsNullOrEmpty(txtArgExtra.Text) ? null : txtArgExtra.Text;
		}
	}
}