using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
namespace BabaIsYou.Controls {
	public class ListPanel : Control {
		public delegate void SelectedIndexChangedEvent(int index, ListItem item);
		public event SelectedIndexChangedEvent IndexChanged;
		public delegate void ItemClickedEvent(ListItem item, MouseButtons buttons);
		public event ItemClickedEvent ItemClicked;

		[EditorBrowsable(EditorBrowsableState.Never), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), ReadOnly(true)]
		public List<ListItem> Items { get; }
		[DefaultValue(true)]
		public bool Focusable { get; set; }
		private bool vertical = true;
		[DefaultValue(true)]
		public bool Vertical {
			get { return vertical; }
			set {
				if (vertical != value) {
					vertical = value;
					Invalidate();
				}
			}
		}
		[DefaultValue(true)]
		public bool DrawText { get; set; }
		[EditorBrowsable(EditorBrowsableState.Never), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), ReadOnly(true)]
		public ListItem SelectedItem {
			get {
				return selectedIndex < 0 || selectedIndex >= Items.Count ? null : Items[selectedIndex];
			}
			set {
				if (value == null) {
					SelectedIndex = -1;
				} else {
					for (int i = 0; i < Items.Count; i++) {
						if (Items[i] == value) {
							SelectedIndex = i;
							return;
						}
					}
					SelectedIndex = -1;
				}
			}
		}
		private int selectedIndex = -1;
		[EditorBrowsable(EditorBrowsableState.Never), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), ReadOnly(true)]
		public int SelectedIndex {
			get { return selectedIndex; }
			set {
				int newIndex = Items.Count == 0 ? -1 : value < 0 ? -1 : value >= Items.Count ? Items.Count - 1 : value;
				if (topIndex >= newIndex) {
					if (newIndex >= 0) {
						topIndex = newIndex;
					}
				} else {
					int maxIndex = MaxIndex(true);
					if (newIndex > maxIndex) {
						topIndex = newIndex;
						do {
							topIndex--;
							maxIndex = MaxIndex(true);
						} while (maxIndex >= newIndex);

						if (maxIndex < newIndex) {
							topIndex++;
						}
					}
				}
				if (newIndex != selectedIndex) {
					selectedIndex = newIndex;
					IndexChanged?.Invoke(newIndex, newIndex < 0 ? null : Items[newIndex]);
					Invalidate();
				}
			}
		}
		private int topIndex = 0;
		private StringBuilder typedString;
		private DateTime lastTyped;
		public ListPanel() {
			DrawText = true;
			Items = new List<ListItem>();
			DoubleBuffered = true;
			typedString = new StringBuilder();
			lastTyped = DateTime.MinValue;
			Focusable = true;
		}

		public void EnsureSelectedInView() {
			Invalidate();
		}
		private int MaxIndex(bool contains = false, bool nextLine = false) {
			int pos = 0;
			int i = topIndex;
			int compareTo = vertical ? Height : Width;
			int compareToExt = vertical ? Width : Height;
			int maxVal = 0;
			int line = 0;
			int added = 0;
			while (i < Items.Count) {
				ListItem item = Items[i++];
				if (!item.Visible) { continue; }

				Size size = item.Size;
				int val = vertical ? size.Height : size.Width;
				int ext = vertical ? size.Width : size.Height;
				line += ext;

				if (!nextLine && pos + val > compareTo) {
					i--;
					break;
				}

				if (line >= compareToExt) {
					bool toAdd = line == compareToExt || added == 0;
					if (toAdd && val > maxVal) {
						maxVal = val;
					}

					pos += maxVal;
					line = 0;
					added = 0;
					maxVal = 0;

					if (!toAdd) {
						line = ext;
						maxVal = val;
					}

					if (nextLine) {
						return toAdd ? i : i - 1;
					}
				} else {
					added++;
					if (val > maxVal) {
						maxVal = val;
					}
				}
			}
			return !contains || nextLine ? i : i - 1 < topIndex ? topIndex : i - 1;
		}
		public bool SelectTopMostVisible() {
			bool selected = false;
			for (int i = 0; i < Items.Count; i++) {
				ListItem item = Items[i];
				if (!item.Visible) { continue; }

				selected = true;
				SelectedIndex = i;
				break;
			}

			if (!selected) {
				SelectedIndex = -1;
			}
			return selected;
		}
		public bool SelectItemWithText(string text) {
			for (int i = 0; i < Items.Count; i++) {
				ListItem item = Items[i];
				if (!item.Visible || string.IsNullOrEmpty(item.Text) || !item.Text.Equals(text, StringComparison.OrdinalIgnoreCase)) { continue; }

				SelectedIndex = i;
				return true;
			}
			return false;
		}
		protected override bool IsInputKey(Keys keyData) {
			if (keyData == Keys.Up || keyData == Keys.Down || keyData == Keys.Right || keyData == Keys.Left) {
				OnKeyDown(new KeyEventArgs(keyData));
			}
			return false;
		}
		protected override void OnKeyDown(KeyEventArgs e) {
			if (e.Alt || e.Control || e.Shift) { return; }

			switch (e.KeyCode) {
				case Keys.Home: {
					if (topIndex != 0) {
						topIndex = 0;
						Invalidate();
					}
					break;
				}
				case Keys.End: {
					int currentTop = topIndex;
					topIndex = Items.Count - 1;
					int maxIndex = MaxIndex(true);
					while (maxIndex == Items.Count - 1) {
						topIndex--;
						maxIndex = MaxIndex(true);
					}
					topIndex++;
					if (topIndex >= Items.Count) { topIndex--; }
					if (currentTop != topIndex) {
						Invalidate();
					}
					break;
				}
				case Keys.Left:
				case Keys.Up:
					if (SelectedIndex > 0) {
						SelectedIndex--;
					}
					break;
				case Keys.Right:
				case Keys.Down:
					if (SelectedIndex + 1 < Items.Count) {
						SelectedIndex++;
					}
					break;
				case Keys.PageDown:
					MoveList(true, true);
					break;
				case Keys.PageUp:
					MoveList(false, true);
					break;
				default: {
					string keyVal = e.KeyCode == Keys.Space ? " " : e.KeyCode.ToString();
					if (keyVal.Length == 1) {
						if ((DateTime.Now - lastTyped).TotalSeconds > 1) {
							typedString.Length = 0;
						}
						lastTyped = DateTime.Now;
						typedString.Append(keyVal);

						keyVal = typedString.ToString();
						for (int i = 0; i < Items.Count; i++) {
							ListItem item = Items[i];
							if (!item.Visible) { continue; }

							if (item.Text.IndexOf(keyVal, StringComparison.OrdinalIgnoreCase) == 0) {
								SelectedIndex = i;
								break;
							}
						}
					}
					break;
				}
			}
		}
		protected override void OnMouseWheel(MouseEventArgs e) {
			if (e.Delta < 0) {
				MoveList(true, false);
			} else if (e.Delta > 0) {
				MoveList(false, false);
			}
		}
		private void MoveList(bool down, bool page) {
			if (down) {
				if (page) {
					int maxTopIndex = MaxIndex();
					if (topIndex != maxTopIndex && maxTopIndex < Items.Count - 1) {
						topIndex = maxTopIndex;
						Invalidate();
					}
				} else {
					int maxIndex = MaxIndex(true);
					if (maxIndex < Items.Count - 1) {
						topIndex = MaxIndex(false, true);
						Invalidate();
					}
				}
			} else if (page) {
				if (topIndex > 0) {
					int currentTop = topIndex;
					int maxIndex;
					do {
						topIndex--;
						maxIndex = MaxIndex();
					} while (maxIndex != currentTop && topIndex > 0);

					if (topIndex != currentTop) {
						Invalidate();
					}
				}
			} else {
				if (topIndex > 0) {
					int currentTop = topIndex;
					int maxIndex;
					do {
						topIndex--;
						maxIndex = MaxIndex(false, true);
					} while (maxIndex != currentTop && topIndex > 0);
					Invalidate();
				}
			}
		}
		protected override void OnMouseClick(MouseEventArgs e) {
			if (!Focused && Focusable) { Focus(); }

			int pos = 0;
			int compareTo = vertical ? Height : Width;
			int compareToExt = vertical ? Width : Height;
			int maxVal = 0;
			int line = 0;
			int added = 0;
			for (int i = topIndex; i < Items.Count; i++) {
				ListItem item = Items[i];
				if (!item.Visible) { continue; }

				Size size = item.Size;
				int val = vertical ? size.Height : size.Width;
				int ext = vertical ? size.Width : size.Height;

				int lineToCheck = line;
				int posToCheck = pos;
				line += ext;

				if (line >= compareToExt) {
					bool toAdd = line == compareToExt || added == 0;
					if (toAdd && val > maxVal) {
						maxVal = val;
					}

					pos += maxVal;
					line = 0;
					added = 0;
					maxVal = 0;

					if (!toAdd) {
						line = ext;
						lineToCheck = 0;
						posToCheck = pos;
						maxVal = val;
					}
				} else {
					added++;
					if (val > maxVal) {
						maxVal = val;
					}
				}

				if (CheckBounds(e.Location, vertical ? lineToCheck : posToCheck, vertical ? posToCheck : lineToCheck, size.Width, size.Height)) {
					if (e.Button == MouseButtons.Left) {
						SelectedIndex = i;
					}
					ItemClicked?.Invoke(item, e.Button);
					break;
				}
			}
		}
		private bool CheckBounds(Point location, int x, int y, int w, int h) {
			return location.X >= x && location.X < x + w && location.Y >= y && location.Y < y + h;
		}
		protected override void OnPaint(PaintEventArgs e) {
			int pos = 0;
			int compareTo = vertical ? Height : Width;
			int compareToExt = vertical ? Width : Height;
			int maxVal = 0;
			int line = 0;
			int added = 0;
			for (int i = topIndex; i < Items.Count; i++) {
				ListItem item = Items[i];
				if (!item.Visible) { continue; }

				Size size = item.Size;
				int val = vertical ? size.Height : size.Width;
				int ext = vertical ? size.Width : size.Height;

				int lineToDraw = line;
				int posToDraw = pos;
				line += ext;

				if (line >= compareToExt) {
					bool toAdd = line == compareToExt || added == 0;
					if (toAdd && val > maxVal) {
						maxVal = val;
					}

					pos += maxVal;
					line = 0;
					added = 0;
					maxVal = 0;

					if (!toAdd) {
						line = ext;
						lineToDraw = 0;
						posToDraw = pos;
						maxVal = val;
					}
				} else {
					added++;
					if (val > maxVal) {
						maxVal = val;
					}
				}

				item.Render(this, e.Graphics, selectedIndex == i, vertical ? lineToDraw : posToDraw, vertical ? posToDraw : lineToDraw, DrawText);

				if (pos >= compareTo) {
					break;
				}
			}
		}
	}
	public class ListItem : IComparable {
		public string Text { get; set; }
		public Bitmap Image { get; set; }
		public Bitmap Extra { get; set; }
		public object Value { get; set; }
		public bool Changed { get; set; }
		public bool Visible { get; set; }
		public Color BackColor { get; set; }
		public Size Size { get { return Image.Size; } }
		private static Bitmap Warning;
		static ListItem() {
			Warning = (Bitmap)Bitmap.FromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("BabaIsYou.Images.changed.png"));
		}

		public ListItem(object value, string text, Bitmap image) {
			BackColor = Color.Transparent;
			Text = text;
			Value = value;
			Image = image;
			Visible = true;
		}

		public void Render(ListPanel panel, Graphics g, bool selected, int x, int y, bool drawText) {
			if (BackColor.A != 0) {
				using (SolidBrush brush = new SolidBrush(BackColor)) {
					g.FillRectangle(brush, x, y, Image.Width, Image.Height);
				}
			}
			g.DrawImage(Image, x, y);
			Rectangle bounds = new Rectangle(x + 2, y + 2, Image.Width - 4, Image.Height - 4);
			if (drawText) {
				if (!string.IsNullOrEmpty(Text)) {
					TextRenderer.DrawText(g, Text, panel.Font, bounds, panel.ForeColor, Color.Empty, TextFormatFlags.HorizontalCenter | TextFormatFlags.Bottom);
				}
				string valueText = Value.ToString();
				if (!string.IsNullOrEmpty(valueText)) {
					TextRenderer.DrawText(g, Value.ToString(), panel.Font, bounds, panel.ForeColor, Color.Empty, TextFormatFlags.HorizontalCenter | TextFormatFlags.Top);
				}
			}
			if (Changed && Warning != null) {
				g.DrawImage(Warning, bounds.X - 1, bounds.Y - 1);
			}
			if (selected) {
				g.DrawRectangle(Pens.DarkRed, bounds.X - 2, bounds.Y - 2, bounds.Width + 3, bounds.Height + 3);
				g.DrawRectangle(Pens.Red, bounds.X - 1, bounds.Y - 1, bounds.Width + 1, bounds.Height + 1);
			}
		}
		public int CompareTo(object obj) {
			if (obj is ListItem item) {
				return Text.CompareTo(item.Text);
			}
			return -1;
		}
	}
}