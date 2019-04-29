using System;
using System.Collections.Generic;
using System.Text;
namespace BabaIsYou.Map {
	public class ItemChange {
		public string ObjectName;
		private Dictionary<string, string> changes = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
		public bool HasChanges {
			get { return changes.Count > 0; }
		}

		public ItemChange(string objectName) {
			ObjectName = objectName;
		}
		public void Apply(Item item) {
			item.Changed = changes.Count > 0;

			foreach (KeyValuePair<string, string> pair in changes) {
				switch (pair.Key) {
					case "root": item.SpriteInRoot = pair.Value != "0"; break;
					case "image": item.Sprite = pair.Value; break;
					case "name": item.Name = pair.Value; break;
					case "colour": item.Color = Reader.CoordinateToShort(pair.Value); break;
					case "activecolour": item.ActiveColor = Reader.CoordinateToShort(pair.Value); break;
					case "type": item.Type = byte.Parse(pair.Value); break;
					case "unittype": item.IsObject = pair.Value == "object"; break;
					case "layer": item.Layer = byte.Parse(pair.Value); break;
					case "tiling": item.Tiling = (byte)short.Parse(pair.Value); break;
				}
			}
		}
		public string this[string property] {
			get {
				string temp;
				if (changes.TryGetValue(property, out temp)) {
					return temp;
				}
				return null;
			}
			set {
				if (changes.ContainsKey(property)) {
					if (string.IsNullOrEmpty(value)) {
						changes.Remove(property);
					} else {
						changes[property] = value;
					}
				} else if (!string.IsNullOrEmpty(value)) {
					changes.Add(property, value);
				}
			}
		}
		public string Serialize() {
			List<string> data = new List<string>();
			Item item = Reader.DefaultsByName[ObjectName];
			foreach (KeyValuePair<string, string> pair in changes) {
				bool changed = false;
				switch (pair.Key) {
					case "root": changed = item.SpriteInRoot != (pair.Value != "0"); break;
					case "image": changed = !item.Sprite.Equals(pair.Value, StringComparison.OrdinalIgnoreCase); break;
					case "name": changed = !item.Name.Equals(pair.Value, StringComparison.OrdinalIgnoreCase); break;
					case "colour": changed = item.Color != Reader.CoordinateToShort(pair.Value); break;
					case "activecolour": changed = item.ActiveColor != Reader.CoordinateToShort(pair.Value); break;
					case "type": changed = item.Type.ToString() != pair.Value; break;
					case "unittype": changed = item.IsObject != (pair.Value == "object"); break;
					case "layer": changed = item.Layer.ToString() != pair.Value; break;
					case "tiling": changed = item.Tiling.ToString() != pair.Value; break;
				}

				if (changed) {
					data.Add($"{ObjectName}_{pair.Key}={pair.Value}");
				}
			}

			data.Sort();
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < data.Count; i++) {
				sb.AppendLine(data[i]);
			}
			return sb.ToString();
		}
		public override string ToString() {
			StringBuilder sb = new StringBuilder();
			foreach (string key in changes.Keys) {
				sb.Append(key).Append(',');
			}
			if (sb.Length > 0) {
				sb.Length--;
			}
			return sb.ToString();
		}
	}
}