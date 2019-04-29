using System.Collections.Generic;
using System.Drawing;
namespace BabaIsYou.Map {
	public class Sprite {
		public string Name;
		public string ActualFile;
		public bool IsRoot;
		public Dictionary<int, Bitmap> Images = new Dictionary<int, Bitmap>();

		public Sprite(string name, string actualFile, bool isRoot) {
			Name = name;
			ActualFile = actualFile;
			IsRoot = isRoot;
		}
		public int MaxIndex {
			get {
				int max = 0;
				foreach (int key in Images.Keys) {
					int id = (key - 1) / 3;
					if (id > max) {
						max = id;
					}
				}
				return max;
			}
		}
		public Bitmap this[int index, int sub] {
			get {
				Bitmap img;
				if (Images.TryGetValue(index * 3 + sub, out img)) {
					return img;
				}
				return null;
			}
			set {
				int key = index * 3 + sub;
				if (Images.ContainsKey(key)) {
					Images[key] = value;
				} else {
					Images.Add(key, value);
				}
			}
		}
		public override string ToString() {
			return $"{Name} [{Images.Count}]";
		}
	}
}