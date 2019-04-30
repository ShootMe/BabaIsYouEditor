using BabaIsYou.Controls;
using BabaIsYou.Map;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
namespace BabaIsYou.Views {
	public partial class SpecialEdit : Form {
		public ListPanel LevelList { get; set; }
		public Palette Palette { get; set; }
		public Grid Map { get; set; }
		public Special Edit { get; set; }
		private Special specialCopy;
		private Flower flower;
		private GameControl gameControl;
		private Level level;
		private LevelEdit levelEdit;
		public SpecialEdit() {
			InitializeComponent();
		}

		private void SpecialEdit_Load(object sender, EventArgs e) {
			specialCopy = (Special)Edit.Copy();

			levelEdit = new LevelEdit();
			levelEdit.Palette = Palette;
			levelEdit.LevelList = LevelList;
			levelEdit.NoIcon = true;
			levelEdit.BackColor = this.BackColor;
			levelEdit.FormBorderStyle = FormBorderStyle.None;
			levelEdit.TopLevel = false;
			levelEdit.Parent = panelLevelEdit;

			foreach (ControlType type in Enum.GetValues(typeof(ControlType))) {
				if (type != ControlType.Unknown) {
					cboControls.Items.Add(type);
				}
			}

			switch ((SpecialType)specialCopy.Type) {
				case SpecialType.Controls:
					gameControl = specialCopy.GetGameControl();
					chkControls.Checked = true;
					break;
				case SpecialType.Flower:
					flower = specialCopy.GetFlower();
					chkFlower.Checked = true;
					break;
				default:
					level = specialCopy.GetLevel();
					for (int i = 0; i < LevelList.Items.Count; i++) {
						ListItem item = LevelList.Items[i];
						Grid map = (Grid)item.Value;
						if (map.FileName == level.File) {
							level.Name = map.Name;
							break;
						}
					}
					chkLevel.Checked = true;
					break;
			}
		}
		private void btnSave_Click(object sender, EventArgs e) {
			switch ((SpecialType)specialCopy.Type) {
				case SpecialType.Controls:
					Edit.Type = (byte)SpecialType.Controls;
					Edit.Object = $"controls,{gameControl.Type.ToString().ToLower()}";
					break;
				case SpecialType.Flower:
					Edit.Type = (byte)SpecialType.Flower;
					Edit.Object = $"flower,{Reader.ShortToCoordinate(flower.Color)},{flower.Radius}";
					break;
				default:
					levelEdit.SaveLevel();
					string state = level.State == 0 ? "" : $",{level.State}";
					Edit.Type = (byte)SpecialType.Level;
					Edit.Object = $"level,{level.File},{level.Style},{level.Number}{state}";
					level = specialCopy.GetLevel();
					break;
			}
			this.DialogResult = DialogResult.OK;
			this.Close();
		}
		private void chkType_CheckedChanged(object sender, EventArgs e) {
			if (specialCopy == null) { return; }

			bool visible = sender == chkControls && chkControls.Checked;
			lblControl.Visible = visible;
			cboControls.Visible = visible;
			if (visible) {
				specialCopy.Type = (byte)SpecialType.Controls;
				ClientSize = new Size(ClientSize.Width, 119);
				if (gameControl == null) {
					specialCopy.Object = Special.CONTROLS;
					gameControl = specialCopy.GetGameControl();
				}
				cboControls.SelectedItem = gameControl.Type;
			}

			visible = sender == chkFlower && chkFlower.Checked;
			lblRadius.Visible = visible;
			numRadius.Visible = visible;
			lblColor.Visible = visible;
			imgColor.Visible = visible;
			if (visible) {
				specialCopy.Type = (byte)SpecialType.Flower;
				ClientSize = new Size(ClientSize.Width, 170);
				if (flower == null) {
					specialCopy.Object = Special.FLOWER;
					flower = specialCopy.GetFlower();
				}
				numRadius.Value = flower.Radius;
				imgColor.BackColor = Palette.Colors[flower.Color];
			}

			visible = sender == chkLevel && chkLevel.Checked;
			panelLevelEdit.Visible = visible;
			if (visible) {
				specialCopy.Type = (byte)SpecialType.Level;
				ClientSize = new Size(ClientSize.Width, 314);
				if (level == null) {
					specialCopy.Object = $"level,{Map.FileName},2,0";
					level = specialCopy.GetLevel();
				}
				levelEdit.Edit = level;
				levelEdit.Show();
			} else if (levelEdit.Visible) {
				levelEdit.Hide();
			}
		}
		private void numRadius_ValueChanged(object sender, EventArgs e) {
			if (flower == null) { return; }

			flower.Radius = (byte)numRadius.Value;
		}
		private void cboControls_SelectedIndexChanged(object sender, EventArgs e) {
			if (gameControl == null) { return; }

			gameControl.Type = cboControls.SelectedItem == null ? ControlType.Idle : (ControlType)cboControls.SelectedItem;
			if (cboControls.SelectedItem == null) {
				cboControls.SelectedItem = ControlType.Idle;
			}
		}
		private void imgColor_Click(object sender, EventArgs e) {
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
					selector.AddItem(item, pair.Key == flower.Color);
				}

				selector.SortItems();
				selector.BackColor = Palette.Edge;
				selector.Icon = this.Icon;
				DialogResult result = selector.ShowDialog(this);
				if (result == DialogResult.OK) {
					KeyValuePair<short, Color> pair = (KeyValuePair<short, Color>)selector.SelectedItem;
					flower.Color = pair.Key;
					imgColor.BackColor = pair.Value;
				}
			}
		}
		private void SpecialEdit_FormClosing(object sender, FormClosingEventArgs e) {
			if (levelEdit != null) {
				levelEdit.Dispose();
				levelEdit = null;
			}
		}
	}
}