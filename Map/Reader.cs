using BabaIsYou.Zip;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
namespace BabaIsYou.Map {
	public class Reader {
		public const long ACHTUNG = 0x21474e5554484341;
		public const int MAP = 0x2050414d;
		public const int LAYR = 0x5259414c;
		public static Dictionary<string, Item> DefaultsByName = new Dictionary<string, Item>(StringComparer.OrdinalIgnoreCase);
		public static Dictionary<short, Item> DefaultsByID = new Dictionary<short, Item>();
		public static Dictionary<string, Palette> Palettes = new Dictionary<string, Palette>(StringComparer.OrdinalIgnoreCase);
		public static Dictionary<string, Sprite> Sprites = new Dictionary<string, Sprite>();
		public static string DataPath = string.Empty;
		public static void Initialize(string path, string world) {
			DataPath = path;

			DefaultsByName.Clear();
			DefaultsByID.Clear();
			int maxID = 0;
			using (StreamReader reader = new StreamReader(Path.Combine(DataPath, "values.lua"))) {
				while (!reader.EndOfStream) {
					string line = reader.ReadLine();
					if (line.IndexOf("tileslist =", StringComparison.OrdinalIgnoreCase) == 0) {
						maxID = ReadObjects(reader);
					}
				}
			}
			string options = Path.Combine(DataPath, "Worlds", world, "Scripts", "options.lua");
			if (File.Exists(options)) {
				using (StreamReader reader = new StreamReader(options)) {
					ReadExtraObjects(reader, maxID);
				}
			}

			Palettes.Clear();
			string[] files = Directory.GetFiles(Path.Combine(DataPath, "Palettes"), "*.png", SearchOption.TopDirectoryOnly);
			for (int i = 0; i < files.Length; i++) {
				string file = files[i];
				Palettes.Add(Path.GetFileName(file), new Palette(file));
			}

			Sprites.Clear();
			files = Directory.GetFiles(Path.Combine(DataPath, "Sprites"), "*.png", SearchOption.TopDirectoryOnly);
			AddSprites(files);
			string imgPath = Path.Combine(DataPath, "Worlds", world, "Sprites");
			if (Directory.Exists(imgPath)) {
				files = Directory.GetFiles(imgPath, "*.png", SearchOption.TopDirectoryOnly);
				AddSprites(files, false);
			}
			imgPath = Path.Combine(DataPath, "Worlds", world, "Images");
			if (Directory.Exists(imgPath)) {
				files = Directory.GetFiles(imgPath, "*.png", SearchOption.TopDirectoryOnly);
				AddSprites(files, false, false);
			}
		}
		private static void AddSprites(string[] files, bool isRoot = true, bool isSprite = true) {
			for (int i = 0; i < files.Length; i++) {
				string file = files[i];
				int index;
				int sub;
				string fileNoExt = Path.GetFileNameWithoutExtension(file);
				string name = GetSpriteInfo(fileNoExt, isSprite, out index, out sub);
				if (string.IsNullOrEmpty(name)) {
					continue;
				}

				Sprite sprite;
				if (!Sprites.TryGetValue(name, out sprite)) {
					sprite = new Sprite(name, fileNoExt, isRoot);
					Sprites.Add(name, sprite);
				}

				sprite[index, sub] = (Bitmap)Bitmap.FromFile(file);
			}
		}
		private static string GetSpriteInfo(string name, bool isSprite, out int index, out int sub) {
			sub = name.LastIndexOf('_');
			index = 0;
			if (sub < 0) { return string.Empty; }

			index = name.LastIndexOf('_', sub - 1);
			if (index < 0) {
				index = sub;
			}

			string spriteName = (isSprite ? "" : "img_") + name.Substring(0, index);
			if (sub != index) {
				if (!int.TryParse(name.Substring(index + 1, sub - index - 1), out index)) {
					spriteName = (isSprite ? "" : "img_") + name.Substring(0, sub);
					index = 0;
				}
				if (!int.TryParse(name.Substring(sub + 1), out sub)) {
					return string.Empty;
				}
			} else {
				if (!int.TryParse(name.Substring(sub + 1), out sub)) {
					return string.Empty;
				}
				index = 0;
			}
			return spriteName;
		}
		private static int ReadObjects(StreamReader reader) {
			int maxID = 0;
			while (!reader.EndOfStream) {
				string title = reader.ReadLine().Trim();
				if (title == "}") {
					break;
				}

				int index = title.IndexOf('=');
				if (title.Length < 2 || index < 0) {
					continue;
				}

				Item item = new Item();
				title = title.Substring(0, index).Trim();
				if (title.IndexOf("object") == 0) {
					int temp;
					if (int.TryParse(title.Substring(6), out temp) && temp > maxID) {
						maxID = temp;
					}
				}
				item.Object = title;
				item.Name = title;

				while (!reader.EndOfStream) {
					string obj = reader.ReadLine().Trim();
					if (obj == "},") {
						break;
					}

					index = obj.IndexOf('=');
					if (index < 0) {
						continue;
					}

					string value = obj.Substring(index + 1, obj.Length - index - 2).Trim();
					obj = obj.Substring(0, index).Trim();
					SetItemValue(item, obj, value);
				}

				DefaultsByName.Add(title, item);
				DefaultsByID.Add(item.ID, item);
			}

			DefaultsByName.Add(Item.EMPTY.Name, Item.EMPTY);
			DefaultsByID.Add(Item.EMPTY.ID, Item.EMPTY);

			return maxID;
		}
		private static void ReadExtraObjects(StreamReader reader, int maxID) {
			while (!reader.EndOfStream) {
				string line = reader.ReadLine().Trim();
				if (line.IndexOf("mod.tile[\"") != 0) {
					continue;
				}

				maxID++;
				Item item = new Item();
				item.Object = $"object{maxID}";
				item.Name = item.Object;

				while (!reader.EndOfStream) {
					string obj = reader.ReadLine().Trim();
					if (obj == "}") {
						break;
					}

					int index = obj.IndexOf("=");
					if (index < 0) {
						continue;
					}

					string value = obj.Substring(index + 1, obj.Length - index - 2).Trim();
					obj = obj.Substring(0, index).Trim();
					SetItemValue(item, obj, value);
				}

				Item existing;
				if (DefaultsByID.TryGetValue(item.ID, out existing)) {
					item.Object = existing.Object;
					maxID--;
					DefaultsByID[item.ID] = item;
					DefaultsByName[item.Object] = item;
				} else {
					DefaultsByID.Add(item.ID, item);
					DefaultsByName.Add(item.Object, item);
				}
			}
		}
		private static void SetItemValue(Item item, string obj, string value) {
			switch (obj) {
				case "name": item.Name = value.Substring(1, value.Length - 2); break;
				case "sprite": item.Sprite = value.Substring(1, value.Length - 2); break;
				case "sprite_in_root": item.SpriteInRoot = ParseBool(value); break;
				case "unittype": item.IsObject = value == "\"object\""; break;
				case "type": item.Type = ParseByte(value); break;
				case "layer": item.Layer = ParseByte(value); break;
				case "colour": item.Color = CoordinateToShort(value); break;
				case "active": item.ActiveColor = CoordinateToShort(value); break;
				case "tiling": item.Tiling = ParseByte(value); break;
				case "tile": item.ID = CoordinateToShort(value); break;
			}
		}
		public static short CoordinateToShort(string coordinate) {
			int startIndex = 0;
			int endIndex = coordinate.Length;
			if (coordinate.IndexOf('{') == 0) {
				startIndex++;
				endIndex--;
			}
			int index = coordinate.IndexOf(',');
			if (index < 0) {
				return ParseShort(coordinate);
			}
			int x = ParseInt(coordinate.Substring(startIndex, index - startIndex));
			int y = ParseInt(coordinate.Substring(index + 1, endIndex - index - 1));
			return (short)((y << 8) | x);
		}
		public static string ShortToCoordinate(short value) {
			int x = (byte)value;
			int y = (byte)(value >> 8);
			return $"{x},{y}";
		}
		public static Grid ReadMap(string filePath) {
			Grid grid = new Grid(filePath);
			if (string.IsNullOrEmpty(grid.Name)) { return null; }

			byte[] data = new byte[8];
			using (FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read)) {
				stream.Read(data, 0, 8);
				long header = BitConverter.ToInt64(data, 0);
				if (header != ACHTUNG) {
					throw new Exception($"Invalid map file {grid.FileName}");
				}

				stream.Read(data, 0, 2);
				int version = BitConverter.ToInt16(data, 0);
				if (version < 256 || version > 261) {
					throw new Exception($"Invalid map version in {grid.FileName}");
				}

				while (stream.Position < stream.Length) {
					stream.Read(data, 0, 8);
					int blockHeader = BitConverter.ToInt32(data, 0);

					switch (blockHeader) {
						case MAP:
							stream.Read(data, 0, 2);
							break;
						case LAYR:
							stream.Read(data, 0, 2);
							int layerCount = BitConverter.ToInt16(data, 0);
							for (int i = 0; i < layerCount; i++) {
								ReadLayer(stream, grid, version);
							}
							break;
					}
				}
			}

			AddPaths(grid);
			ApplyChanges(grid);
			AddLevels(grid);
			AddImages(grid);

			return grid;
		}
		private static void ReadLayer(FileStream stream, Grid grid, int version) {
			byte[] data = new byte[25];

			stream.Read(data, 0, 4);
			grid.Width = BitConverter.ToInt32(data, 0);
			stream.Read(data, 0, 4);
			grid.Height = BitConverter.ToInt32(data, 0);

			if (version >= 258) {
				stream.Read(data, 0, 4);
			}
			stream.Read(data, 0, 25);

			if (version == 260) {
				stream.Read(data, 0, 2);
			} else if (version == 261) {
				stream.Read(data, 0, 3);
			}

			int size = grid.Width * grid.Height;
			if (grid.Cells.Count == 0) {
				for (int i = 0; i < size; i++) {
					Cell cell = new Cell((short)i);
					grid.Cells.Add(cell);
				}
			}

			int dataBlocks = stream.ReadByte();
			if (dataBlocks < 1 && dataBlocks > 2) {
				throw new Exception($"Invalid data block count in {grid.FileName}");
			}

			//MAIN
			stream.Read(data, 0, 8);
			int compressedSize = BitConverter.ToInt32(data, 4);
			int nextPosition = (int)stream.Position + compressedSize;
			InflaterInputStream zip = new InflaterInputStream(stream);
			byte[] decompressed = new byte[size * 2];
			int read = zip.Read(decompressed, 0, size * 2);
			if (read != size * 2) {
				throw new Exception($"Failed to read layer stream in {grid.FileName}");
			}
			stream.Position = nextPosition;

			List<Item> items = new List<Item>();
			for (int j = 0, k = 0; j < size; j++, k += 2) {
				Cell cell = grid.Cells[j];
				short id = BitConverter.ToInt16(decompressed, k);
				Item item;
				if (!DefaultsByID.TryGetValue(id, out item)) {
					throw new Exception($"Failed to find Object with ID {id} [{Reader.ShortToCoordinate(id)}] in {grid.FileName}");
				}
				item = item.Copy();
				item.Position = (short)j;
				items.Add(item);
				if (id != -1) {
					cell.Objects.Add(item);
				}
			}

			if (dataBlocks == 2) {
				//DATA
				stream.Read(data, 0, 13);
				compressedSize = BitConverter.ToInt32(data, 9);
				nextPosition = (int)stream.Position + compressedSize;
				zip = new InflaterInputStream(stream);
				read = zip.Read(decompressed, 0, size);
				//if (read != size) {
				//	throw new Exception("Failed to read layer stream");
				//}
				stream.Position = nextPosition;

				for (int j = 0; j < read; j++) {
					Item item = items[j];
					item.Direction = decompressed[j];
				}
			}
		}
		public static void ApplyChanges(Grid grid, Dictionary<string, string> changes = null) {
			if (changes == null) {
				changes = grid.Info["tiles"];
			}
			if (changes == null) { return; }

			Dictionary<short, ItemChange> itemChanges = grid.Changes;
			itemChanges.Clear();
			Dictionary<short, string> names = new Dictionary<short, string>();
			foreach (KeyValuePair<string, string> pair in changes) {
				if (pair.Key.IndexOf("object") == 0) {
					int index = pair.Key.IndexOf('_');
					string name = pair.Key.Substring(0, index);
					ItemChange item;
					short id = DefaultsByName[name].ID;
					if (!itemChanges.TryGetValue(id, out item)) {
						item = new ItemChange(name);
						itemChanges.Add(id, item);
					}

					string property = pair.Key.Substring(index + 1).ToLower();
					item[property] = pair.Value;

					if (property == "image" && !names.ContainsKey(id)) {
						names.Add(id, pair.Value);
					}
				}
			}

			foreach (KeyValuePair<short, string> pair in names) {
				if (!Sprites.ContainsKey(pair.Value)) {
					itemChanges.Remove(pair.Key);
				}
			}

			grid.ApplyChanges();
		}
		private static void AddPaths(Grid grid) {
			Dictionary<string, string> paths = grid.Info["paths"];
			if (paths == null) { return; }
			int pathCount = 0;
			int.TryParse(grid.Info["general", "paths"], out pathCount);

			Dictionary<byte, Line> lines = new Dictionary<byte, Line>();
			foreach (KeyValuePair<string, string> pair in paths) {
				byte id = 0;
				int index = 0;
				while (char.IsDigit(pair.Key[index])) {
					id = (byte)(id * 10 + (pair.Key[index++] ^ 0x30));
				}
				if (id >= pathCount) { continue; }

				string property = pair.Key.Substring(index).ToLower();

				Line line;
				if (!lines.TryGetValue(id, out line)) {
					line = new Line();
					lines.Add(id, line);
				}

				switch (property) {
					case "object": line.Object = pair.Value; break;
					case "x": line.X = ParseByte(pair.Value); break;
					case "y": line.Y = ParseByte(pair.Value); break;
					case "style": line.Style = ParseByte(pair.Value); break;
					case "gate": line.Gate = ParseByte(pair.Value); break;
					case "dir": line.Direction = ParseByte(pair.Value); break;
					case "requirement": line.Requirement = ParseByte(pair.Value); break;
				}
			}

			foreach (Line line in lines.Values) {
				line.UpdateLine();
				line.Position = (short)(line.Y * grid.Width + line.X);
				if (line.Position < 0 || line.Position >= grid.Cells.Count) { continue; }

				grid.Cells[line.Position].Objects.Add(line);
			}
		}
		private static void AddLevels(Grid grid) {
			Dictionary<string, string> levels = grid.Info["levels"];
			if (levels == null) { return; }
			int levelCount = 0;
			int.TryParse(grid.Info["general", "levels"], out levelCount);

			Dictionary<byte, Level> maps = new Dictionary<byte, Level>();
			foreach (KeyValuePair<string, string> pair in levels) {
				byte id = 0;
				int index = 0;
				while (char.IsDigit(pair.Key[index])) {
					id = (byte)(id * 10 + (pair.Key[index++] ^ 0x30));
				}
				if (id >= levelCount) { continue; }

				string property = pair.Key.Substring(index).ToLower();

				Level level;
				if (!maps.TryGetValue(id, out level)) {
					level = (Level)Level.DEFAULT.Copy();
					level.Position = id;
					maps.Add(id, level);
				}

				switch (property) {
					case "name": level.Name = pair.Value; break;
					case "file": level.File = pair.Value; break;
					case "colour": level.Color = CoordinateToShort(pair.Value); break;
					case "clearcolour": level.ActiveColor = CoordinateToShort(pair.Value); break;
					case "number": level.Number = ParseByte(pair.Value); break;
					case "style": level.Style = ParseByte(pair.Value); break;
					case "state": level.State = ParseByte(pair.Value); break;
					case "x": level.X = ParseByte(pair.Value); break;
					case "y": level.Y = ParseByte(pair.Value); break;
					case "dir": level.Direction = ParseByte(pair.Value); break;
				}
			}

			foreach (Level level in maps.Values) {
				if (level.Style == 255) {
					string icon = grid.Info["icons", $"{level.Number}file"];
					if (!string.IsNullOrEmpty(icon)) {
						int a, b;
						level.Sprite = GetSpriteInfo(icon, true, out a, out b);
					}
				}
				level.Position = (short)(level.Y * grid.Width + level.X);
				if (level.Position < 0 || level.Position >= grid.Cells.Count) { continue; }

				grid.Cells[level.Position].Objects.Add(level);
			}
		}
		private static bool ParseBool(string value) {
			bool temp;
			if (bool.TryParse(value, out temp)) {
				return temp;
			}
			return value != "0";
		}
		private static byte ParseByte(string value) {
			short temp;
			if (short.TryParse(value, out temp)) {
				return (byte)temp;
			}
			return 0;
		}
		private static short ParseShort(string value) {
			short temp;
			if (short.TryParse(value, out temp)) {
				return temp;
			}
			return 0;
		}
		private static int ParseInt(string value) {
			int temp;
			if (int.TryParse(value, out temp)) {
				return temp;
			}
			return 0;
		}
		private static void AddImages(Grid grid) {
			Dictionary<string, string> images = grid.Info["images"];
			if (images == null) { return; }
			int imageCount = 0;
			int.TryParse(grid.Info["images", "total"], out imageCount);

			foreach (KeyValuePair<string, string> pair in images) {
				if (pair.Key != "total") {
					grid.Images.Add("img_" + pair.Value);
				}
			}
		}
	}
}