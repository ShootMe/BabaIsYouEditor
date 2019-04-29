using BabaIsYou.Controls;
using BabaIsYou.Map;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
namespace BabaIsYou.Views {
	public partial class WorldViewer : Form {
		private const string RegKey = @"HKEY_CURRENT_USER\SOFTWARE\BabaIsYou\Form";
		private static string TitleBarText = "Baba Editor v" + Assembly.GetExecutingAssembly().GetName().Version.ToString(3);
		private static string GameDirectory;
		private static string GameWorld = "baba";
		[STAThread]
		public static void Main() {
#if !DEBUG
			try {
#endif
			string path = @"C:\Program Files (x86)\Steam\steamapps\common\Baba Is You\Data\";
			GameDirectory = RegistryRead<string>("GameDirectory", path);
			if (!Directory.Exists(GameDirectory)) {
				GameDirectory = Environment.CurrentDirectory;
			}
			GameDirectory = GameDirectory.Replace('/', '\\');
			GameWorld = RegistryRead<string>("GameWorld", "baba");

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new WorldViewer());
#if !DEBUG
			} catch (Exception ex) {
				MessageBox.Show(ex.ToString(), "Application Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
#endif
		}
		private const int SpriteSize = 36;
		private const int MaxLayerCount = 3;
		private Grid map;
		private Bitmap textX;
		private Item currentObject;
		private List<ListItem> levelsToBeRemoved = new List<ListItem>();
		private bool addedObject;
		private bool? isAdding = null;
		private bool holdingControl = false;
		private string GameWorldName;

		public WorldViewer() {
			InitializeComponent();

			Text = TitleBarText;
			using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("BabaIsYou.Images.baba.ico")) {
				Icon = new Icon(stream);
			}

			statusLevel.Text = "Load world from the menu";
			statusLevelInfo.Text = "N/A";
			statusLevelName.Text = "N/A";
			statusPosition.Text = "[-1, -1]";
			statusSprite.Text = "N/A";

			using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("BabaIsYou.Images.error.png")) {
				textX = new Bitmap(stream);
			}

			menuItemShowDirections.Checked = RegistryRead<int>("ShowDirections", 0) != 0;
			menuItemShowStacked.Checked = RegistryRead<int>("ShowStacked", 0) != 0;
			mapViewer.ShowStacked = menuItemShowStacked.Checked;
			mapViewer.ShowDirections = menuItemShowDirections.Checked;
			txtLevelFilter.Visible = false;
			menuLevel.Enabled = false;
			menuPalette.Enabled = false;
			menuItemWorldProperties.Enabled = false;
			menuItemAddLevel.Enabled = false;
			menuItemRemoveLevel.Enabled = false;
			menuItemSaveWorld.Enabled = false;

			if (GameDirectory.IndexOf(@"Baba Is You\Data\", StringComparison.OrdinalIgnoreCase) > 0 && Directory.Exists(Path.Combine(GameDirectory, "Worlds", GameWorld))) {
				LoadWorld();
			}
		}

		private void LoadWorld() {
			RegistryWrite<string>("GameDirectory", GameDirectory);
			RegistryWrite<string>("GameWorld", GameWorld);

			menu.Enabled = false;
			imgBaba.Visible = true;
			this.BackColor = Color.Black;
			Info worldInfo = new Info(Path.Combine(GameDirectory, "Worlds", GameWorld, "world_data.txt"), false);
			GameWorldName = worldInfo["general", "name"];
			GameWorldName = string.IsNullOrEmpty(GameWorldName) ? GameWorld : GameWorldName;
			statusLevel.Text = $"Loading world  \"{GameWorldName}\"  ...";
			levelsToBeRemoved.Clear();
			mapViewer.Map = null;
			listLevels.Items.Clear();
			listObjects.Items.Clear();

			Thread thread = new Thread(delegate () {
#if !DEBUG
				try {
#endif
				string[] files = Directory.GetFiles(Path.Combine(GameDirectory, "Worlds", GameWorld), "*.l", SearchOption.TopDirectoryOnly);
				Reader.Initialize(GameDirectory, GameWorld);

				map = null;
				List<ListItem> newItems = new List<ListItem>();
				int imgSize = listLevels.Width;
				for (int i = 0; i < files.Length; i++) {
					string file = files[i];
					map = Reader.ReadMap(files[i]);
					if (map == null) { continue; }

					float widthRatio = imgSize / map.Width;
					Bitmap img = new Bitmap(imgSize, (int)(widthRatio * map.Height) + listLevels.Font.Height * 2);
					using (Graphics g = Graphics.FromImage(img)) {
						Renderer.Render(map, g, img.Width, img.Height);
					}
					Bitmap imgExtra = new Bitmap(200, 150);
					using (Graphics g = Graphics.FromImage(imgExtra)) {
						Renderer.Render(map, g, imgExtra.Width, imgExtra.Height);
					}
					ListItem item = new ListItem(map, map.Name, img);
					item.Extra = imgExtra;
					newItems.Add(item);
				}

				this.Invoke((MethodInvoker)delegate () {
					txtLevelFilter.Visible = true;

					statusLevel.Text = "N/A";
					listLevels.Items.AddRange(newItems);

					Text = $"{TitleBarText} - {GameWorldName} - {listLevels.Items.Count} Levels";

					menuPalette.DropDownItems.Clear();
					foreach (string name in Reader.Palettes.Keys) {
						string paletteName = $"{char.ToUpper(name[0])}{name.Substring(1, name.Length - 5)}";
						ToolStripMenuItem menuItem = new ToolStripMenuItem(paletteName, null, PaletteMenuItemClick);
						menuItem.CheckOnClick = true;
						menuItem.DisplayStyle = ToolStripItemDisplayStyle.Text;
						menuPalette.DropDownItems.Add(menuItem);
					}

					listLevels.Items.Sort();
					listLevels.SelectedIndex = -1;
					listLevels.SelectTopMostVisible();

					if (txtLevelFilter.ForeColor == Color.Black) {
						txtLevelFilter.Text = string.Empty;
						txtLevelFilter_Leave(null, null);
					}
					menu.Enabled = true;
					imgBaba.Visible = false;
					menuLevel.Enabled = true;
					menuPalette.Enabled = true;
					menuItemWorldProperties.Enabled = true;
					menuItemAddLevel.Enabled = true;
					menuItemRemoveLevel.Enabled = true;
					menuItemSaveWorld.Enabled = true;

					if (listLevels.SelectedItem == null) {
						AddSprites();
						SelectPalette("default.png");
					}
					listObjects.Focus();
				});
#if !DEBUG
				} catch (Exception ex) {
					MessageBox.Show(ex.ToString(), "Loading World Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					this.Invoke((MethodInvoker)delegate () {
						menu.Enabled = true;
						imgBaba.Visible = false;
						txtLevelFilter.Visible = false;
						statusLevel.Text = $"Load world from the menu";
					});
				}
#endif
			});
			thread.IsBackground = true;
			thread.Start();
		}
		private void Solver_Resize(object sender, EventArgs e) {
			listLevels.Invalidate();
			listObjects.Invalidate();
			mapViewer.Invalidate();
		}
		private void WorldViewer_KeyDown(object sender, KeyEventArgs e) {
			holdingControl = e.Control;
			if (holdingControl) {
				mapViewer.Invalidate();
			}
			if (e.KeyCode == Keys.Space && map != null && mapViewer.CurrentCell != null) {
				mapViewer_CellMouseDown(map, mapViewer.CurrentCell, new MouseEventArgs(MouseButtons.Middle, 1, 0, 0, 0));
			}
		}
		private void WorldViewer_KeyUp(object sender, KeyEventArgs e) {
			if (holdingControl) {
				holdingControl = false;
				mapViewer.Invalidate();
			}
		}
		private void WorldViewer_FormClosing(object sender, FormClosingEventArgs e) {
			RegistryWrite<int>("ShowStacked", menuItemShowStacked.Checked ? 1 : 0);
			RegistryWrite<int>("ShowDirections", menuItemShowDirections.Checked ? 1 : 0);
		}
		private void WorldViewer_Deactivate(object sender, EventArgs e) {
			holdingControl = false;
		}
		private void txtLevelFilter_Enter(object sender, EventArgs e) {
			if (txtLevelFilter.ForeColor == Color.Gray) {
				txtLevelFilter.Text = string.Empty;
				txtLevelFilter.ForeColor = Color.Black;
			}
		}
		private void txtLevelFilter_Leave(object sender, EventArgs e) {
			if (txtLevelFilter.ForeColor == Color.Black && string.IsNullOrEmpty(txtLevelFilter.Text)) {
				txtLevelFilter.ForeColor = Color.Gray;
				txtLevelFilter.Text = "Filter ...";
				listObjects.Focus();
			}
		}
		private void txtLevelFilter_TextChanged(object sender, EventArgs e) {
			if (txtLevelFilter.ForeColor == Color.Black) {
				string searchText = txtLevelFilter.Text;
				int size = listLevels.Items.Count;
				for (int i = 0; i < size; i++) {
					ListItem item = listLevels.Items[i];
					string text = item.Text;
					item.Visible = string.IsNullOrEmpty(searchText) || text.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0;
				}
				listLevels.EnsureSelectedInView();
			}
		}
		private static T RegistryRead<T>(string name, T defaultValue) {
			object objValue = null;
			try {
				objValue = Registry.GetValue(RegKey, name, null);
			} catch { }

			if (objValue is T) {
				return (T)objValue;
			}

			return defaultValue;
		}
		private static void RegistryWrite<T>(string name, T value) {
			try {
				Registry.SetValue(RegKey, name, value);
			} catch { }
		}
		private void listLevels_IndexChanged(int index, ListItem item) {
			if (item == null) {
				map = null;
				mapViewer.Map = null;
			} else {
				if (mapViewer.Map == (Grid)item.Value) { return; }

				map = (Grid)item.Value;
				mapViewer.Map = map;
				SelectPalette(map.Palette);
			}

			AddSprites();
			UpdateStatusBar();
			mapViewer.Invalidate();
			listObjects.Focus();
		}
		private void UpdateStatusBar() {
			statusLevel.Text = map == null ? "N/A" : map.FileName;
			statusLevelName.Text = map == null ? "N/A" : map.Name;
			statusLevelInfo.Text = map == null ? "N/A" : $"[{map.Width}, {map.Height}]";
		}
		private void listLevels_ItemClicked(ListItem item, MouseButtons buttons) {
			if (buttons == MouseButtons.Right && item != null) {
				OpenLevelProperties(item);
			}
		}
		private void UpdateCurrentLevel(ListItem levelItem, bool changed = true) {
			Grid mapToUpdate = (Grid)levelItem.Value;
			int imgSize = listLevels.Width;
			float widthRatio = imgSize / mapToUpdate.Width;
			Bitmap img = new Bitmap(imgSize, (int)(widthRatio * mapToUpdate.Height) + listLevels.Font.Height * 2);
			using (Graphics g = Graphics.FromImage(img)) {
				Renderer.Render(mapToUpdate, g, img.Width, img.Height);
			}

			levelItem.Text = mapToUpdate.Name;
			levelItem.Changed = changed;
			levelItem.Image.Dispose();
			levelItem.Image = img;

			listLevels.Invalidate();
			mapViewer.Invalidate();
		}
		private void AddSprites() {
			string currentText = listObjects.SelectedItem?.Text;
			listObjects.SelectedIndex = -1;
			listObjects.Items.Clear();

			currentObject = null;
			string paletteName = map == null ? "default.png" : map.Palette;
			Palette palette = Reader.Palettes[paletteName];
			foreach (Item item in Reader.DefaultsByName.Values) {
				if (item.ID <= 0 || string.IsNullOrEmpty(item.Sprite)) { continue; }

				ItemChange change;
				Item copy = item.Copy();
				if (map != null && map.Changes.TryGetValue(item.ID, out change)) {
					change.Apply(copy);
				}

				Bitmap img = new Bitmap(SpriteSize, SpriteSize);
				Rectangle rect = new Rectangle(0, 0, img.Width, img.Height);
				using (Graphics g = Graphics.FromImage(img)) {
					Renderer.DrawSprite(null, g, rect, copy, palette);
				}

				string name = copy.Name;
				int index = name.IndexOf("text");
				if (index == 0) {
					name = name.Substring(5) + "_text";
				}
				ListItem listItem = new ListItem(copy, name, img);
				listItem.Changed = copy.Changed;
				listItem.BackColor = palette.Background;
				listObjects.Items.Add(listItem);
			}
			int rowCount = listObjects.Items.Count / 24;
			if (rowCount * 24 < listObjects.Items.Count) {
				rowCount++;
			}
			splitObjectsLevel.SplitterDistance = SpriteSize * rowCount;
			listObjects.Items.Sort();
			listObjects.BackColor = palette.Edge;
			if (!listObjects.SelectItemWithText(currentText)) {
				listObjects.SelectedIndex = 0;
			}
		}
		private void statusAddLevel_ButtonClick(object sender, EventArgs e) {
			if (map == null) { return; }

			listObjects.SelectedItem = null;
			Level level = (Level)Level.DEFAULT.Copy();
			level.Name = map.Name;
			level.File = map.FileName;
			UpdateCurrentObject(level, true);
		}
		private void statusAddPath_ButtonClick(object sender, EventArgs e) {
			if (map == null) { return; }

			listObjects.SelectedItem = null;
			Line line = new Line();
			line.UpdateLine();
			UpdateCurrentObject(line, true);
		}
		private void statusSetSelector_ButtonClick(object sender, EventArgs e) {
			if (map == null) { return; }

			listObjects.SelectedItem = null;
			UpdateCurrentObject(Item.SELECTOR.Copy(), true);
		}
		private void listObjects_IndexChanged(int index, ListItem item) {
			UpdateCurrentObject((Item)(item == null ? null : item.Value), true);
		}
		private void UpdateCurrentObject(Item item, bool invalidate) {
			if (item == null) {
				currentObject = null;
				statusSprite.Text = "Empty";
			} else {
				currentObject = item;

				if (currentObject is Level) {
					statusSprite.Text = $"Level Object";
				} else if (currentObject is Line) {
					statusSprite.Text = $"Path Object";
				} else {
					statusSprite.Text = $"{currentObject.Name}";
				}
			}

			if (invalidate) {
				mapViewer.Invalidate();
			}
		}
		private void listObjects_ItemClicked(ListItem item, MouseButtons buttons) {
			if (buttons == MouseButtons.Right && map != null) {
				Palette palette = Reader.Palettes[map.Palette];
				using (ObjectEditor editor = new ObjectEditor()) {
					editor.Palette = palette;
					editor.Sprite = ((Item)item.Value).Copy();
					editor.Map = map;
					editor.BackColor = palette.Edge;
					editor.Icon = this.Icon;
					DialogResult result = editor.ShowDialog(this);
					if (result == DialogResult.OK) {
						UpdateSprite(item, editor.Sprite, palette);
						UpdateCurrentLevel(listLevels.SelectedItem);
					}
				}
			}
		}
		private void UpdateSprite(ListItem item, Item sprite, Palette palette) {
			Bitmap img = new Bitmap(SpriteSize, SpriteSize);
			Rectangle rect = new Rectangle(0, 0, img.Width, img.Height);
			using (Graphics g = Graphics.FromImage(img)) {
				Renderer.DrawSprite(null, g, rect, sprite, palette);
			}
			string name = sprite.Name;
			int index = name.IndexOf("text");
			if (index == 0) {
				name = name.Substring(5) + "_text";
			}
			item.Text = name;
			item.Changed = sprite.Changed;
			item.Value = sprite;
			item.Image.Dispose();
			item.Image = img;

			if (listObjects.SelectedItem == item) {
				UpdateCurrentObject((Item)(item == null ? null : item.Value), false);
			}
			listObjects.Invalidate();
		}
		private void mapViewer_MouseUp(object sender, MouseEventArgs e) {
			isAdding = null;
			mapViewer.Invalidate();
		}
		private void mapViewer_CellMouseDown(Grid map, Cell cell, MouseEventArgs e) {
			if (e.Button == MouseButtons.Left) {
				if (currentObject == null) { return; }

				bool hasObject = cell.ContainsObjectType(currentObject);
				bool isLevelLine = currentObject is Level || currentObject is Line;
				bool willAdd = cell.LayerCount() < MaxLayerCount && (!isLevelLine || !cell.HasLevelPath());
				if (currentObject.ID == short.MaxValue) {
					Point location = cell.GetLocation(map.Width, map.Height);
					string xpos = location.X.ToString();
					string ypos = location.Y.ToString();
					string currentX = map.Info["general", "selectorX"];
					string currentY = map.Info["general", "selectorY"];

					if (xpos != currentX || ypos != currentY) {
						map.Info["general", "selectorX"] = xpos;
						map.Info["general", "selectorY"] = ypos;
					} else {
						map.Info["general", "selectorX"] = "-1";
						map.Info["general", "selectorY"] = "-1";
					}

					UpdateCurrentLevel(listLevels.SelectedItem);
				} else if (willAdd && (!hasObject || holdingControl) && isAdding.GetValueOrDefault(true)) {
					isAdding = true;
					Item item = currentObject.Copy();
					item.Position = cell.Position;
					cell.Objects.Add(item);
					cell.Objects.Sort();
					UpdateCurrentLevel(listLevels.SelectedItem);
				} else if (hasObject && !isAdding.GetValueOrDefault(false)) {
					isAdding = false;
					if (cell.RemoveObjectOfType(currentObject)) {
						UpdateCurrentLevel(listLevels.SelectedItem);
					}
				}
			} else if (e.Button == MouseButtons.Right) {
				Item item = cell.GetNextItem(currentObject);
				if (item != null) {
					if (item is Level) {
						statusAddLevel_ButtonClick(null, null);
					} else if (item is Line) {
						statusAddPath_ButtonClick(null, null);
					} else {
						for (int i = 0; i < listObjects.Items.Count; i++) {
							ListItem sprite = listObjects.Items[i];
							if (((Item)sprite.Value).ID == item.ID) {
								listObjects.SelectedIndex = i;
							}
						}
					}
					mapViewer.Invalidate();
				}
			} else if (e.Button == MouseButtons.Middle) {
				Item levelItem = cell.GetItemOfType(Level.DEFAULT);
				Item lineItem = cell.GetItemOfType(new Line());

				Palette palette = Reader.Palettes[map.Palette];
				if (levelItem != null) {
					using (LevelEdit editor = new LevelEdit()) {
						editor.Palette = palette;
						editor.Level = (Level)levelItem;
						editor.LevelList = listLevels;
						editor.BackColor = palette.Edge;
						editor.Icon = this.Icon;
						DialogResult result = editor.ShowDialog(this);
						if (result == DialogResult.OK) {
							UpdateCurrentLevel(listLevels.SelectedItem);
						}
					}
				} else if (lineItem != null) {
					using (PathEdit editor = new PathEdit()) {
						editor.Palette = palette;
						editor.Line = (Line)lineItem;
						editor.BackColor = palette.Edge;
						editor.Map = map;
						editor.Icon = this.Icon;
						DialogResult result = editor.ShowDialog(this);
						if (result == DialogResult.OK) {
							UpdateCurrentLevel(listLevels.SelectedItem);
						}
					}
				}
			}
		}
		private void mapViewer_CellMouseOver(Grid map, Cell cell, MouseEventArgs e) {
			if (cell == null) {
				statusPosition.Text = "[-1, -1]";
			} else {
				Point location = cell.GetLocation(map.Width, map.Height);
				statusPosition.Text = $"[{location.X}, {location.Y}]";
			}
			mapViewer.Invalidate();
		}
		private void mapViewer_CellMouseWheel(Grid map, Cell cell, MouseEventArgs e) {
			if (currentObject == null) { return; }

			if (holdingControl || cell.Objects.Count == 0) {
				ChangeItemDirection(currentObject, e.Delta < 0, false);
			} else {
				Item cellItem = cell.GetItemOfType(currentObject);
				if (cellItem == null) {
					cellItem = cell.GetNextItem(null);
				}
				if (cellItem != null) {
					ChangeItemDirection(cellItem, e.Delta < 0, true);
				}
			}
		}
		private void ChangeItemDirection(Item item, bool clockwise, bool updateMap) {
			if (clockwise) {
				item.Direction--;
				if (item.Direction > 3) {
					item.Direction = 3;
				}
			} else {
				item.Direction++;
				if (item.Direction > 3) {
					item.Direction = 0;
				}
			}
			if (updateMap) {
				UpdateCurrentLevel(listLevels.SelectedItem);
			}
		}
		private void mapViewer_DrawCurrentCellStart(Graphics g, Grid map, Cell cell, Rectangle bounds) {
			if (currentObject != null) {
				bool containsType = cell.ContainsObjectType(currentObject);
				bool isLevelLine = currentObject is Level || currentObject is Line;
				addedObject = (cell.LayerCount() < MaxLayerCount || currentObject.ID == short.MaxValue) && (!isLevelLine || !cell.HasLevelPath());
				if (addedObject && (!containsType || holdingControl) && isAdding.GetValueOrDefault(true)) {
					currentObject.Position = cell.Position;
					cell.Objects.Add(currentObject);
					cell.Objects.Sort();
				}
			}
		}
		private void mapViewer_DrawCurrentCellFinish(Graphics g, Grid map, Cell cell, Rectangle bounds) {
			if (currentObject != null) {
				if (addedObject) {
					cell.Objects.Remove(currentObject);
				} else {
					g.DrawImage(textX, bounds);
				}
			}
			g.DrawRectangle(Pens.Red, bounds);
		}
		private void menuItemSetTheme_DropDownOpening(object sender, EventArgs e) {
			string themePath = Path.Combine(GameDirectory, "Worlds", GameWorld, "Themes");
			if (!Directory.Exists(themePath)) { return; }

			ToolStripItemCollection collection = menuItemSetTheme.DropDownItems;
			for (int i = collection.Count - 1; i >= 3; i--) {
				ToolStripItem item = collection[i];
				item.Click -= menuItemSetTheme_Click;
				collection.RemoveAt(i);
			}

			string[] files = Directory.GetFiles(themePath, "*.txt", SearchOption.TopDirectoryOnly);
			for (int i = 0; i < files.Length; i++) {
				Info info = new Info(files[i]);
				string name = info["general", "name"];
				if (!string.IsNullOrEmpty(name)) {
					ToolStripMenuItem item = new ToolStripMenuItem(name, null, menuItemSetTheme_Click);
					item.Tag = info;
					collection.Add(item);
				}
			}
		}
		private void menuItemNewTheme_Click(object sender, EventArgs e) {
			if (map == null) { return; }

			using (AddDialog addTheme = new AddDialog()) {
				addTheme.Text = "Add Theme";
				addTheme.InputLabel = "Theme Name";
				addTheme.InputText = map.Name;
				addTheme.Icon = this.Icon;
				Palette palette = Reader.Palettes[map.Palette];
				addTheme.BackColor = palette.Edge;
				if (addTheme.ShowDialog(this) == DialogResult.OK) {
					string name = addTheme.InputText;
					if (!string.IsNullOrEmpty(name)) {
						string themePath = Path.Combine(GameDirectory, "Worlds", GameWorld, "Themes");
						string themeFile = Path.Combine(themePath, $"{name.Replace(" ", "")}.txt");

						ToolStripItemCollection collection = menuItemSetTheme.DropDownItems;
						for (int i = collection.Count - 1; i >= 3; i--) {
							ToolStripItem item = collection[i];
							Info info = (Info)item.Tag;
							if (info != null) {
								if (info["general", "name"].Equals(name, StringComparison.OrdinalIgnoreCase)) {
									themeFile = info.FilePath;
									break;
								}
							}
						}

						Directory.CreateDirectory(themePath);
						if (!File.Exists(themeFile) || MessageBox.Show(this, "Theme already exists. Do you want to overwrite it?", "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK) {
							string theme = map.SerializeChanges(name);
							File.WriteAllText(themeFile, theme);
						}
					}
				}
			}
		}
		private void menuItemSetTheme_Click(object sender, EventArgs e) {
			ToolStripMenuItem item = (ToolStripMenuItem)sender;
			if (map != null) {
				Info info = (Info)item.Tag;
				if (info == null) {
					map.Changes.Clear();
					map.ApplyChanges();
					SelectPalette("default.png");
				} else {
					Reader.ApplyChanges(map, info["tiles"]);
					string paletteName = info["general", "palette"];
					if (!string.IsNullOrEmpty(paletteName)) {
						SelectPalette(paletteName);
					}
				}

				AddSprites();
				UpdateCurrentLevel(listLevels.SelectedItem, true);
				UpdateStatusBar();
				listObjects.Focus();
			}
		}
		private void menuItemSaveWorld_Click(object sender, EventArgs e) {
			try {
				int saved = 0;
				int deleted = 0;
				ListItem currentLevel = listLevels.SelectedItem;
				for (int i = 0; i < levelsToBeRemoved.Count; i++) {
					ListItem levelItem = levelsToBeRemoved[i];
					Grid levelToRemove = (Grid)levelItem.Value;
					string filePath = Path.Combine(GameDirectory, "Worlds", GameWorld, $"{levelToRemove.FileName}.l");
					if (File.Exists(filePath)) {
						File.Delete(filePath);
					}
					filePath = Path.Combine(GameDirectory, "Worlds", GameWorld, $"{levelToRemove.FileName}.ld");
					if (File.Exists(filePath)) {
						File.Delete(filePath);
					}
					filePath = Path.Combine(GameDirectory, "Worlds", GameWorld, $"{levelToRemove.FileName}.png");
					if (File.Exists(filePath)) {
						File.Delete(filePath);
					}

					deleted++;
				}
				levelsToBeRemoved.Clear();

				for (int i = 0; i < listLevels.Items.Count; i++) {
					ListItem item = listLevels.Items[i];
					if (item.Changed) {
						Grid mapToSave = (Grid)item.Value;
						Writer.WriteMap(mapToSave, Path.Combine(GameDirectory, "Worlds", GameWorld));

						using (Bitmap imgExtra = new Bitmap(72, 72)) {
							using (Graphics g = Graphics.FromImage(imgExtra)) {
								Renderer.Render(mapToSave, g, imgExtra.Width, imgExtra.Height);
							}
							imgExtra.Save(Path.Combine(GameDirectory, "Worlds", GameWorld, $"{mapToSave.FileName}.png"));
						}

						item.Changed = false;
						saved++;
					}
				}

				listLevels.SelectedItem = currentLevel;
				if (listLevels.SelectedItem == null) {
					listLevels.SelectTopMostVisible();
				}
				listLevels.Invalidate();

				Text = $"{TitleBarText} - {GameWorldName} - {listLevels.Items.Count} Levels";
				if (saved > 0 && deleted == 0) {
					MessageBox.Show(this, $"Saved {saved} modified level(s) in world.", "Saved World", MessageBoxButtons.OK);
				} else if (saved == 0 && deleted > 0) {
					MessageBox.Show(this, $"Deleted {deleted} level(s) from world.", "Saved World", MessageBoxButtons.OK);
				} else if (saved > 0 && deleted > 0) {
					MessageBox.Show(this, $"Saved {saved} modified level(s) in world.\nDeleted {deleted} level(s) from world.", "Saved World", MessageBoxButtons.OK);
				}
			} catch (Exception ex) {
				MessageBox.Show(this, ex.ToString(), "Error Saving World", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		private void menuItemAddLevel_Click(object sender, EventArgs e) {
			using (AddDialog addLevel = new AddDialog()) {
				addLevel.Text = "Add Level";
				addLevel.InputLabel = "Level Name";
				addLevel.Icon = this.Icon;
				Palette palette = Reader.Palettes["default.png"];
				addLevel.BackColor = palette.Edge;
				if (addLevel.ShowDialog(this) == DialogResult.OK) {
					string name = addLevel.InputText;
					if (!string.IsNullOrEmpty(name)) {
						string dirName = Path.Combine(GameDirectory, "Worlds", GameWorld);

						int startID = listLevels.Items.Count;
						while (File.Exists(Path.Combine(dirName, $"{startID}level.l"))) {
							startID++;
						}
						Grid newMap = new Grid(Path.Combine(dirName, $"{startID}level.l"));
						newMap.Name = name;
						newMap.Resize(14, 8);
						Writer.WriteMap(newMap, dirName);

						int imgSize = listLevels.Width;
						float widthRatio = imgSize / newMap.Width;
						Bitmap img = new Bitmap(imgSize, (int)(widthRatio * newMap.Height) + listLevels.Font.Height * 2);
						using (Graphics g = Graphics.FromImage(img)) {
							Renderer.Render(newMap, g, img.Width, img.Height);
						}
						Bitmap imgExtra = new Bitmap(200, 150);
						using (Graphics g = Graphics.FromImage(imgExtra)) {
							Renderer.Render(newMap, g, imgExtra.Width, imgExtra.Height);
						}
						ListItem item = new ListItem(newMap, newMap.Name, img);
						item.Extra = imgExtra;
						listLevels.Items.Add(item);
						listLevels.Items.Sort();
						listLevels.SelectedItem = item;
						listLevels.EnsureSelectedInView();
					}
				}
			}
		}
		private void menuItemRemoveLevel_Click(object sender, EventArgs e) {
			if (listLevels.SelectedItem != null && MessageBox.Show(this, "Are you sure you want to remove this level? It will only get deleted when you save the world.", "Remove Level", MessageBoxButtons.OKCancel) == DialogResult.OK) {
				ListItem levelItem = listLevels.SelectedItem;
				levelsToBeRemoved.Add(levelItem);
				listLevels.Items.Remove(levelItem);
				listLevels.SelectTopMostVisible();
				Text = $"{TitleBarText} - {GameWorldName} - {listLevels.Items.Count} Levels";
			}
		}
		private void menuItemWorldProperties_Click(object sender, EventArgs e) {
			using (WorldProperties properties = new WorldProperties()) {
				properties.LevelList = listLevels;
				Palette palette;
				if (map == null) {
					palette = Reader.Palettes["default.png"];
				} else {
					palette = Reader.Palettes[map.Palette];
				}
				properties.WorldInfo = new Info(Path.Combine(GameDirectory, "Worlds", GameWorld, "world_data.txt"), false);
				properties.BackColor = palette.Edge;
				properties.Icon = this.Icon;
				DialogResult result = properties.ShowDialog(this);
				if (result == DialogResult.OK) {
					GameWorldName = properties.WorldInfo["general", "name"];
					Text = $"{TitleBarText} - {GameWorldName} - {listLevels.Items.Count} Levels";
					File.WriteAllText(properties.WorldInfo.FilePath, properties.WorldInfo.Serialize(true));
				}
			}
		}
		private void menuItemAddWorld_Click(object sender, EventArgs e) {
			using (AddDialog addWorld = new AddDialog()) {
				addWorld.Text = "Add World";
				addWorld.InputLabel = "World Name";
				addWorld.Icon = this.Icon;
				Palette palette = Reader.Palettes["default.png"];
				addWorld.BackColor = palette.Edge;
				if (addWorld.ShowDialog(this) == DialogResult.OK) {
					string name = addWorld.InputText;
					if (!string.IsNullOrEmpty(name)) {
						string dirName = Path.Combine(GameDirectory, "Worlds", name.Replace(" ", ""));
						if (Directory.Exists(dirName)) {
							MessageBox.Show(this, "This world already exists in the Worlds directory. Please open it instead.", "World Exists", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						} else {
							GameWorld = name.Replace(" ", "");
							Directory.CreateDirectory(dirName);
							Directory.CreateDirectory(Path.Combine(dirName, "Sprites"));
							Info worldInfo = new Info(Path.Combine(dirName, "world_data.txt"), false);
							worldInfo["general", "name"] = name;
							worldInfo["general", "start"] = "0level";
							worldInfo["general", "firstlevel"] = "0level";
							File.WriteAllText(worldInfo.FilePath, worldInfo.Serialize(true));
							Grid firstMap = new Grid(Path.Combine(dirName, "0level.l"));
							firstMap.Name = "Empty";
							firstMap.Resize(14, 8);
							Writer.WriteMap(firstMap, dirName);
							LoadWorld();
						}
					}
				}
			}
		}
		private void menuItemOpenWorld_Click(object sender, EventArgs e) {
			OpenFolderDialog browser = new OpenFolderDialog();
			browser.Title = "Open World";
			if (GameDirectory.IndexOf(@"Baba Is You\Data\", StringComparison.OrdinalIgnoreCase) > 0) {
				if (Directory.Exists(Path.Combine(GameDirectory, "Worlds"))) {
					browser.InitialDirectory = Path.Combine(GameDirectory, "Worlds");
				} else if (Directory.Exists(GameDirectory)) {
					browser.InitialDirectory = GameDirectory;
				}
			} else if (!string.IsNullOrEmpty(GameDirectory)) {
				browser.InitialDirectory = GameDirectory;
			} else {
				browser.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
			}

			if (browser.Show(this.Handle) && !string.IsNullOrEmpty(browser.FileName)) {
				string gameDir = browser.FileName;
				gameDir = gameDir.Replace('/', '\\');
				int index = gameDir.IndexOf(@"Baba Is You\Data\Worlds\", StringComparison.OrdinalIgnoreCase);
				if (index > 0) {
					GameDirectory = gameDir.Substring(0, index + 17);
					GameWorld = gameDir.Substring(index + 24);
					index = GameWorld.IndexOf('\\');
					if (index > 0) {
						GameWorld = GameWorld.Substring(0, index);
					}

					LoadWorld();
				} else {
					MessageBox.Show(this, "Invalid directory. Please verify and try again.", "Invalid World", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				}
			}
		}
		private void menuItemLevelProperties_Click(object sender, EventArgs e) {
			OpenLevelProperties(listLevels.SelectedItem);
		}
		private void OpenLevelProperties(ListItem levelItem) {
			if (levelItem == null) { return; }

			using (LevelProperties properties = new LevelProperties()) {
				properties.LevelList = listLevels;
				Grid mapToOpen = (Grid)levelItem.Value;
				properties.Map = mapToOpen;
				properties.BackColor = Reader.Palettes[mapToOpen.Palette].Edge;
				properties.Icon = this.Icon;
				DialogResult result = properties.ShowDialog(this);
				if (result == DialogResult.OK) {
					UpdateCurrentLevel(levelItem);
					UpdateStatusBar();
				}
			}
		}
		private void menuItemShowStacked_Click(object sender, EventArgs e) {
			mapViewer.ShowStacked = menuItemShowStacked.Checked;
			mapViewer.Invalidate();
		}
		private void menuItemShowDirections_Click(object sender, EventArgs e) {
			mapViewer.ShowDirections = menuItemShowDirections.Checked;
			mapViewer.Invalidate();
		}
		private void menuItemEdgePlacement_Click(object sender, EventArgs e) {
			mapViewer.AllowEdgePlacement = menuItemEdgePlacement.Checked;
			mapViewer.Invalidate();
		}
		private void menuItemRevertChanges_Click(object sender, EventArgs e) {
			if (map != null && MessageBox.Show(this, $"Are you sure you want to revert all changes to \"{map.Name}\" and reload it?", "Revert Changes", MessageBoxButtons.YesNo) == DialogResult.Yes) {
				map = Reader.ReadMap(Path.Combine(GameDirectory, "Worlds", GameWorld, $"{map.FileName}.l"));
				ListItem item = listLevels.SelectedItem;
				item.Value = map;
				UpdateCurrentLevel(item, false);
				listLevels_IndexChanged(listLevels.SelectedIndex, item);
			}
		}
		private void PaletteMenuItemClick(object sender, EventArgs e) {
			ToolStripMenuItem paletteMenu = (ToolStripMenuItem)sender;
			bool isChecked = paletteMenu.Checked;
			if (!isChecked) {
				paletteMenu.Checked = true;
				return;
			} else if (map == null) {
				paletteMenu.Checked = false;
				return;
			}

			SelectPalette(paletteMenu.Text.ToLower() + ".png");
			AddSprites();
			UpdateCurrentLevel(listLevels.SelectedItem);
		}
		private void SelectPalette(string paletteName) {
			for (int i = 0; i < menuPalette.DropDownItems.Count; i++) {
				ToolStripMenuItem menuItem = (ToolStripMenuItem)menuPalette.DropDownItems[i];
				menuItem.Checked = map != null && paletteName.IndexOf(menuItem.Text, StringComparison.OrdinalIgnoreCase) == 0;
			}

			Palette palette = Reader.Palettes[paletteName];
			if (map != null) {
				map.Palette = paletteName;
				map.Info["general", "paletteroot"] = palette.IsRoot ? "1" : "0";
			}
			BackColor = palette.Edge;
			listLevels.BackColor = palette.Edge;
		}
		private void menuItemReadMe_Click(object sender, EventArgs e) {
			using (ReadMe readme = new ReadMe()) {
				readme.Icon = this.Icon;
				readme.ShowDialog(this);
			}
		}
		private void menuItemWebsite_Click(object sender, EventArgs e) {
			Process.Start(@"https://github.com/ShootMe/BabaIsYouEditor/releases");
		}
	}
}