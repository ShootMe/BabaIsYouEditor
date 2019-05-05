using System;
using System.Collections.Generic;
using System.Text;
namespace BabaIsYou.Map {
	public class Rule {
		public Target Lonely;
		public List<Target> Targets = new List<Target>();
		public List<ExtraRule> Extras = new List<ExtraRule>();
		public List<ExtraRule> Actions = new List<ExtraRule>();

		public bool ApplysTo(Item item) {
			bool applies = false;
			foreach (Target target in Targets) {
				string targetItem = target.Item.Name.Replace("text_", "");
				string itemName = string.Empty;
				Type itemType = item.GetType();
				if (itemType == typeof(Item)) {
					itemName = item.Name;
				} else if (itemType == typeof(Level)) {
					itemName = "level";
				}
				bool isItem = targetItem == itemName || (targetItem == "all" && item.IsObject && itemName != "level") || (targetItem == "text" && !item.IsObject);
				if ((!target.Not && isItem) || (target.Not && !isItem && itemName != "level" && item.IsObject)) {
					applies = true;
					break;
				}
			}

			if (!applies) { return false; }

			foreach (ExtraRule extra in Extras) {
				foreach (Target target in extra.Targets) {
					if (extra.Extra.Name == "text_facing") {

					}
				}
			}

			return applies;
		}
		public override bool Equals(object obj) {
			if (obj == null || !(obj is Rule)) {
				return false;
			}
			Rule rule = (Rule)obj;
			bool pass = (rule.Lonely == null && Lonely == null) || (Lonely != null && Lonely.Equals(rule.Lonely));
			if (!pass || Targets.Count != rule.Targets.Count || Extras.Count != rule.Extras.Count || Actions.Count != rule.Actions.Count) { return false; }

			for (int i = 0; i < Targets.Count; i++) {
				Target target = Targets[i];
				if (!target.Equals(rule.Targets[i])) {
					return false;
				}
			}

			for (int i = 0; i < Extras.Count; i++) {
				ExtraRule extra = Extras[i];
				if (!extra.Equals(rule.Extras[i])) {
					return false;
				}
			}

			for (int i = 0; i < Actions.Count; i++) {
				ExtraRule extra = Actions[i];
				if (!extra.Equals(rule.Actions[i])) {
					return false;
				}
			}
			return true;
		}
		public override int GetHashCode() {
			return Targets.Count ^ Extras.Count ^ Actions.Count;
		}
		public bool IsValid() {
			for (int i = Extras.Count - 1; i >= 0; i--) {
				bool valid = Extras[i].IsValid();
				if (!valid) {
					Extras.RemoveAt(i);
				}
			}
			for (int i = Actions.Count - 1; i >= 0; i--) {
				bool valid = Actions[i].IsValid();
				if (!valid) {
					Actions.RemoveAt(i);
				}
			}
			return Targets.Count > 0 && Actions.Count > 0;
		}
		public List<string> AllRules() {
			List<string> rules = new List<string>();
			foreach (Target target in Targets) {
				StringBuilder rule = new StringBuilder();
				if (Lonely != null) {
					rule.Append($"{Lonely.ToString()} ");
				}

				rule.Append(target.ToString()).Append(' ');

				for (int i = 0; i < Extras.Count; i++) {
					ExtraRule extra = Extras[i];
					rule.Append($"({extra.ToString()}) ");
				}

				for (int i = 0; i < Actions.Count; i++) {
					ExtraRule extra = Actions[i];
					string currentRule = rule.ToString();
					foreach (Target action in extra.Targets) {
						string not = extra.Not ? "not " : string.Empty;
						rules.Add($"{currentRule}{not}{extra.Extra.Name.Substring(5)} {action.ToString()}");
					}
				}
			}
			return rules;
		}
		public override string ToString() {
			StringBuilder allRules = new StringBuilder();
			List<string> rules = AllRules();
			for (int i = 0; i < rules.Count; i++) {
				allRules.AppendLine(rules[i]);
			}
			return allRules.ToString().Trim();
		}
	}
	public class ExtraRule {
		public Item Extra;
		public bool Not;
		public List<Target> Targets = new List<Target>();

		public ExtraRule(Item extra) {
			Extra = extra;
		}
		public bool IsValid() {
			return Targets.Count > 0;
		}
		public override bool Equals(object obj) {
			if (obj == null || !(obj is ExtraRule)) {
				return false;
			}

			ExtraRule rule = (ExtraRule)obj;
			if (Extra.ID != rule.Extra.ID || Extra.Position != rule.Extra.Position || Not != rule.Not || rule.Targets.Count != Targets.Count) { return false; }

			for (int i = 0; i < Targets.Count; i++) {
				Target target = Targets[i];
				if (!target.Equals(rule.Targets[i])) {
					return false;
				}
			}
			return true;
		}
		public override int GetHashCode() {
			return Extra.ID ^ Extra.Position;
		}
		public override string ToString() {
			StringBuilder rule = new StringBuilder();
			bool allNots = Targets.Count > 0;
			foreach (Target target in Targets) {
				allNots &= target.Not;
			}
			rule.Append(Not || allNots ? "not " : string.Empty).Append($"{Extra.Name.Substring(5)} ");
			foreach (Target target in Targets) {
				rule.Append(target.ToString(!Not && !allNots)).Append(" and ");
			}
			if (Targets.Count > 0) {
				rule.Length -= 5;
			}
			return rule.ToString();
		}
	}
	public class Target {
		public bool FromWord;
		public bool Not;
		public Item Item;

		public Target(Item item, bool not) : this(item, not, false) { }
		public Target(Item item, bool not, bool fromWord) {
			Item = item;
			Not = not;
			FromWord = fromWord;
		}
		public override bool Equals(object obj) {
			return obj != null && obj is Target target && target.Not == Not && target.Item.Position == Item.Position && target.Item.Name == Item.Name;
		}
		public override int GetHashCode() {
			return Item.ID ^ Item.Position;
		}
		public string ToString(bool otherNot) {
			return (Not && otherNot ? "not " : string.Empty) + (FromWord ? "*" : string.Empty) + Item.Name.Replace("text_", "") + (FromWord ? "*" : string.Empty);
		}
		public override string ToString() {
			return ToString(true);
		}
	}
}