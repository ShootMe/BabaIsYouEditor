using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
namespace BabaIsYou.Map {
	public class Grid {
		public delegate void ResizedEvent(Grid map);
		public event ResizedEvent Resized;
		public Info Info;
		public string FileName;
		public Particles Effect;
		public Music Theme;
		public int Width;
		public int Height;
		public List<Cell> Cells;
		public Dictionary<short, ItemChange> Changes;
		public List<string> Images;
		public string Name {
			get { return Info["general", "name"]; }
			set { Info["general", "name"] = value; }
		}
		public string Palette {
			get { return Info["general", "palette"]; }
			set { Info["general", "palette"] = value; }
		}

		public Grid(string filePath) {
			Info = new Info($"{filePath}d");
			FileName = Path.GetFileNameWithoutExtension(filePath);
			Cells = new List<Cell>();
			Images = new List<string>();
			Changes = new Dictionary<short, ItemChange>();
			Width = 0;
			Height = 0;
			Effect = Particles.None;
			Theme = Music.None;
		}

		public void Resize(int width, int height) {
			Item edgeItem = Reader.DefaultsByID[0];
			List<Cell> newCells = new List<Cell>();
			int newSize = width * height;
			Cell cell = null;
			for (int i = 0; i < newSize; i++) {
				int x = i % width;
				int y = i / width;

				if (x == 0 || x == width - 1 || y == 0 || y == height - 1) {
					cell = new Cell((short)i);
					Item newEdge = edgeItem.Copy();
					newEdge.Position = (short)i;
					cell.Objects.Add(newEdge);
				} else if (x < Width && y < Height) {
					int oldPos = y * Width + x;
					cell = Cells[oldPos];

					cell.Position = (short)i;
					for (int j = cell.Objects.Count - 1; j >= 0; j--) {
						Item item = cell.Objects[j];
						item.Position = (short)i;
						if (item.ID == 0) {
							cell.Objects.RemoveAt(j);
						}
					}
				} else {
					cell = new Cell((short)i);
				}

				newCells.Add(cell);
			}
			Width = width;
			Height = height;
			Cells = newCells;
			Resized?.Invoke(this);
		}
		public int CountOfType<T>() {
			int count = 0;
			int size = Cells.Count;
			for (int i = 0; i < size; i++) {
				Cell cell = Cells[i];
				count += cell.CountOfType<T>();
			}
			return count;
		}
		public int LayerCount() {
			int layerCount = 1;
			int size = Cells.Count;
			for (int i = 0; i < size; i++) {
				Cell cell = Cells[i];
				int objectCount = cell.LayerCount();
				if (objectCount > layerCount) {
					layerCount = objectCount;
				}
			}
			return layerCount;
		}
		public void MoveObjects(int direction) {
			int moveX = 0;
			int moveY = 0;
			switch (direction) {
				case 0: moveX = 1; break;
				case 1: moveY = -1; break;
				case 2: moveX = -1; break;
				case 3: moveY = 1; break;
			}
			if (moveX == 0 && moveY == 0) { return; }

			int selectorX = Reader.ParseInt(Info["general", "selectorX"], -1);
			int selectorY = Reader.ParseInt(Info["general", "selectorY"], -1);
			if (selectorX >= 0 && selectorY >= 0) {
				Info["general", "selectorX"] = WrapValue(selectorX + moveX, Width).ToString();
				Info["general", "selectorY"] = WrapValue(selectorY + moveY, Height).ToString();
			}

			int x = 0;
			int y = 0;
			int size = Cells.Count;
			List<Cell> newCells = new List<Cell>(size);
			for (int i = 0; i < size; i++) {
				newCells.Add(null);
			}
			for (int i = 0; i < size; i++) {
				Cell cell = Cells[i];

				if (x > 0 && x + 1 < Width && y > 0 && y + 1 < Height) {
					int newX = WrapValue(x + moveX, Width);
					int newY = WrapValue(y + moveY, Height);
					int newPos = newY * Width + newX;
					newCells[newPos] = cell;
				} else {
					newCells[i] = cell;
				}

				x++;
				if (x >= Width) {
					x = 0;
					y++;
				}
			}
			for (int i = 0; i < size; i++) {
				Cell cell = newCells[i];
				cell.Position = (short)i;
				int objects = cell.Objects.Count;
				for (int j = 0; j < objects; j++) {
					Item item = cell.Objects[j];
					item.Position = (short)i;
				}
				Cells[i] = cell;
			}
		}
		private int WrapValue(int value, int max) {
			if (value + 1 >= max) {
				value = 1;
			} else if (value <= 0) {
				value = max - 2;
			}
			return value;
		}
		public bool ChangeItemDirection(Cell cell, Item item, bool changeCurrent, bool clockwise, int specific = -1) {
			if (changeCurrent) {
				ChangeItemDirection(item, clockwise, specific);
			} else {
				Item cellItem = cell.GetObject(item);
				if (cellItem == null) {
					cellItem = cell.GetNextObject(null);
				}
				if (cellItem != null) {
					ChangeItemDirection(cellItem, clockwise, specific);
					return true;
				}
			}
			return false;
		}
		private void ChangeItemDirection(Item item, bool clockwise, int specific) {
			if (specific >= 0) {
				item.Direction = (byte)specific;
			} else if (clockwise) {
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
		}
		public void UpdateExtraObjects() {
			Info.RemoveSection("Levels");
			Info.RemoveSection("Paths");
			Info.RemoveSection("Icons");
			Info.RemoveSection("Specials");

			int levelID = 0;
			int pathID = 0;
			int specialID = 0;
			int iconID = 0;
			int size = Cells.Count;
			for (int i = 0; i < size; i++) {
				Cell cell = Cells[i];
				Point location = cell.GetLocation(Width, Height);

				for (int j = cell.Objects.Count - 1; j >= 0; j--) {
					Item item = cell.Objects[j];
					if (item is Level level) {
						Info["Levels", $"{levelID}name"] = level.Name;
						Info["Levels", $"{levelID}file"] = level.File;
						Info["Levels", $"{levelID}number"] = level.Style == (byte)LevelStyle.Icon ? iconID.ToString() : level.Number.ToString();
						Info["Levels", $"{levelID}style"] = level.Style == (byte)LevelStyle.Icon ? "-1" : level.Style.ToString();
						Info["Levels", $"{levelID}state"] = level.State.ToString();
						Info["Levels", $"{levelID}colour"] = Reader.ShortToCoordinate(level.Color);
						Info["Levels", $"{levelID}clearcolour"] = Reader.ShortToCoordinate(level.ActiveColor);
						Info["Levels", $"{levelID}X"] = location.X.ToString();
						Info["Levels", $"{levelID}Y"] = location.Y.ToString();
						Info["Levels", $"{levelID}Z"] = "0";
						Info["Levels", $"{levelID}dir"] = level.Direction.ToString();
						if (level.Style == (byte)LevelStyle.Icon) {
							Sprite sprite = Reader.Sprites[level.Sprite];
							Info["Icons", $"{iconID}file"] = sprite.ActualFile;
							Info["Icons", $"{iconID}root"] = sprite.IsRoot ? "1" : "0";
							iconID++;
						}
						levelID++;
					} else if (item is LevelPath path) {
						Info["Paths", $"{pathID}object"] = path.Object;
						Info["Paths", $"{pathID}style"] = path.Style.ToString();
						Info["Paths", $"{pathID}gate"] = path.Gate.ToString();
						Info["Paths", $"{pathID}requirement"] = path.Requirement.ToString();
						Info["Paths", $"{pathID}X"] = location.X.ToString();
						Info["Paths", $"{pathID}Y"] = location.Y.ToString();
						Info["Paths", $"{pathID}Z"] = "0";
						Info["Paths", $"{pathID}dir"] = path.Direction.ToString();
						pathID++;
					} else if (item is Special special) {
						Info["Specials", $"{specialID}data"] = special.Object;
						Info["Specials", $"{specialID}X"] = location.X.ToString();
						Info["Specials", $"{specialID}Y"] = location.Y.ToString();
						Info["Specials", $"{specialID}Z"] = "0";
						specialID++;
					}
				}
			}
		}
		public string SerializeChanges(string themeName, bool includePalette) {
			StringBuilder sb = new StringBuilder();
			sb.AppendLine("[general]");
			sb.AppendLine($"name={themeName}");
			if (includePalette) {
				sb.AppendLine($"palette={Palette}");
				sb.AppendLine($"paletteroot={Info["general", "paletteroot"]}");
			}
			sb.AppendLine("[tiles]");
			sb.Append("changed=");
			StringBuilder tiles = new StringBuilder();
			foreach (ItemChange change in Changes.Values) {
				string objectChange = change.Serialize();
				if (!string.IsNullOrEmpty(objectChange)) {
					sb.Append(change.ObjectName).Append(',');
					tiles.Append(objectChange);
				}
			}
			sb.AppendLine().Append(tiles.ToString());

			return sb.ToString();
		}
		public bool UpdateChanges(Item item) {
			Item defaultItem = Reader.DefaultsByID[item.ID];
			bool hasChanges = true;
			hasChanges = UpdateChanges(item.ID, "activecolour", defaultItem.ActiveColor != item.ActiveColor ? Reader.ShortToCoordinate(item.ActiveColor) : null);
			hasChanges = UpdateChanges(item.ID, "colour", defaultItem.Color != item.Color ? Reader.ShortToCoordinate(item.Color) : null);
			hasChanges = UpdateChanges(item.ID, "layer", defaultItem.Layer != item.Layer ? item.Layer.ToString() : null);
			hasChanges = UpdateChanges(item.ID, "type", defaultItem.Type != item.Type ? item.Type.ToString() : null);
			hasChanges = UpdateChanges(item.ID, "tiling", defaultItem.Tiling != item.Tiling ? (item.Tiling == 255 ? -1 : (int)item.Tiling).ToString() : null);
			hasChanges = UpdateChanges(item.ID, "name", defaultItem.Name != item.Name ? item.Name : null);
			hasChanges = UpdateChanges(item.ID, "image", defaultItem.Sprite != item.Sprite ? item.Sprite : null);
			hasChanges = UpdateChanges(item.ID, "unittype", defaultItem.IsObject != item.IsObject ? item.IsObject ? "object" : "text" : null);
			hasChanges = UpdateChanges(item.ID, "root", defaultItem.SpriteInRoot != item.SpriteInRoot ? item.SpriteInRoot ? "1" : "0" : null);
			hasChanges = UpdateChanges(item.ID, "operatortype", defaultItem.OperatorType != item.OperatorType ? item.OperatorType : null);
			hasChanges = UpdateChanges(item.ID, "argextra", defaultItem.ArgExtra != item.ArgExtra ? item.ArgExtra : null);
			return hasChanges;
		}
		public bool UpdateChanges(short id, string property, string value) {
			ItemChange temp;
			if (Changes.TryGetValue(id, out temp)) {
				temp[property] = value;
				if (!temp.HasChanges) {
					Changes.Remove(id);
				} else {
					return true;
				}
			} else if (!string.IsNullOrEmpty(value)) {
				string name = Reader.DefaultsByID[id].Object;
				temp = new ItemChange(name);
				Changes.Add(id, temp);
				temp[property] = value;
				return true;
			}
			return false;
		}
		public void ApplyChanges() {
			int size = Cells.Count;
			for (int i = 0; i < size; i++) {
				Cell cell = Cells[i];
				int itemCount = cell.Objects.Count;
				for (int j = 0; j < itemCount; j++) {
					Item item = cell.Objects[j];
					ItemChange change;
					if (Changes.TryGetValue(item.ID, out change)) {
						change.Apply(item);
					} else if (item.Changed) {
						Item copy = Reader.DefaultsByID[item.ID].Copy();
						copy.Position = item.Position;
						copy.Direction = item.Direction;
						cell.Objects[j] = copy;
					}
				}
			}
		}
		public override string ToString() {
			return $"{FileName}";
		}
	}
}