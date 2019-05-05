using System.Collections.Generic;
using System.Text;
namespace BabaIsYou.Map {
	public enum ParseDirection {
		Right,
		Down
	}
	public class Sentance {
		public List<Item> Words = new List<Item>();

		public Sentance Copy(int copyAmount = int.MaxValue) {
			Sentance sentance = new Sentance();
			int count = Words.Count;
			for (int i = 0; i < count; i++) {
				if (i < copyAmount) {
					sentance.Words.Add(Words[i]);
				}
			}
			return sentance;
		}
		public override string ToString() {
			StringBuilder sb = new StringBuilder();
			int count = Words.Count;
			for (int i = 0; i < count; i++) {
				sb.Append($"{Words[i].Name} ");
			}
			if (sb.Length > 0) { sb.Length--; }
			return sb.ToString();
		}
	}
	public class Word {
		public Item Item;
		public StringBuilder Name = new StringBuilder();
		public int EndPosition;
		public int EndDimension;

		public Word Copy(int copyLength = int.MaxValue) {
			Word word = new Word();
			word.Name.Append(Name.ToString(0, Name.Length < copyLength ? Name.Length : copyLength));
			return word;
		}

		public bool IsValid(int position) {
			Item item = null;
			if (!Reader.DefaultsByName.TryGetValue($"text_{Name.ToString()}", out item) || item.Type == (byte)TextType.Letter) {
				return false;
			}
			Item = item.Copy();
			Item.Position = (short)position;
			return true;
		}
		public override string ToString() {
			return Name.ToString();
		}
	}
	public class Parser {
		public List<Rule> Rules = new List<Rule>();
		private bool useObjects;
		public Parser(Grid grid, bool includeObjects) {
			useObjects = includeObjects;
			ParseRules(grid);
		}
		private void ParseRules(Grid grid) {
			List<Sentance> sentances = new List<Sentance>();
			int x = 0;
			int y = 1;
			HashSet<int> calcRight = new HashSet<int>();
			HashSet<int> calcDown = new HashSet<int>();
			int size = grid.Cells.Count;
			for (int i = grid.Width; i < size; i++) {
				if (x > 0) {
					if (!calcRight.Contains(i)) {
						calcRight.Add(i);
						Parse(sentances, new Sentance(), calcRight, grid, i, x, ParseDirection.Right);
					}
					if (!calcDown.Contains(i)) {
						calcDown.Add(i);
						Parse(sentances, new Sentance(), calcDown, grid, i, y, ParseDirection.Down);
					}
				}

				x++;
				if (x >= grid.Width) {
					x = 0;
					y++;
				}
			}

			size = sentances.Count;
			for (int i = 0; i < size; i++) {
				Sentance sentance = sentances[i];
				SentanceParser parser = new SentanceParser(sentance, useObjects);
				List<Rule> rules = parser.Parse();
				for (int j = rules.Count - 1; j >= 0; j--) {
					Rule rule = rules[j];
					if (!ContainsRule(Rules, rule)) {
						Rules.Add(rule);
					}
				}
			}
		}
		private bool ContainsRule(List<Rule> rules, Rule newRule) {
			for (int i = rules.Count - 1; i >= 0; i--) {
				Rule rule = rules[i];
				if (rule.Equals(newRule)) {
					return true;
				}
			}
			return false;
		}
		private void Parse(List<Sentance> sentances, Sentance current, HashSet<int> calculated, Grid grid, int position, int dimension, ParseDirection direction) {
			int dimensionCheck = direction == ParseDirection.Right ? grid.Width : grid.Height;
			int newPosition = position + (direction == ParseDirection.Right ? 1 : grid.Width);
			if (dimension >= dimensionCheck) {
				if (current.Words.Count > 2) {
					sentances.Add(current);
				}
				return;
			}

			int currentCount = current.Words.Count;
			int added = 0;
			calculated.Add(position);
			Cell cell = grid.Cells[position];
			int objects = cell.Objects.Count;
			for (int j = 0; j < objects; j++) {
				Item item = cell.Objects[j];
				if (item.ID != 0 && item.ID != short.MaxValue) {
					if (!item.IsObject && item.Type == (byte)TextType.Letter) {
						List<Word> words = new List<Word>();
						Word word = new Word();
						word.Name.Append(item.Name.Substring(5));
						FindWords(words, word, calculated, grid, newPosition, dimension + 1, direction);
						for (int i = 0; i < words.Count; i++) {
							word = words[i];
							Sentance newSentance = added == 0 ? current : current.Copy(currentCount);
							added++;
							newSentance.Words.Add(word.Item);
							Parse(sentances, newSentance, calculated, grid, word.EndPosition, word.EndDimension, direction);
						}
					} else if (!item.IsObject || useObjects) {
						Sentance newSentance = added == 0 ? current : current.Copy(currentCount);
						added++;
						newSentance.Words.Add(item);
						Parse(sentances, newSentance, calculated, grid, newPosition, dimension + 1, direction);
					}
				}
			}

			if (added == 0 && current.Words.Count > 2) {
				sentances.Add(current);
			}
		}
		private void FindWords(List<Word> words, Word current, HashSet<int> calculated, Grid grid, int position, int dimension, ParseDirection direction) {
			int dimensionCheck = direction == ParseDirection.Right ? grid.Width : grid.Height;
			int newPosition = position + (direction == ParseDirection.Right ? 1 : grid.Width);
			if (dimension >= dimensionCheck) { return; }

			int currentCount = current.Name.Length;
			int added = 0;
			calculated.Add(position);
			Cell cell = grid.Cells[position];
			int objects = cell.Objects.Count;
			for (int j = 0; j < objects; j++) {
				Item item = cell.Objects[j];
				if (item.ID != 0 && item.ID != short.MaxValue && !item.IsObject && item.Type == (byte)TextType.Letter) {
					Word newWord = added == 0 ? current : current.Copy(currentCount);
					added++;

					newWord.Name.Append(item.Name.Substring(5));
					newWord.EndPosition = newPosition;
					newWord.EndDimension = dimension + 1;

					if (newWord.IsValid(item.Position)) {
						words.Add(newWord);
						newWord = newWord.Copy();
					}
					FindWords(words, newWord, calculated, grid, newPosition, dimension + 1, direction);
				}
			}
		}
	}
	public class SentanceParser {
		private Sentance expression;
		private int currentIndex;
		private Item current;
		private TextType token;
		private bool includeObjects;

		public SentanceParser(Sentance sentance, bool useObjects) {
			expression = sentance;
			currentIndex = 0;
			includeObjects = useObjects;
		}

		private void GetNext() {
			current = currentIndex < expression.Words.Count ? expression.Words[currentIndex++] : null;
			token = current == null ? (TextType)255 : (TextType)current.Type;
		}
		public List<Rule> Parse() {
			List<Rule> rules = new List<Rule>();

			int lastIndex = 0;
			currentIndex = 0;
			GetNext();
			while (token != (TextType)255) {
				lastIndex++;
				Rule rule = ParseStart();
				if (rule.IsValid()) {
					rules.Add(rule);
					lastIndex = currentIndex - 2;
				}
				currentIndex = lastIndex;
				GetNext();
			}

			return rules;
		}
		private Rule ParseStart() {
			Rule rule = new Rule();

			bool not = ParseNot();
			bool lonely = token == TextType.Lonely;
			if (lonely) {
				rule.Lonely = new Target(current, not);
				GetNext();
			}

			ParseTarget(rule, null, !lonely && not);

			return rule;
		}
		private bool ParseNot() {
			bool isNot = token == TextType.Not;
			bool not = false;
			while (token == TextType.Not) {
				not = !not;
				GetNext();
			}
			return isNot && not;
		}
		private void ParseTarget(Rule rule, ExtraRule extra, bool not) {
			if ((token != TextType.Noun || (!includeObjects && current.IsObject)) && token != TextType.Not && (extra == null || extra.Extra.Name != "text_facing" || (current.Name != "text_up" && current.Name != "text_left" && current.Name != "text_right" && current.Name != "text_down"))) {
				return;
			}

			not = not || ParseNot();
			if (token == TextType.Noun) {
				List<Target> targets = extra != null ? extra.Targets : rule.Targets;
				targets.Add(new Target(current, not, current.IsObject));
				GetNext();
			}

			if (token == TextType.And) {
				GetNext();
				ParseTarget(rule, extra, false);
			} else if (token == TextType.Not || token == TextType.Extra) {
				ParseExtra(rule, not);
			} else if (token == TextType.Link) {
				ParseLink(rule, false);
			}
		}
		private void ParseExtra(Rule rule, bool not) {
			not = not || ParseNot();
			if (token != TextType.Extra) {
				return;
			}

			ExtraRule extra = new ExtraRule(current);
			extra.Not = not;
			rule.Extras.Add(extra);
			GetNext();
			ParseTarget(rule, extra, false);
		}
		private void ParseLink(Rule rule, bool not) {
			not = not || ParseNot();
			if (token != TextType.Link) {
				return;
			}

			ExtraRule extra = new ExtraRule(current);
			extra.Not = not;
			rule.Actions.Add(extra);
			GetNext();
			ParseAction(rule, extra, false);
		}
		private void ParseAction(Rule rule, ExtraRule extra, bool not) {
			not = not || ParseNot();
			if ((token != TextType.Noun || (!includeObjects && current.IsObject)) && (extra.Extra.Name != "text_is" || token != TextType.Action)) {
				return;
			}

			extra.Targets.Add(new Target(current, not, current.IsObject));
			GetNext();

			if (token == TextType.And) {
				GetNext();
				not = ParseNot();
				if (token == TextType.Link) {
					ParseLink(rule, not);
				} else {
					ParseAction(rule, extra, not);
				}
			}
		}
	}
}