﻿using BabaIsYou.Zip;
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
			using (StreamReader reader = new StreamReader(Path.Combine(path, "values.lua"))) {
				while (!reader.EndOfStream) {
					string line = reader.ReadLine();
					if (line.IndexOf("tileslist =", StringComparison.OrdinalIgnoreCase) == 0) {
						ReadObjects(reader);
					}
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
				string name = GetSpriteInfo(Path.GetFileNameWithoutExtension(file), isSprite, out index, out sub);
				if (string.IsNullOrEmpty(name)) {
					continue;
				}

				Sprite sprite;
				if (!Sprites.TryGetValue(name, out sprite)) {
					sprite = new Sprite(name, isRoot);
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
		private static void ReadObjects(StreamReader reader) {
			reader.ReadLine();
			while (!reader.EndOfStream) {
				string title = reader.ReadLine().Trim();
				if (title == "}," || title.Length <= 2) {
					break;
				}

				Item item = new Item();
				title = title.Substring(0, title.Length - 2);
				item.Object = title;
				item.Name = title;

				reader.ReadLine();
				while (!reader.EndOfStream) {
					string obj = reader.ReadLine().Trim();
					if (obj == "},") {
						break;
					}

					int index = obj.IndexOf("=");
					if (index < 0) {
						continue;
					}

					string value = obj.Substring(index + 1, obj.Length - index - 2).Trim();
					obj = obj.Substring(0, index).Trim();
					switch (obj) {
						case "name":
							item.Name = value.Substring(1, value.Length - 2);
							break;
						case "sprite":
							item.Sprite = value.Substring(1, value.Length - 2);
							break;
						case "sprite_in_root":
							item.SpriteInRoot = bool.Parse(value);
							break;
						case "unittype":
							item.IsObject = value == "\"object\"";
							break;
						case "type":
							item.Type = byte.Parse(value);
							break;
						case "layer":
							item.Layer = byte.Parse(value);
							break;
						case "colour":
							item.Color = CoordinateToShort(value);
							break;
						case "active":
							item.ActiveColor = CoordinateToShort(value);
							break;
						case "tiling":
							item.Tiling = (byte)short.Parse(value);
							break;
						case "tile":
							item.ID = CoordinateToShort(value);
							break;
					}
				}

				DefaultsByName.Add(title, item);
				DefaultsByID.Add(item.ID, item);
			}

			DefaultsByName.Add(Item.EMPTY.Name, Item.EMPTY);
			DefaultsByID.Add(Item.EMPTY.ID, Item.EMPTY);
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
				return short.Parse(coordinate);
			}
			int x = int.Parse(coordinate.Substring(startIndex, index - startIndex));
			int y = int.Parse(coordinate.Substring(index + 1, endIndex - index - 1));
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
					throw new Exception("Invalid map file");
				}

				stream.Read(data, 0, 2);
				int version = BitConverter.ToInt16(data, 0);
				if (version < 256 || version > 261) {
					throw new Exception("Invalid map version");
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
				throw new Exception("Invalid data block count");
			}

			//MAIN
			stream.Read(data, 0, 8);
			int compressedSize = BitConverter.ToInt32(data, 4);
			int nextPosition = (int)stream.Position + compressedSize;
			InflaterInputStream zip = new InflaterInputStream(stream);
			byte[] decompressed = new byte[size * 2];
			int read = zip.Read(decompressed, 0, size * 2);
			if (read != size * 2) {
				throw new Exception("Failed to read layer stream");
			}
			stream.Position = nextPosition;

			List<Item> items = new List<Item>();
			for (int j = 0, k = 0; j < size; j++, k += 2) {
				Cell cell = grid.Cells[j];
				short id = BitConverter.ToInt16(decompressed, k);
				Item item;
				if (!DefaultsByID.TryGetValue(id, out item)) {
					throw new Exception($"Failed to find Object with ID {id}");
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
					case "object":
						line.Object = pair.Value;
						break;
					case "x":
						line.X = byte.Parse(pair.Value);
						break;
					case "y":
						line.Y = byte.Parse(pair.Value);
						break;
					case "style":
						line.Style = byte.Parse(pair.Value);
						break;
					case "gate":
						line.Gate = byte.Parse(pair.Value);
						break;
					case "dir":
						line.Direction = byte.Parse(pair.Value);
						break;
					case "requirement":
						line.Requirement = byte.Parse(pair.Value);
						break;
				}
			}

			foreach (Line line in lines.Values) {
				Item item = DefaultsByName[line.Object];
				line.ID = item.ID;
				line.Sprite = item.Sprite;
				line.Name = item.Name;
				line.SpriteInRoot = item.SpriteInRoot;
				line.IsObject = item.IsObject;
				line.Type = item.Type;
				line.Color = item.Color;
				line.ActiveColor = item.ActiveColor;
				line.Tiling = item.Tiling;
				line.Layer = item.Layer;
				line.Position = (short)(line.Y * grid.Width + line.X);
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
					case "name":
						level.Name = pair.Value;
						break;
					case "file":
						level.File = pair.Value;
						break;
					case "colour":
						level.Color = CoordinateToShort(pair.Value);
						break;
					case "number":
						level.Number = byte.Parse(pair.Value);
						break;
					case "style":
						level.Style = (byte)short.Parse(pair.Value);
						break;
					case "x":
						level.X = byte.Parse(pair.Value);
						break;
					case "y":
						level.Y = byte.Parse(pair.Value);
						break;
					case "dir":
						level.Direction = byte.Parse(pair.Value);
						break;
				}
			}

			Dictionary<string, Level> usedLevel = new Dictionary<string, Level>();
			foreach (Level level in maps.Values) {
				Level current;
				if (usedLevel.TryGetValue(level.Name, out current)) {
					if (current.Position > level.Position) {
						usedLevel[level.Name] = level;
					}
				} else {
					usedLevel.Add(level.Name, level);
				}
			}

			foreach (Level level in usedLevel.Values) {
				if (level.Style == 255) {
					string icon = grid.Info["icons", $"{level.Number}file"];
					if (!string.IsNullOrEmpty(icon)) {
						int a, b;
						level.Sprite = GetSpriteInfo(icon, true, out a, out b);
					}
				}
				level.Position = (short)(level.Y * grid.Width + level.X);
				grid.Cells[level.Position].Objects.Add(level);
			}
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