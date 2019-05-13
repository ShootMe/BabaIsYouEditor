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

			if (!Enum.IsDefined(typeof(TextType), Edit.Type)) {
				cboTextType.Items.Add(Edit.Type);
				cboTextType.SelectedItem = Edit.Type;
			}
			if (Edit.Type != DefaultItem.Type && !Enum.IsDefined(typeof(TextType), DefaultItem.Type)) {
				cboTextType.Items.Add(DefaultItem.Type);
			}
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
				SetTextType((TextType)DefaultItem.Type);
			}
		}
		private void cboTextType_Validated(object sender, EventArgs e) {
			if (string.IsNullOrEmpty(cboTextType.Text)) {
				SetTextType((TextType)DefaultItem.Type);
			}
		}
		private void imgCurrent_Click(object sender, EventArgs e) {
			using (ObjectSelector selector = new ObjectSelector()) {
				int spriteCount = 0;
				foreach (Sprite sprite in Reader.Sprites.Values) {
					if (sprite.Name.IndexOf("img_") == 0 || sprite.Name.IndexOf("icon_") == 0) { continue; }

					spriteCount++;
				}
				Size size = Renderer.GetSizeForCount(spriteCount);

				int imgSize = WorldViewer.Instance.Width / (size.Width + 1);
				if (imgSize * (size.Height + 2) > WorldViewer.Instance.Height) {
					imgSize = WorldViewer.Instance.Height / (size.Height + 2);
				}
				if (imgSize > 48) { imgSize = 48; }

				Rectangle rect = new Rectangle(0, 0, imgSize, imgSize);
				Color color = Palette.Colors[Edit.ActiveColor >= 0 ? Edit.ActiveColor : Edit.Color];
				foreach (Sprite sprite in Reader.Sprites.Values) {
					if (sprite.Name.IndexOf("img_") == 0 || sprite.Name.IndexOf("icon_") == 0) { continue; }

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
				selector.ClientSize = new Size(imgSize * size.Width, imgSize * size.Height);
				selector.SortItems();

				DialogResult result = selector.ShowDialog(this);
				if (result == DialogResult.OK) {
					Sprite sprite = (Sprite)selector.SelectedItem;
					Edit.Name = sender == imgImage ? Edit.Name : sprite.Name;
					txtObjectText.Text = Edit.Name;
					Edit.Sprite = sender == imgImage ? sprite.Name : Edit.Sprite;
					Edit.SpriteInRoot = sender == imgImage ? sprite.IsRoot : Edit.SpriteInRoot;
					Edit.IsObject = Edit.Name.IndexOf("text_") < 0;
					if (Edit.IsObject) {
						Edit.ActiveColor = -1;
						Edit.ArgExtra = null;
						Edit.ArgType = null;
					}

					Item item;
					if (sender == imgObject) {
						if (Reader.DefaultsByName.TryGetValue(Edit.Name, out item)) {
							Edit.Type = item.Type;
							if (!Edit.IsObject) {
								if (Edit.ActiveColor < 0) {
									Edit.ActiveColor = Edit.Color;
								}
								Edit.ArgExtra = item.ArgExtra;
								Edit.ArgType = item.ArgType;
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
			lblArgType.Visible = !Edit.IsObject;
			txtArgType.Visible = !Edit.IsObject;
			if (!Edit.IsObject) {
				ClientSize = new Size(ClientSize.Width, 344);
				DrawImage(imgActive);
			} else {
				ClientSize = new Size(ClientSize.Width, 264);
			}

			toolTips.SetToolTip(imgObject, Edit.Name);
			DrawImage(imgObject, true);
			toolTips.SetToolTip(imgImage, Edit.Sprite);
			DrawImage(imgImage);

			numLayer.Value = Edit.Layer;
			SetTextType((TextType)Edit.Type);
			cboTiling.SelectedItem = (Tiling)Edit.Tiling;
			txtArgExtra.Text = Edit.ArgExtra;
			txtArgType.Text = Edit.ArgType;
			txtObjectText.Text = Edit.Name;
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
			SetTextType((TextType)DefaultItem.Type);
			cboTiling.SelectedItem = (Tiling)DefaultItem.Tiling;
			UpdateImages();
		}
		private void btnSave_Click(object sender, EventArgs e) {
			bool hasChanges = Map.UpdateChanges(Edit);
			Edit.Changed = hasChanges;
			Map.ApplyChanges();
			this.DialogResult = DialogResult.OK;
			this.Close();
		}
		private void SetTextType(TextType textType) {
			cboTextType.SelectedItem = textType;
			if (cboTextType.SelectedItem == null) {
				cboTextType.SelectedItem = (byte)textType;
			}
		}
		private void txtArgExtra_TextChanged(object sender, EventArgs e) {
			Edit.ArgExtra = string.IsNullOrEmpty(txtArgExtra.Text) ? null : txtArgExtra.Text;
		}
		private void txtArgType_TextChanged(object sender, EventArgs e) {
			Edit.ArgType = string.IsNullOrEmpty(txtArgType.Text) ? null : txtArgType.Text;
		}
		private void txtObjectText_TextChanged(object sender, EventArgs e) {
			Edit.Name = txtObjectText.Text;
			DrawImage(imgObject, true);
		}
		private void txtObjectText_Validated(object sender, EventArgs e) {
			if (string.IsNullOrEmpty(txtObjectText.Text)) {
				txtObjectText.Text = DefaultItem.Name;
			}
			Edit.Name = txtObjectText.Text;
			DrawImage(imgObject, true);
		}
	}
}