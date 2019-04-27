using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
namespace BabaIsYou.Map {
	public class Palette {
		public string FileName;
		public Dictionary<short, Color> Colors = new Dictionary<short, Color>();
		public Color Default { get { return Colors[3 << 8]; } }
		public Color Background { get { return Colors[4 << 8]; } }
		public Color Edge { get { return Colors[1]; } }
		public bool IsRoot { get; }
		public Palette(string filePath) {
			FileName = Path.GetFileName(filePath);
			filePath.Replace("/", @"\");
			IsRoot = filePath.IndexOf(@"\Worlds\", StringComparison.OrdinalIgnoreCase) < 0;
			using (Bitmap colors = (Bitmap)Bitmap.FromFile(filePath)) {
				for (int i = 0; i < colors.Height; i++) {
					for (int j = 0; j < colors.Width; j++) {
						Colors.Add((short)((i << 8) | j), colors.GetPixel(j, i));
					}
				}
			}
		}

		public override string ToString() {
			return FileName;
		}
	}
}