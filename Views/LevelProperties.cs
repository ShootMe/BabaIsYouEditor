using BabaIsYou.Controls;
using BabaIsYou.Map;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
namespace BabaIsYou.Views {
	public partial class LevelProperties : Form {
		public ListPanel LevelList { get; set; }
		public Grid Map { get; set; }
		private HashSet<string> filterKeys = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
		private HashSet<string> removedKeys = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
		private bool loading = true;
		public LevelProperties() {
			InitializeComponent();

			foreach (Music type in Enum.GetValues(typeof(Music))) {
				cboMusic.Items.Add(type);
			}
			foreach (Particles type in Enum.GetValues(typeof(Particles))) {
				cboParticles.Items.Add(type);
			}
			filterKeys.Add("name");
			filterKeys.Add("subtitle");
			filterKeys.Add("music");
			filterKeys.Add("particles");
			filterKeys.Add("levels");
			filterKeys.Add("paths");
			filterKeys.Add("palette");
			filterKeys.Add("paletteroot");
			filterKeys.Add("leveltype");
			filterKeys.Add("specials");
			filterKeys.Add("customparent");
			filterKeys.Add("unlockcount");
			filterKeys.Add("selectorX");
			filterKeys.Add("selectorY");
		}

		private void LevelProperties_Load(object sender, EventArgs e) {
			txtName.Text = Map.Name;
			txtSubtitle.Text = Map.Info["general", "subtitle"];
			string music = Map.Info["general", "music"];
			Music musicValue;
			if (Enum.TryParse<Music>(music, true, out musicValue)) {
				cboMusic.SelectedItem = musicValue;
			} else {
				cboMusic.SelectedItem = Music.None;
			}

			string particles = Map.Info["general", "particles"];
			Particles particlesValue;
			if (Enum.TryParse<Particles>(particles, true, out particlesValue)) {
				cboParticles.SelectedItem = particlesValue;
			} else {
				cboParticles.SelectedItem = Particles.None;
			}

			txtCustomParent.Text = Map.Info["general", "customparent"];
			chkCallingParent.Checked = string.IsNullOrEmpty(txtCustomParent.Text);

			string unlockCount = Map.Info["general", "unlockcount"];
			numUnlockCount.Value = string.IsNullOrEmpty(unlockCount) ? 0 : int.Parse(unlockCount);

			numWidth.Value = Map.Width;
			numHeight.Value = Map.Height;

			gridOther.BackgroundColor = Reader.Palettes[Map.Palette].Background;
			DataTable dt = new DataTable();
			dt.Columns.Add("Key", typeof(string));
			dt.Columns.Add("Value", typeof(string));
			Dictionary<string, string> info = Map.Info["general"];
			foreach (KeyValuePair<string, string> pair in info) {
				if (!filterKeys.Contains(pair.Key)) {
					dt.Rows.Add(pair.Key, pair.Value);
					removedKeys.Add(pair.Key);
				}
			}
			gridOther.DataSource = dt;
			loading = false;
			ClientSize = new Size(ClientSize.Width, 225 + (dt.Rows.Count + 2) * 22);
		}
		private void chkCallingParent_CheckedChanged(object sender, EventArgs e) {
			if (chkCallingParent.Checked) {
				txtCustomParent.Text = string.Empty;
			}
		}
		private void txtCustomParent_TextChanged(object sender, EventArgs e) {
			chkCallingParent.Checked = string.IsNullOrEmpty(txtCustomParent.Text);
		}
		private void btnSetLevel_Click(object sender, EventArgs e) {
			using (ObjectSelector selector = new ObjectSelector()) {
				selector.DrawText = true;

				int addedCount = 0;
				int imgWidth = 0;
				int imgHeight = 0;
				for (int i = 0; i < LevelList.Items.Count; i++) {
					ListItem level = LevelList.Items[i];
					Grid map = (Grid)level.Value;
					if (map.CountOfType<Level>() == 0) { continue; }
					addedCount++;

					imgWidth = level.Extra.Width;
					imgHeight = level.Extra.Height;
					ListItem item = new ListItem(map, map.Name, level.Extra);
					item.BackColor = level.BackColor;
					selector.AddItem(item, map.FileName.Equals(txtCustomParent.Text, StringComparison.OrdinalIgnoreCase));
				}

				selector.BackColor = this.BackColor;
				selector.SortItems();
				selector.Icon = this.Icon;

				if (addedCount == 0) {
					MessageBox.Show(this, "No World level maps to choose from.");
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
					txtCustomParent.Text = map.FileName;
					chkCallingParent.Checked = false;
				}
			}
		}
		private void gridOther_RowValidating(object sender, DataGridViewCellCancelEventArgs e) {
			DataGridViewRow row = gridOther.Rows[e.RowIndex];
			string key = row.Cells["Key"].Value.ToString();
			string value = row.Cells["Value"].Value.ToString();

			if ((!row.IsNewRow || !string.IsNullOrEmpty(value)) && string.IsNullOrEmpty(key)) {
				gridOther.CurrentCell = row.Cells["Key"];
				e.Cancel = true;
				MessageBox.Show(this, "Please enter a value for Key.");
			} else if (filterKeys.Contains(key)) {
				gridOther.CurrentCell = row.Cells["Key"];
				e.Cancel = true;
				MessageBox.Show(this, "This Key is calculated for you and does not need to be added.");
			}
		}
		private void gridOther_DataSourceChanged(object sender, EventArgs e) {
			gridOther.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
			int i = 0;
			Setup("Key", i++, 120, "Key", DataGridViewContentAlignment.MiddleLeft);
			Setup("Value", i++, 0, "Value", DataGridViewContentAlignment.MiddleLeft);
		}
		public void Setup(string column, int index, int width = -1, string header = null, DataGridViewContentAlignment align = DataGridViewContentAlignment.MiddleCenter) {
			if (gridOther.Columns == null || gridOther.Columns[column] == null) { return; }
			gridOther.Columns[column].Visible = true;
			gridOther.Columns[column].DisplayIndex = index;
			gridOther.Columns[column].SortMode = DataGridViewColumnSortMode.Automatic;
			gridOther.Columns[column].DefaultCellStyle.Alignment = align;
			if (width > 0) {
				gridOther.Columns[column].Width = width;
			} else if (width == 0) {
				gridOther.Columns[column].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
			} else if (width < 0) {
				gridOther.Columns[column].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
			}
			gridOther.Columns[column].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
			if (header != null) {
				gridOther.Columns[column].HeaderText = header;
			}
		}
		private void btnSave_Click(object sender, EventArgs e) {
			if (string.IsNullOrEmpty(txtName.Text)) {
				txtName.Focus();
				MessageBox.Show(this, "Please enter a name.");
				return;
			}
			Map.Name = txtName.Text;
			Map.Info["general", "subtitle"] = string.IsNullOrEmpty(txtSubtitle.Text) ? null : txtSubtitle.Text.ToLower();
			Map.Info["general", "music"] = cboMusic.SelectedItem == null || (Music)cboMusic.SelectedItem == Music.None ? null : ((Music)cboMusic.SelectedItem).ToString().ToLower();
			Map.Info["general", "particles"] = cboParticles.SelectedItem == null || (Particles)cboParticles.SelectedItem == Particles.None ? null : ((Particles)cboParticles.SelectedItem).ToString().ToLower();
			Map.Info["general", "unlockcount"] = numUnlockCount.Value.ToString();
			Map.Info["general", "customparent"] = txtCustomParent.Text;

			if (Map.Width != numWidth.Value || Map.Height != numHeight.Value) {
				Map.Resize((int)numWidth.Value, (int)numHeight.Value);
			}

			DataTable dt = (DataTable)gridOther.DataSource;
			for (int i = 0; i < dt.Rows.Count; i++) {
				DataRow row = dt.Rows[i];
				string value = row["Value"].ToString();
				string key = row["Key"].ToString();
				Map.Info["general", key] = value;
				removedKeys.Remove(key);
			}
			foreach (string key in removedKeys) {
				Map.Info["general", key] = null;
			}

			this.DialogResult = DialogResult.OK;
			this.Close();
		}
		private void gridOther_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e) {
			if (!loading) {
				ClientSize = new Size(ClientSize.Width, 225 + (gridOther.Rows.Count + 1) * 22);
			}
		}
		private void gridOther_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e) {
			ClientSize = new Size(ClientSize.Width, 225 + (gridOther.Rows.Count + 1) * 22);
		}
	}
}