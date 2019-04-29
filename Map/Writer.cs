using BabaIsYou.Zip;
using System;
using System.IO;
using System.Text;
namespace BabaIsYou.Map {
	public class Writer {
		public const int MAIN = 0x4e49414d;
		public const int DATA = 0x41544144;
		public static void WriteMap(Grid map, string filePath) {
			using (FileStream stream = File.Open(Path.Combine(filePath, $"{map.FileName}.l"), FileMode.Create, FileAccess.ReadWrite)) {
				byte[] data = BitConverter.GetBytes(Reader.ACHTUNG);
				stream.Write(data, 0, data.Length);

				data = BitConverter.GetBytes((short)261);
				stream.Write(data, 0, data.Length);

				data = BitConverter.GetBytes(Reader.MAP);
				stream.Write(data, 0, data.Length);

				//Blocksize Default
				data = BitConverter.GetBytes(2);
				stream.Write(data, 0, data.Length);

				//MAP Property Count
				data = BitConverter.GetBytes((short)0);
				stream.Write(data, 0, data.Length);

				data = BitConverter.GetBytes(Reader.LAYR);
				stream.Write(data, 0, data.Length);

				//Blocksize Default
				data = BitConverter.GetBytes(2);
				stream.Write(data, 0, data.Length);

				short layerCount = (short)map.LayerCount();
				data = BitConverter.GetBytes(layerCount);
				stream.Write(data, 0, data.Length);

				for (int i = 0; i < layerCount; i++) {
					WriteLayer(stream, map, i);
				}
			}

			int levelCount = map.LevelCount();
			map.Info["general", "leveltype"] = levelCount > 0 ? "1" : "0";
			map.Info["general", "levels"] = levelCount.ToString();
			int pathCount = map.PathCount();
			map.Info["general", "paths"] = pathCount.ToString();
			map.UpdateLevelsAndPaths();

			using (FileStream stream = File.Open(Path.Combine(filePath, $"{map.FileName}.ld"), FileMode.Create, FileAccess.ReadWrite)) {
				string info = map.Info.Serialize();
				byte[] data = Encoding.UTF8.GetBytes(info);
				stream.Write(data, 0, data.Length);

				StringBuilder changed = new StringBuilder("[tiles]");
				changed.AppendLine().Append("changed=");
				StringBuilder sb = new StringBuilder();
				foreach (ItemChange change in map.Changes.Values) {
					changed.Append(change.ObjectName).Append(',');
					sb.Append(change.Serialize());
				}
				changed.AppendLine();
				data = Encoding.UTF8.GetBytes(changed.ToString());
				stream.Write(data, 0, data.Length);
				data = Encoding.UTF8.GetBytes(sb.ToString());
				stream.Write(data, 0, data.Length);
			}
		}
		private static void WriteLayer(FileStream stream, Grid grid, int layer) {
			byte[] data = BitConverter.GetBytes(grid.Width);
			stream.Write(data, 0, data.Length);

			data = BitConverter.GetBytes(grid.Height);
			stream.Write(data, 0, data.Length);

			//12x12 tile size default
			data = BitConverter.GetBytes(0xc000c);
			stream.Write(data, 0, data.Length);

			//Tileset = 0, Collision = 1
			data = BitConverter.GetBytes((short)0x100);
			stream.Write(data, 0, data.Length);

			//OffsetX = 0,OffsetY = 0
			data = BitConverter.GetBytes((long)0);
			stream.Write(data, 0, data.Length);

			//ScrollX = 1,ScrollY = 1
			data = BitConverter.GetBytes((float)1);
			stream.Write(data, 0, data.Length);
			stream.Write(data, 0, data.Length);

			//WrapX = 0,WrapY = 0
			data = BitConverter.GetBytes((short)0);
			stream.Write(data, 0, data.Length);

			//Visible = 1
			stream.WriteByte(1);

			//Opacity = 1
			data = BitConverter.GetBytes((float)1);
			stream.Write(data, 0, data.Length);

			//SubLayer Tileset = 255,Animation = 255,AnimationFrame = 255
			stream.WriteByte(0xff);
			stream.WriteByte(0xff);
			stream.WriteByte(0xff);

			//Data blocks
			stream.WriteByte(0x2);

			data = BitConverter.GetBytes(MAIN);
			stream.Write(data, 0, data.Length);

			int size = grid.Width * grid.Height;
			data = new byte[size * 2];
			byte[] directions = new byte[size];

			for (int i = 0, k = 0; i < size; i++) {
				Cell cell = grid.Cells[i];
				int itemCount = cell.Objects.Count;
				int objectCount = 0;
				bool wroteID = false;
				for (int j = 0; j < itemCount; j++) {
					Item item = cell.Objects[j];
					if (!(item is Line) && !(item is Level)) {
						if (objectCount < layer) {
							objectCount++;
						} else {
							data[k++] = (byte)item.ID;
							data[k++] = (byte)(item.ID >> 8);
							directions[i] = item.Direction;
							wroteID = true;
							break;
						}
					}
				}
				if (!wroteID) {
					data[k++] = 0xff;
					data[k++] = 0xff;
					directions[i] = 3;
				}
			}

			byte[] compressed;
			using (MemoryStream ms = new MemoryStream()) {
				using (DeflaterOutputStream zip = new DeflaterOutputStream(ms, new Deflater(Deflater.BEST_COMPRESSION))) {
					zip.Write(data, 0, data.Length);
				}
				compressed = ms.ToArray();
			}

			data = BitConverter.GetBytes(compressed.Length);
			stream.Write(data, 0, data.Length);
			stream.Write(compressed, 0, compressed.Length);

			data = BitConverter.GetBytes(DATA);
			stream.Write(data, 0, data.Length);

			//Cellsize
			stream.WriteByte(1);

			//Default value = 0
			data = BitConverter.GetBytes((int)0);
			stream.Write(data, 0, data.Length);

			using (MemoryStream ms = new MemoryStream()) {
				using (DeflaterOutputStream zip = new DeflaterOutputStream(ms, new Deflater(Deflater.BEST_COMPRESSION))) {
					zip.Write(directions, 0, directions.Length);
				}
				compressed = ms.ToArray();
			}

			data = BitConverter.GetBytes(compressed.Length);
			stream.Write(data, 0, data.Length);
			stream.Write(compressed, 0, compressed.Length);
		}
	}
}