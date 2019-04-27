using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
namespace BabaIsYou.Map {
	public enum LevelType {
		Normal = 0,
		Overworld = 1
	}
	public enum Particles {
		None,
		Bubbles,
		Clouds,
		Dust,
		Pollen,
		Smoke,
		Snow,
		Soot,
		Sparks
	}
	public enum Music {
		None,
		Baba,
		Burn,
		Cave,
		Ending,
		Factory,
		Float,
		Forest,
		Garden,
		Map,
		Mountain,
		Noise,
		Ruin,
		Stars
	}
	public class Info {
		public string FilePath { get; set; }
		private Dictionary<string, Dictionary<string, string>> data = new Dictionary<string, Dictionary<string, string>>(StringComparer.OrdinalIgnoreCase);

		public Info(string filePath, bool isMap = true) {
			FilePath = filePath;
			if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath)) {
				if (isMap) {
					this["general", "name"] = string.Empty;
					this["general", "subtitle"] = string.Empty;
					this["general", "palette"] = "default.png";
					this["general", "levels"] = "0";
					this["general", "selectorX"] = "-1";
					this["general", "selectorY"] = "-1";
					this["general", "paletteroot"] = "1";
					this["general", "paths"] = "0";
					this["general", "leveltype"] = ((int)LevelType.Normal).ToString();
					this["general", "unlockcount"] = "0";
					this["general", "customparent"] = string.Empty;
					this["general", "specials"] = "0";
				}
				return;
			}

			string info = File.ReadAllText(filePath);
			StringReader sr = new StringReader(info);
			string line = string.Empty;
			string section = string.Empty;
			Dictionary<string, string> dict = null;
			while ((line = sr.ReadLine()) != null) {
				line = line.Trim();
				if (line.IndexOf('[') == 0) {
					if (dict != null) {
						data.Add(section, dict);
					}
					dict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
					section = line.Substring(1, line.Length - 2);
				} else if (dict != null && !string.IsNullOrEmpty(line)) {
					int index = line.IndexOf('=');
					if (index > 0 && line.IndexOf("--") != 0) {
						string name = line.Substring(0, index).Trim();
						string value = line.Substring(index + 1).Trim();
						dict.Add(name, value);
					}
				}
			}

			if (dict != null) {
				data.Add(section, dict);
			}
		}

		public Dictionary<string, string> this[string section] {
			get {
				Dictionary<string, string> temp;
				if (data.TryGetValue(section, out temp)) {
					return temp;
				}
				return null;
			}
		}
		public string this[string section, string item] {
			get {
				Dictionary<string, string> tempSection;
				if (data.TryGetValue(section, out tempSection)) {
					string temp;
					if (tempSection.TryGetValue(item, out temp)) {
						return temp;
					}
				}
				return string.Empty;
			}
			set {
				Dictionary<string, string> tempSection;
				if (!data.TryGetValue(section, out tempSection)) {
					if (value == null) {
						return;
					}
					tempSection = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
					data.Add(section, tempSection);
				}

				if (tempSection.ContainsKey(item)) {
					if (value == null) {
						tempSection.Remove(item);
						if (tempSection.Count == 0) {
							data.Remove(section);
						}
					} else {
						tempSection[item] = value;
					}
				} else if (value != null) {
					tempSection.Add(item, value);
				}
			}
		}
		public string Serialize(bool asIs = false) {
			StringBuilder sb = new StringBuilder();
			if (asIs) {
				foreach (string section in data.Keys) {
					Serialize(sb, section);
				}
			} else {
				Serialize(sb, "general");
				Serialize(sb, "images");
				Serialize(sb, "levels");
				Serialize(sb, "paths");
				Serialize(sb, "specials");
				Serialize(sb, "icons");
			}
			return sb.ToString();
		}
		public void Serialize(StringBuilder sb, string section) {
			Dictionary<string, string> info = this[section];
			if (info != null) {
				sb.AppendLine($"[{section}]");
				List<string> keys = new List<string>();
				foreach (string key in info.Keys) {
					keys.Add(key);
				}
				keys.Sort(delegate (string one, string two) {
					int id1 = GetID(one);
					int id2 = GetID(two);
					return id1 > id2 ? 1 : id1 < id2 ? -1 : one.CompareTo(two);
				});
				for (int i = 0; i < keys.Count; i++) {
					string key = keys[i];
					sb.Append(key).Append('=').AppendLine(info[key]);
				}
			}
		}
		private int GetID(string key) {
			if (string.IsNullOrEmpty(key)) { return -1; }

			int id = char.IsDigit(key[0]) ? 0 : -1;
			int index = 0;
			while (index < key.Length && char.IsDigit(key[index])) {
				id = id * 10 + (key[index++] ^ 0x30);
			}
			return id;
		}
		public override string ToString() {
			StringBuilder sb = new StringBuilder();
			foreach (string key in data.Keys) {
				sb.Append(key).Append(',');
			}
			if (sb.Length > 0) {
				sb.Length--;
			}
			return sb.ToString();
		}
	}
}