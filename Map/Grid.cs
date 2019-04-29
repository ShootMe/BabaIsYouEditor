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
		public int LevelCount() {
			int count = 0;
			int size = Cells.Count;
			for (int i = 0; i < size; i++) {
				Cell cell = Cells[i];
				count += cell.LevelCount();
			}
			return count;
		}
		public int PathCount() {
			int count = 0;
			int size = Cells.Count;
			for (int i = 0; i < size; i++) {
				Cell cell = Cells[i];
				count += cell.PathCount();
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
		public void UpdateLevelsAndPaths() {
			Info.RemoveSection("Levels");
			Info.RemoveSection("Paths");
			Info.RemoveSection("Icons");

			int levelID = 0;
			int pathID = 0;
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
					} else if (item is Line line) {
						Info["Paths", $"{pathID}object"] = line.Object;
						Info["Paths", $"{pathID}style"] = line.Style.ToString();
						Info["Paths", $"{pathID}gate"] = line.Gate.ToString();
						Info["Paths", $"{pathID}requirement"] = line.Requirement.ToString();
						Info["Paths", $"{pathID}X"] = location.X.ToString();
						Info["Paths", $"{pathID}Y"] = location.Y.ToString();
						Info["Paths", $"{pathID}Z"] = "0";
						Info["Paths", $"{pathID}dir"] = line.Direction.ToString();
						pathID++;
					}
				}
			}
		}
		public string SerializeChanges(string themeName) {
			StringBuilder sb = new StringBuilder();
			sb.AppendLine("[general]");
			sb.AppendLine($"name={themeName}");
			sb.AppendLine($"palette={Palette}");
			sb.AppendLine($"paletteroot={Info["general", "paletteroot"]}");
			sb.AppendLine("[tiles]");
			sb.Append("changed=");
			StringBuilder tiles = new StringBuilder();
			foreach (ItemChange change in Changes.Values) {
				sb.Append(change.ObjectName).Append(',');
				tiles.Append(change.Serialize());
			}
			sb.AppendLine().Append(tiles.ToString());

			return sb.ToString();
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
			return $"{FileName} [{Width}, {Height}]";
		}
	}
}