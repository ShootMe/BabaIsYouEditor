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
		public static WorldViewer Instance;
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
			Instance = new WorldViewer();
			Application.Run(Instance);
#if !DEBUG
			} catch (Exception ex) {
				MessageBox.Show(ex.ToString(), "Application Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
#endif
		}

		private const int MaxLayerCount = 3;
		public static int LevelImageWidth = 160;
		public static int LevelImageHeight = 120;
		private Grid map;
		private Item currentObject;
		private List<ListItem> levelsToBeRemoved = new List<ListItem>();
		private bool addedObject;
		private bool hasChanged = false;
		private bool? isAdding = null;
		private bool holdingControl = false;
		private string GameWorldName;
		private int lastLevelWidth = 14;
		private int lastLevelHeight = 8;
		private ListItem parentLevel;
		private int maxSpriteRowSize = 20;
		private int maxSpriteRowCount = 5;
		private bool drawLevelRules = false;

		public WorldViewer() {
			InitializeComponent();
			Renderer.SetFonts(this);
			Renderer.SetFonts(listRules, 10);

			Text = TitleBarText;
			using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("BabaIsYou.Images.baba.ico")) {
				Icon = new Icon(stream);
			}

			statusLevel.Text = "Load world from the menu";
			statusLevelInfo.Text = "N/A";
			statusLevelName.Text = "N/A";
			statusPosition.Text = "[-1, -1]";
			statusSprite.Text = "N/A";

			menuItemShowDirections.Checked = RegistryRead<int>("ShowDirections", 0) != 0;
			menuItemShowStacked.Checked = RegistryRead<int>("ShowStacked", 0) != 0;
			menuItemShowAnimations.Checked = RegistryRead<int>("ShowAnimations", 1) != 0;
			listLevels.SortByText = RegistryRead<int>("SortByFile", 0) == 0;
			menuItemSortLevels.Checked = !listLevels.SortByText;
			menuItemSortObjectsByID.Checked = RegistryRead<int>("SortObjectsByID", 0) != 0;

			mapViewer.ShowStacked = menuItemShowStacked.Checked;
			mapViewer.ShowDirections = menuItemShowDirections.Checked;
			mapViewer.ShowAnimations = menuItemShowAnimations.Checked;
			txtLevelFilter.Visible = false;
			menuLevel.Enabled = false;
			menuPalette.Enabled = false;
			menuItemAddWorld.Enabled = false;
			menuItemWorldProperties.Enabled = false;
			menuItemAddLevel.Enabled = false;
			menuItemRemoveLevel.Enabled = false;
			menuItemSaveWorld.Enabled = false;
			menuItemSortLevels.Enabled = false;

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
			listLevels.ClearItems();
			listObjects.ClearItems();

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
					if (map == null || map.Width <= 0 || map.Height <= 0) { continue; }

					float widthRatio = imgSize / map.Width;
					int height = (int)(widthRatio * map.Height) + listLevels.Font.Height * 2;
					ListItem item = new ListItem(map, map.Name, imgSize, height);
					newItems.Add(item);
				}

				this.Invoke((MethodInvoker)delegate () {
					txtLevelFilter.Visible = true;

					statusLevel.Text = "N/A";
					listLevels.AddItems(newItems);

					Text = $"{TitleBarText} - {GameWorldName} - {listLevels.Count} Levels";

					menuPalette.DropDownItems.Clear();
					foreach (string name in Reader.Palettes.Keys) {
						string paletteName = $"{char.ToUpper(name[0])}{name.Substring(1, name.Length - 5)}";
						ToolStripMenuItem menuItem = new ToolStripMenuItem(paletteName, null, PaletteMenuItemClick);
						menuItem.CheckOnClick = true;
						menuItem.DisplayStyle = ToolStripItemDisplayStyle.Text;
						menuPalette.DropDownItems.Add(menuItem);
					}

					listLevels.SortItems();
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
					menuItemAddWorld.Enabled = true;
					menuItemRemoveLevel.Enabled = true;
					menuItemSaveWorld.Enabled = true;
					menuItemSortLevels.Enabled = true;

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
			LevelImageWidth = Width / 6;
			LevelImageHeight = LevelImageWidth * 3 / 4;
			if (LevelImageHeight * 7 > Height) {
				LevelImageHeight = Height / 7;
				LevelImageWidth = LevelImageHeight * 4 / 3;
			}
		}
		protected override bool ProcessCmdKey(ref Message msg, Keys keyData) {
			WorldViewer_KeyDown(null, new KeyEventArgs(keyData));
			return base.ProcessCmdKey(ref msg, keyData);
		}
		private void WorldViewer_KeyDown(object sender, KeyEventArgs e) {
			holdingControl = e.Control;
			if (holdingControl) {
				mapViewer.Invalidate();
			}
			if (e.KeyCode == Keys.Space && map != null && mapViewer.CurrentCell != null) {
				mapViewer_CellMouseDown(map, mapViewer.CurrentCell, new MouseEventArgs(MouseButtons.Middle, 1, 0, 0, 0));
			} else if (e.KeyCode == Keys.E && e.Shift && map != null && mapViewer.CurrentCell != null) {
				Cell cell = mapViewer.CurrentCell;
				Special special = cell.GetExtraObject<Special>();
				Level level;
				if (special != null && special.Type == (byte)SpecialType.Level) {
					level = special.GetLevel();
				} else {
					level = cell.GetExtraObject<Level>();
				}

				if (level != null) {
					int size = listLevels.Count;
					for (int i = 0; i < size; i++) {
						ListItem item = listLevels[i];
						Grid grid = (Grid)item.Value;
						if (grid.FileName == level.File) {
							parentLevel = listLevels.SelectedItem;
							listLevels.SelectedItem = item;
							break;
						}
					}
				}
			} else if (e.KeyCode == Keys.R && e.Shift) {
				if (parentLevel == null && map != null) {
					int size = listLevels.Count;
					for (int i = 0; i < size; i++) {
						Grid grid = (Grid)listLevels[i].Value;
						int cells = grid.Cells.Count;
						for (int j = 0; j < cells; j++) {
							Cell cell = grid.Cells[j];
							Special special = cell.GetExtraObject<Special>();
							Level level;
							if (special != null && special.Type == (byte)SpecialType.Level) {
								level = special.GetLevel();
							} else {
								level = cell.GetExtraObject<Level>();
							}

							if (level != null && level.File == map.FileName) {
								parentLevel = listLevels[i];
								break;
							}
						}
					}
				}

				if (parentLevel != null) {
					listLevels.SelectedItem = parentLevel;
					parentLevel = null;
				}
			} else if (map != null && mapViewer.CurrentCell != null) {
				int direction = -1;
				switch (e.KeyCode) {
					case Keys.Right: direction = 0; break;
					case Keys.Up: direction = 1; break;
					case Keys.Left: direction = 2; break;
					case Keys.Down: direction = 3; break;
				}

				if (direction >= 0) {
					Cell cell = mapViewer.CurrentCell;
					if (map.ChangeItemDirection(cell, currentObject, holdingControl || cell.Objects.Count == 0, false, direction)) {
						UpdateCurrentLevel(listLevels.SelectedItem);
					} else {
						mapViewer.Invalidate();
					}
				}
			}

			if (map != null && e.Shift) {
				int direction = -1;
				switch (e.KeyCode) {
					case Keys.D: direction = 0; break;
					case Keys.W: direction = 1; break;
					case Keys.A: direction = 2; break;
					case Keys.S: direction = 3; break;
				}

				if (direction >= 0) {
					map.MoveObjects(direction);
					mapViewer.ClearCurrentCell();
					UpdateCurrentLevel(listLevels.SelectedItem);
				}
			}
		}
		private void WorldViewer_KeyUp(object sender, KeyEventArgs e) {
			if (holdingControl) {
				holdingControl = false;
				mapViewer.Invalidate();
			}
		}
		private void WorldViewer_FormClosing(object sender, FormClosingEventArgs e) {
			if (HasChanges() && MessageBox.Show(this, "This world has unsaved changes. Are you sure you want to exit?", "World Changes", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) != DialogResult.OK) {
				e.Cancel = true;
				return;
			}
			RegistryWrite<int>("ShowStacked", menuItemShowStacked.Checked ? 1 : 0);
			RegistryWrite<int>("ShowDirections", menuItemShowDirections.Checked ? 1 : 0);
			RegistryWrite<int>("SortByFile", menuItemSortLevels.Checked ? 1 : 0);
			RegistryWrite<int>("ShowAnimations", menuItemShowAnimations.Checked ? 1 : 0);
			RegistryWrite<int>("SortObjectsByID", menuItemSortObjectsByID.Checked ? 1 : 0);
		}
		private void WorldViewer_Deactivate(object sender, EventArgs e) {
			holdingControl = false;
		}
		private bool HasChanges() {
			if (levelsToBeRemoved.Count > 0) {
				return true;
			}

			int size = listLevels.Count;
			for (int i = 0; i < size; i++) {
				ListItem item = listLevels[i];
				if (item.Changed) {
					return true;
				}
			}
			return false;
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
				ListItem selected = listLevels.SelectedItem;
				string searchText = txtLevelFilter.Text;
				int size = listLevels.Count;
				for (int i = 0; i < size; i++) {
					ListItem item = listLevels[i];
					string text = item.Text;
					string value = item.Value == null ? string.Empty : item.Value.ToString();
					item.Visible = string.IsNullOrEmpty(searchText) || text.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0 || value.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0;
				}
				listLevels.SelectedItem = selected;
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
				map = (Grid)item.Value;
				lastLevelWidth = map.Width;
				lastLevelHeight = map.Height;

				if (mapViewer.Map == map) { return; }

				mapViewer.Map = map;
				SelectPalette(map.Palette);
			}

			UpdateRules();
			AddSprites();
			UpdateStatusBar();
			mapViewer.Invalidate();
			listObjects.Focus();
		}
		private void listLevels_Resize(object sender, EventArgs e) {
			int size = listLevels.Count;
			for (int i = 0; i < size; i++) {
				ListItem item = listLevels[i];
				Grid grid = (Grid)item.Value;
				item.Width = splitMain.SplitterDistance;
				float widthRatio = item.Width / grid.Width;
				item.Height = (int)(widthRatio * grid.Height) + listLevels.Font.Height * 2;
			}
		}
		private void listLevels_RenderItem(ListItem item, Graphics g) {
			Grid grid = (Grid)item.Value;
			Renderer.Render(grid, g, item.Width, item.Height);
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
			if (levelItem.Image != null) {
				levelItem.Image.Dispose();
			}
			levelItem.Image = null;
			Grid mapToUpdate = (Grid)levelItem.Value;
			levelItem.Text = mapToUpdate.Name;
			levelItem.Changed = changed;

			UpdateRules();
			listLevels.Invalidate();
			mapViewer.Invalidate();
		}
		private void AddSprites() {
			ListItem current = listObjects.SelectedItem;
			int currentID = current == null ? -100 : ((Item)current.Value).ID;
			current = null;
			listObjects.SelectedIndex = -1;
			listObjects.ClearItems();

			currentObject = null;
			string paletteName = map == null ? "default.png" : map.Palette;
			Palette palette = Reader.Palettes[paletteName];
			int spriteCount = 0;
			foreach (Item item in Reader.DefaultsByObject.Values) {
				if (item.ID <= 0 || string.IsNullOrEmpty(item.Sprite)) { continue; }

				spriteCount++;
			}

			int spriteSize = 1;
			foreach (Item item in Reader.DefaultsByObject.Values) {
				if (item.ID <= 0 || string.IsNullOrEmpty(item.Sprite)) { continue; }

				ItemChange change;
				Item copy = item.Copy();
				if (map != null && map.Changes.TryGetValue(item.ID, out change)) {
					change.Apply(copy);
				}

				string name = copy.Name;
				int index = name.IndexOf("text");
				if (index == 0) {
					name = name.Substring(5) + "_text";
				}
				if (menuItemSortObjectsByID.Checked) {
					name = $"{copy.Grid.ToString("00000")} {name}";
				}

				ListItem listItem = new ListItem(copy, name, spriteSize, spriteSize);
				listItem.Changed = copy.Changed;
				listItem.BackColor = palette.Background;
				listObjects.AddItem(listItem);

				if (copy.ID == currentID) {
					current = listItem;
				}
			}

			spriteSize = ResizeListObjects();
			int size = listObjects.Count;
			for (int i = 0; i < size; i++) {
				ListItem item = listObjects[i];
				item.Width = spriteSize;
				item.Height = spriteSize;
			}

			listObjects.SortItems();
			listObjects.BackColor = palette.Edge;
			listObjects.SelectedItem = current;
			if (listObjects.SelectedItem == null) {
				listObjects.SelectedIndex = 0;
			}
		}
		private int ResizeListObjects() {
			int spriteCount = listObjects.Count;
			if (spriteCount == 0) { return 24; }

			int objectWindowSize = splitObjectsLevel.Height / 4;
			splitObjectsLevel.SplitterDistance = objectWindowSize;
			Size size = Renderer.GetFillSizeForCount(spriteCount, splitObjectsLevel.Width, objectWindowSize);
			maxSpriteRowCount = size.Height;
			maxSpriteRowSize = size.Width;

			int spriteSize = objectWindowSize / maxSpriteRowCount;
			if (spriteSize * maxSpriteRowSize > splitObjectsLevel.Width) {
				spriteSize = splitObjectsLevel.Width / maxSpriteRowSize;
			}
			return spriteSize;
		}
		private void statusAddLevel_ButtonClick(object sender, EventArgs e) {
			if (map == null) { return; }

			listObjects.SelectedItem = null;

			Level level;
			if (mapViewer.CurrentCell == null || (level = mapViewer.CurrentCell.GetExtraObject<Level>()) == null) {
				level = new Level();
				level.Name = map.Name;
				level.File = map.FileName;
			} else {
				level = (Level)level.Copy();
			}
			UpdateCurrentObject(level, true);
		}
		private void statusAddPath_ButtonClick(object sender, EventArgs e) {
			if (map == null) { return; }

			listObjects.SelectedItem = null;

			LevelPath path;
			if (mapViewer.CurrentCell == null || (path = mapViewer.CurrentCell.GetExtraObject<LevelPath>()) == null) {
				path = new LevelPath();
				path.UpdatePath();
			} else {
				path = (LevelPath)path.Copy();
			}
			UpdateCurrentObject(path, true);
		}
		private void statusAddSpecial_ButtonClick(object sender, EventArgs e) {
			if (map == null) { return; }

			listObjects.SelectedItem = null;

			Special special;
			if (mapViewer.CurrentCell == null || (special = mapViewer.CurrentCell.GetExtraObject<Special>()) == null) {
				special = new Special();
				special.Object = Special.FLOWER;
				special.Type = (byte)SpecialType.Flower;
			} else {
				special = (Special)special.Copy();
			}
			UpdateCurrentObject(special, true);
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
				} else if (currentObject is LevelPath) {
					statusSprite.Text = $"Path Object";
				} else if (currentObject is Special) {
					statusSprite.Text = $"Special Object";
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
					editor.Edit = ((Item)item.Value).Copy();
					editor.Map = map;
					editor.BackColor = palette.Edge;
					editor.Icon = this.Icon;
					DialogResult result = editor.ShowDialog(this);
					if (result == DialogResult.OK) {
						UpdateSprite(item, editor.Edit, palette);
						UpdateCurrentLevel(listLevels.SelectedItem);
					}
				}
			}
		}
		private void listObjects_Resize(object sender, EventArgs e) {
			int spriteSize = ResizeListObjects();
			int size = listObjects.Count;
			if (size == 0 || listObjects[0].Width == spriteSize) { return; }

			for (int i = 0; i < size; i++) {
				ListItem item = listObjects[i];
				Item spriteItem = (Item)item.Value;
				item.Width = spriteSize;
				item.Height = spriteSize;
			}
		}
		private void listObjects_RenderItem(ListItem item, Graphics g) {
			Palette palette = Reader.Palettes[map == null ? "default.png" : map.Palette];
			Item spriteItem = (Item)item.Value;
			Renderer.DrawSprite(null, g, new Rectangle(0, 0, item.Width, item.Height), spriteItem, palette);
		}
		private void UpdateSprite(ListItem item, Item sprite, Palette palette) {
			Bitmap img = new Bitmap(item.Width, item.Height);
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
			if (hasChanged) {
				UpdateCurrentLevel(listLevels.SelectedItem);
				hasChanged = false;
			}
		}
		private void mapViewer_CellMouseDown(Grid map, Cell cell, MouseEventArgs e) {
			if (e.Button == MouseButtons.Left) {
				if (currentObject == null) { return; }

				bool hasObject = cell.ContainsObject(currentObject);
				bool isLevelPath = currentObject is Level || currentObject is LevelPath;
				bool willAdd = cell.LayerCount() < MaxLayerCount && (!isLevelPath || !cell.HasLevelPath()) && (!(currentObject is Special) || !cell.HasObject<Special>());
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

					hasChanged = true;
				} else if (willAdd && (!hasObject || holdingControl) && isAdding.GetValueOrDefault(true)) {
					isAdding = true;
					Item item = currentObject.Copy();
					item.Position = cell.Position;
					cell.Objects.Add(item);
					cell.Objects.Sort();
					hasChanged = true;
				} else if (hasObject && !isAdding.GetValueOrDefault(false)) {
					isAdding = false;
					if (cell.RemoveObjectOfType(currentObject)) {
						hasChanged = true;
					}
				}
			} else if (e.Button == MouseButtons.Right) {
				Item item = cell.GetNextObject(currentObject);
				if (holdingControl) {
					Item currentItem = cell.GetObject(currentObject);
					if (currentItem == null) { currentItem = item; }
					if (currentItem != null) {
						item = currentItem;
						if (item is Level || item is Special || item is LevelPath) {
							hasChanged = true;
							cell.Objects.Remove(item);
							listObjects.SelectedItem = null;
							UpdateCurrentObject(item, true);
							return;
						}
					}
				}

				if (item != null) {
					if (item is Level) {
						statusAddLevel_ButtonClick(null, null);
					} else if (item is LevelPath) {
						statusAddPath_ButtonClick(null, null);
					} else if (item is Special) {
						statusAddSpecial_ButtonClick(null, null);
					} else {
						int size = listObjects.Count;
						for (int i = 0; i < size; i++) {
							ListItem sprite = listObjects[i];
							if (((Item)sprite.Value).ID == item.ID) {
								listObjects.SelectedItem = sprite;
								if (holdingControl) {
									Item spriteItem = (Item)sprite.Value;
									hasChanged = true;
									cell.Objects.Remove(item);
									spriteItem.Direction = item.Direction;
								}
							}
						}
					}
					mapViewer.Invalidate();
				}
			} else if (e.Button == MouseButtons.Middle) {
				Item extraItem = cell.GetExtraObject();
				if (extraItem == null) { return; }

				Palette palette = Reader.Palettes[map.Palette];
				if (extraItem is Level levelItem) {
					using (LevelEdit editor = new LevelEdit()) {
						editor.Palette = palette;
						editor.Edit = levelItem;
						editor.LevelList = listLevels;
						editor.BackColor = palette.Edge;
						editor.Icon = this.Icon;
						DialogResult result = editor.ShowDialog(this);
						if (result == DialogResult.OK) {
							UpdateCurrentLevel(listLevels.SelectedItem);
						}
					}
				} else if (extraItem is LevelPath pathItem) {
					using (PathEdit editor = new PathEdit()) {
						editor.Palette = palette;
						editor.Edit = pathItem;
						editor.BackColor = palette.Edge;
						editor.Map = map;
						editor.Icon = this.Icon;
						DialogResult result = editor.ShowDialog(this);
						if (result == DialogResult.OK) {
							UpdateCurrentLevel(listLevels.SelectedItem);
						}
					}
				} else if (extraItem is Special specialItem) {
					if (specialItem.Type == (byte)SpecialType.Art || specialItem.Type == (byte)SpecialType.Unknown) { return; }

					using (SpecialEdit editor = new SpecialEdit()) {
						editor.Palette = palette;
						editor.LevelList = listLevels;
						editor.Edit = specialItem;
						editor.Map = map;
						editor.BackColor = palette.Edge;
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

			if (map.ChangeItemDirection(cell, currentObject, holdingControl || cell.Objects.Count == 0, e.Delta < 0)) {
				UpdateCurrentLevel(listLevels.SelectedItem);
			} else {
				mapViewer.Invalidate();
			}
		}
		private void mapViewer_DrawCurrentCellStart(Graphics g, Grid map, Cell cell, Rectangle bounds) {
			if (currentObject != null) {
				bool containsType = cell.ContainsObject(currentObject);
				bool isLevelPath = currentObject is Level || currentObject is LevelPath;
				addedObject = (cell.LayerCount() < MaxLayerCount || currentObject.ID == short.MaxValue) && (!isLevelPath || !cell.HasLevelPath()) && (!(currentObject is Special) || !cell.HasObject<Special>());
				if (addedObject && (!containsType || holdingControl) && isAdding.GetValueOrDefault(true)) {
					currentObject.Position = cell.Position;
					currentObject.Active = false;
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
					g.DrawImage(Renderer.Error, bounds);
				}
			}
			g.DrawRectangle(Pens.Red, bounds);
		}
		private void mapViewer_PaintFinished(Graphics g, Grid map) {
			if (drawLevelRules) {
				listRules.Size = mapViewer.Size;
				listRules.PaintList(g);
			}
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
				addTheme.OptionText = "Only Save Object Changes";
				addTheme.OptionSetting = false;
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
							string theme = map.SerializeChanges(name, !addTheme.OptionSetting);
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

				int size = listLevels.Count;
				for (int i = 0; i < size; i++) {
					ListItem item = listLevels[i];
					if (item.Changed) {
						Grid mapToSave = (Grid)item.Value;
						Writer.WriteMap(mapToSave, Path.Combine(GameDirectory, "Worlds", GameWorld));
						SaveThumbnail(mapToSave);

						item.Changed = false;
						saved++;
					}
				}

				listLevels.SelectedItem = currentLevel;
				if (listLevels.SelectedItem == null) {
					listLevels.SelectTopMostVisible();
					listLevels.Invalidate();
				}

				Text = $"{TitleBarText} - {GameWorldName} - {listLevels.Count} Levels";
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
		private void SaveThumbnail(Grid mapToSave) {
			using (Bitmap imgExtra = new Bitmap(72, 72)) {
				using (Graphics g = Graphics.FromImage(imgExtra)) {
					Renderer.Render(mapToSave, g, imgExtra.Width, imgExtra.Height);
				}
				imgExtra.Save(Path.Combine(GameDirectory, "Worlds", GameWorld, $"{mapToSave.FileName}.png"));
			}
		}
		private void menuItemAddLevel_Click(object sender, EventArgs e) {
			using (AddDialog addLevel = new AddDialog()) {
				addLevel.Text = "Add Level";
				addLevel.InputLabel = "Level Name";
				addLevel.OptionSetting = false;
				addLevel.OptionText = map != null ? "Make Copy Of Current Level" : string.Empty;
				addLevel.Icon = this.Icon;
				Palette palette = Reader.Palettes["default.png"];
				addLevel.BackColor = palette.Edge;
				if (addLevel.ShowDialog(this) == DialogResult.OK) {
					string name = addLevel.InputText;
					if (!string.IsNullOrEmpty(name)) {
						string dirName = Path.Combine(GameDirectory, "Worlds", GameWorld);

						int startID = listLevels.Count;
						while (File.Exists(Path.Combine(dirName, $"{startID:000}level.l"))) {
							startID++;
						}

						Grid newMap = null;
						string newMapFile = Path.Combine(dirName, $"{startID:000}level");
						if (addLevel.OptionSetting) {
							File.Delete($"{newMapFile}.l");
							File.Copy(Path.Combine(dirName, $"{map.FileName}.l"), $"{newMapFile}.l");
							File.Delete($"{newMapFile}.ld");
							File.Copy(Path.Combine(dirName, $"{map.FileName}.ld"), $"{newMapFile}.ld");
							newMap = Reader.ReadMap($"{newMapFile}.l");
						} else {
							newMap = new Grid($"{newMapFile}.l");
						}

						newMap.Name = name;
						if (!addLevel.OptionSetting) {
							newMap.Resize(lastLevelWidth, lastLevelHeight);
						}
						Writer.WriteMap(newMap, dirName);
						SaveThumbnail(newMap);

						int imgSize = listLevels.Width;
						float widthRatio = imgSize / newMap.Width;
						int height = (int)(widthRatio * newMap.Height) + listLevels.Font.Height * 2;
						ListItem item = new ListItem(newMap, newMap.Name, imgSize, height);
						listLevels.AddItem(item);
						listLevels.SortItems();
						listLevels.SelectedItem = item;
					}
				}
			}
		}
		private void menuItemRemoveLevel_Click(object sender, EventArgs e) {
			if (listLevels.SelectedItem != null && MessageBox.Show(this, "Are you sure you want to remove this level? It will only get deleted when you save the world.", "Remove Level", MessageBoxButtons.OKCancel) == DialogResult.OK) {
				ListItem levelItem = listLevels.SelectedItem;
				levelsToBeRemoved.Add(levelItem);
				listLevels.RemoveItem(levelItem);
				listLevels.ReselectCurrentIndex();
				Text = $"{TitleBarText} - {GameWorldName} - {listLevels.Count} Levels";
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
					Text = $"{TitleBarText} - {GameWorldName} - {listLevels.Count} Levels";
					File.WriteAllText(properties.WorldInfo.FilePath, properties.WorldInfo.Serialize(true));
				}
			}
		}
		private void menuItemAddWorld_Click(object sender, EventArgs e) {
			using (AddDialog addWorld = new AddDialog()) {
				addWorld.Text = "Add World";
				addWorld.InputLabel = "World Name";
				addWorld.Icon = this.Icon;
				Palette palette = null;
				if (Reader.Palettes.TryGetValue("default.png", out palette)) {
					addWorld.BackColor = palette.Edge;
				} else {
					addWorld.BackColor = Color.Black;
				}
				if (addWorld.ShowDialog(this) == DialogResult.OK) {
					string name = addWorld.InputText;
					if (!string.IsNullOrEmpty(name)) {
						string dirName = Path.Combine(GameDirectory, "Worlds", name);
						if (Directory.Exists(dirName)) {
							MessageBox.Show(this, "This world already exists in the Worlds directory. Please open it instead.", "World Exists", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						} else if (!Directory.Exists(GameDirectory)) {
							MessageBox.Show(this, "You need to open a world from the Baba directory first before you can add any new worlds.", "World Exists", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						} else {
							GameWorld = name;
							Directory.CreateDirectory(dirName);
							Directory.CreateDirectory(Path.Combine(dirName, "Sprites"));
							Info worldInfo = new Info(Path.Combine(dirName, "world_data.txt"), false);
							worldInfo["general", "name"] = name;
							worldInfo["general", "start"] = "000level";
							worldInfo["general", "firstlevel"] = "000level";
							File.WriteAllText(worldInfo.FilePath, worldInfo.Serialize(true));
							Grid firstMap = new Grid(Path.Combine(dirName, "000level.l"));
							firstMap.Name = "Empty";
							firstMap.Resize(lastLevelWidth, lastLevelHeight);
							Writer.WriteMap(firstMap, dirName);
							LoadWorld();
						}
					}
				}
			}
		}
		private void menuItemOpenWorld_Click(object sender, EventArgs e) {
			if (HasChanges() && MessageBox.Show(this, "This world has unsaved changes. Are you sure you want to load another one?", "World Changes", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) != DialogResult.OK) {
				return;
			}

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
		private void menuItemShowRules_Click(object sender, EventArgs e) {
			drawLevelRules = !drawLevelRules;
		}
		private void UpdateRules() {
			listRules.ClearItems();
			if (map != null) {
				map.MarkAllInactive();
				Parser parser = new Parser(map, false);
				List<Rule> rules = parser.Rules;
				for (int i = 0; i < rules.Count; i++) {
					Rule rule = rules[i];
					rule.MarkItemsActive();
					List<string> allRules = rule.AllRules();
					for (int j = 0; j < allRules.Count; j++) {
						string currentRule = allRules[j];
						Size textSize = TextRenderer.MeasureText(currentRule, listRules.Font, new Size(99999, 90), TextFormatFlags.NoPadding | TextFormatFlags.SingleLine);
						ListItem item = new ListItem(null, currentRule, textSize.Width + 5, textSize.Height + 5);
						listRules.AddItem(item);
					}
				}
				listRules.SortItems();
			}
			listRules.Size = mapViewer.Size;
		}
		private void menuItemShowAnimations_Click(object sender, EventArgs e) {
			mapViewer.ShowAnimations = menuItemShowAnimations.Checked;
			mapViewer.Invalidate();
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
		private void menuItemSortLevels_Click(object sender, EventArgs e) {
			listLevels.SortByText = !menuItemSortLevels.Checked;
			ListItem selected = listLevels.SelectedItem;
			listLevels.SortItems();
			listLevels.SelectedItem = selected;
		}
		private void menuItemSortObjectsByID_Click(object sender, EventArgs e) {
			AddSprites();
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