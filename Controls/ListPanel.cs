using BabaIsYou.Map;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;
namespace BabaIsYou.Controls {
	public class ListPanel : MessageFilter {
		public delegate void SelectedIndexChangedEvent(int index, ListItem item);
		public event SelectedIndexChangedEvent IndexChanged;
		public delegate void ItemClickedEvent(ListItem item, MouseButtons buttons);
		public event ItemClickedEvent ItemClicked;
		public delegate void RenderItemEvent(ListItem item, Graphics g);
		public event RenderItemEvent RenderItem;
		private static Bitmap Warning;

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
		[DefaultValue(false)]
		public bool UseControlMovement { get; set; }
		[DefaultValue(true)]
		public bool SortByText { get; set; }

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
							if (SelectedIndex == i) {
								selectedIndex = -1;
							}
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

				//Get first visible or top of list
				SetTopToVisible(true);
				int maxIndex = RemoveEmptySpace();

				//Scroll up if selected is higher
				if (topIndex >= newIndex) {
					if (newIndex >= 0) {
						topIndex = newIndex;
						SetTopToVisible(true);
					}
				} else {
					//Scroll down if selected is lower
					while (maxIndex < newIndex) {
						SetTopToVisible(false);
						maxIndex = MaxIndex(true);
					}
				}

				if (newIndex != selectedIndex) {
					selectedIndex = newIndex;
					IndexChanged?.Invoke(newIndex, newIndex < 0 ? null : Items[newIndex]);
					Invalidate();
				}
			}
		}

		private List<ListItem> Items = new List<ListItem>();
		private int topIndex = 0;
		private StringBuilder typedString;
		private DateTime lastTyped;
		static ListPanel() {
			Warning = Renderer.GetBitmapFromAssembly("BabaIsYou.Images.changed.png");
		}
		public ListPanel() : base() {
			DrawText = true;
			SortByText = true;
			DoubleBuffered = true;
			UseControlMovement = false;
			typedString = new StringBuilder();
			lastTyped = DateTime.MinValue;
			Focusable = true;
			ResizeRedraw = true;
		}

		private int RemoveEmptySpace() {
			int maxIndex = MaxIndex(true);
			bool removeExtraSpace = maxIndex + 1 == Items.Count;
			while (topIndex > 0 && maxIndex + 1 == Items.Count) {
				topIndex--;
				SetTopToVisible(true);
				maxIndex = MaxIndex(true);
			}
			if (removeExtraSpace && maxIndex + 1 < Items.Count) {
				SetTopToVisible(false);
				maxIndex = Items.Count;
			}
			return maxIndex;
		}
		private void SetTopToVisible(bool up) {
			if (up) {
				while (topIndex > 0 && !Items[topIndex].Visible) {
					topIndex--;
				}
			} else {
				int currentTop = topIndex;
				do {
					topIndex++;
				} while (topIndex < Items.Count && !Items[topIndex].Visible);
				if (topIndex >= Items.Count) {
					topIndex = currentTop;
				}
			}
		}
		public void ReselectCurrentIndex() {
			int current = selectedIndex;
			if (current >= Items.Count) {
				current = Items.Count - 1;
			}
			while (current >= 0 && !Items[current].Visible) {
				current--;
			}
			if (current < 0 && Items.Count > 0) {
				current = 0;
				while (current < Items.Count && !Items[current].Visible) {
					current++;
				}
			}
			if (current >= Items.Count) {
				current = -1;
			}
			if (selectedIndex == current) {
				selectedIndex = -1;
			}
			SelectedIndex = current;
		}
		public void AddItem(ListItem item) {
			if (item.Parent != null) {
				throw new Exception($"This ListItem ({item.Text}) already has a parent ListPanel ({item.Parent.Name})");
			}
			item.Parent = this;
			Items.Add(item);
		}
		public void AddItems(List<ListItem> items) {
			int size = items.Count;
			for (int i = 0; i < size; i++) {
				ListItem item = items[i];
				AddItem(item);
			}
		}
		public bool RemoveItem(ListItem item) {
			return Items.Remove(item);
		}
		public void ClearItems() {
			Items.Clear();
			topIndex = 0;
			selectedIndex = -1;
		}
		public void SortItems() {
			Items.Sort();
		}
		public int Count { get { return Items.Count; } }
		public ListItem this[int index] {
			get { return Items[index]; }
		}
		private int RowLength() {
			if (Items.Count == 0) { return -1; }
			ListItem item = Items[0];
			int result = 0;
			if (vertical) {
				int width = item.Width;
				result = Width / width;
				if (result == 0) {
					result = 1;
				}
			} else {
				int height = item.Height;
				result = Height / height;
				if (result == 0) {
					result = 1;
				}
			}
			return result;
		}
		private int MaxIndex(bool contains = false, bool stopAtRow = false) {
			int endHeight = vertical ? Height : Width;
			int endWidth = vertical ? Width : Height;
			int rowLength = RowLength();
			int maxSize = 0;
			int currentHeight = 0;
			int rowSize = 0;
			for (int i = topIndex; i < Items.Count; i++) {
				ListItem item = Items[i];
				if (!item.Visible) { continue; }

				int height = vertical ? item.Height : item.Width;
				if (height > maxSize) {
					maxSize = height;
				}
				rowSize++;

				if (contains && height + currentHeight > endHeight) {
					return i - 1;
				} else if (rowSize == rowLength) {
					currentHeight += maxSize;
					maxSize = 0;
					rowSize = 0;
					if (stopAtRow || currentHeight >= endHeight) {
						return stopAtRow ? i + 1 : i;
					}
				}
			}
			return Items.Count - 1;
		}
		public bool SelectTopMostVisible() {
			bool selected = false;
			for (int i = 0; i < Items.Count; i++) {
				ListItem item = Items[i];
				if (!item.Visible) { continue; }

				selected = true;
				if (SelectedIndex == i) {
					selectedIndex = -1;
				}
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
		protected override bool ProcessCmdKey(ref Message msg, Keys keyData) {
			KeyDownLogic(new KeyEventArgs(keyData));
			return base.ProcessCmdKey(ref msg, keyData);
		}
		private void KeyDownLogic(KeyEventArgs e) {
			bool stopLogic = e.Alt || e.Control || e.Shift;
			if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down || e.KeyCode == Keys.Right || e.KeyCode == Keys.Left) {
				stopLogic = e.Alt || e.Shift || (UseControlMovement ^ e.Control);
			}
			if (stopLogic) { return; }

			int starting = selectedIndex;
			bool overflowed = false;
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
					for (int i = vertical ? 1 : RowLength(); i > 0; i--) {
						while (starting > 0 && !Items[--starting].Visible) { }
						if (starting == 0 && i > 1) {
							overflowed = true;
							break;
						}
					}
					if (selectedIndex != starting && !overflowed) {
						if (!vertical && starting < topIndex) {
							MoveList(false, false);
						}
						SelectedIndex = starting;
					}
					break;
				case Keys.Up:
					for (int i = vertical ? RowLength() : 1; i > 0; i--) {
						while (starting > 0 && !Items[--starting].Visible) { }
						if (starting == 0 && i > 1) {
							overflowed = true;
							break;
						}
					}
					if (selectedIndex != starting && !overflowed) {
						if (vertical && starting < topIndex) {
							MoveList(false, false);
						}
						SelectedIndex = starting;
					}
					break;
				case Keys.Right:
					for (int i = vertical ? 1 : RowLength(); i > 0; i--) {
						while (starting + 1 < Items.Count && !Items[++starting].Visible) { }
						if (starting + 1 >= Items.Count && i > 1) {
							overflowed = true;
							break;
						}
					}
					if (selectedIndex != starting && !overflowed) {
						if (!vertical && starting > MaxIndex()) {
							MoveList(true, false);
						}
						SelectedIndex = starting;
					}
					break;
				case Keys.Down:
					for (int i = vertical ? RowLength() : 1; i > 0; i--) {
						while (starting + 1 < Items.Count && !Items[++starting].Visible) { }
						if (starting + 1 >= Items.Count && i > 1) {
							overflowed = true;
							break;
						}
					}
					if (selectedIndex != starting && !overflowed) {
						if (vertical && starting > MaxIndex()) {
							MoveList(true, false);
						}
						SelectedIndex = starting;
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
			int rowLength = RowLength();
			if (down) {
				int maxTopIndex = MaxIndex(true);
				if (topIndex != maxTopIndex && maxTopIndex < Items.Count - 1) {
					int currentTop = topIndex;
					if (page) {
						topIndex = maxTopIndex;
						SetTopToVisible(false);
						RemoveEmptySpace();
					} else {
						while (rowLength > 0) {
							SetTopToVisible(false);
							rowLength--;
						}
					}
					if (currentTop != topIndex) {
						Invalidate();
					}
				}
			} else {
				int currentTop = topIndex;
				if (page) {
					if (topIndex > 0) {
						int maxIndex;
						do {
							topIndex--;
							SetTopToVisible(true);
							maxIndex = MaxIndex();
						} while (maxIndex >= currentTop && topIndex > 0);
					}
				} else {
					while (topIndex > 0 && rowLength > 0) {
						topIndex--;
						SetTopToVisible(true);
						rowLength--;
					}
				}
				if (topIndex != currentTop) {
					Invalidate();
				}
			}
		}
		protected override void OnMouseClick(MouseEventArgs e) {
			if (!Focused && Focusable) { Focus(); }

			int endHeight = vertical ? Height : Width;
			int endWidth = vertical ? Width : Height;
			int rowLength = RowLength();
			int maxSize = 0;
			int currentHeight = 0;
			int currentWidth = 0;
			int rowSize = 0;
			for (int i = topIndex; i < Items.Count; i++) {
				ListItem item = Items[i];
				if (!item.Visible) { continue; }

				int height = vertical ? item.Height : item.Width;
				int width = vertical ? item.Width : item.Height;
				if (height > maxSize) {
					maxSize = height;
				}
				rowSize++;

				if (CheckBounds(e.Location, vertical ? currentWidth : currentHeight, vertical ? currentHeight : currentWidth, item.Width, item.Height)) {
					if (e.Button == MouseButtons.Left) {
						SelectedIndex = i;
					}
					ItemClicked?.Invoke(item, e.Button);
					break;
				}
				currentWidth += width;

				if (rowSize == rowLength) {
					currentHeight += maxSize;
					currentWidth = 0;
					maxSize = 0;
					rowSize = 0;
					if (currentHeight >= endHeight) {
						break;
					}
				}
			}
		}
		private bool CheckBounds(Point location, int x, int y, int w, int h) {
			return location.X >= x && location.X < x + w && location.Y >= y && location.Y < y + h;
		}
		protected override void OnPaint(PaintEventArgs e) {
			PaintList(e.Graphics);
		}
		public void PaintList(Graphics g) {
			int endHeight = vertical ? Height : Width;
			int endWidth = vertical ? Width : Height;
			int rowLength = RowLength();
			int maxSize = 0;
			int currentHeight = 0;
			int currentWidth = 0;
			int rowSize = 0;
			for (int i = topIndex; i < Items.Count; i++) {
				ListItem item = Items[i];
				if (!item.Visible) { continue; }

				int height = vertical ? item.Height : item.Width;
				int width = vertical ? item.Width : item.Height;
				if (height > maxSize) {
					maxSize = height;
				}
				rowSize++;

				Render(item, g, selectedIndex == i, vertical ? currentWidth : currentHeight, vertical ? currentHeight : currentWidth, DrawText);
				currentWidth += width;

				if (rowSize == rowLength) {
					currentHeight += maxSize;
					currentWidth = 0;
					maxSize = 0;
					rowSize = 0;
					if (currentHeight >= endHeight) {
						break;
					}
				}
			}
		}
		private void Render(ListItem item, Graphics g, bool selected, int x, int y, bool drawText) {
			if (item.BackColor.A != 0) {
				using (SolidBrush brush = new SolidBrush(item.BackColor)) {
					g.FillRectangle(brush, x, y, item.Width, item.Height);
				}
			}

			if (item.Image == null) {
				if (RenderItem != null) {
					Bitmap img = new Bitmap(item.Width, item.Height);
					using (Graphics graphics = Graphics.FromImage(img)) {
						RenderItem.Invoke(item, graphics);
					}
					item.Image = img;
				}
			}
			g.InterpolationMode = InterpolationMode.NearestNeighbor;
			g.PixelOffsetMode = PixelOffsetMode.Half;
			if (item.Image != null) {
				g.DrawImage(item.Image, x, y);
			}

			Rectangle bounds = new Rectangle(x + 2, y + 2, item.Width - 4, item.Height - 4);
			if (drawText) {
				if (!string.IsNullOrEmpty(item.Text)) {
					TextRenderer.DrawText(g, item.Text, this.Font, bounds, this.ForeColor, Color.Empty, TextFormatFlags.HorizontalCenter | TextFormatFlags.Bottom);
				}
				string valueText = item.Value == null ? string.Empty : item.Value.ToString();
				if (!string.IsNullOrEmpty(valueText)) {
					TextRenderer.DrawText(g, valueText, this.Font, bounds, this.ForeColor, Color.Empty, TextFormatFlags.HorizontalCenter | TextFormatFlags.Top);
				}
			}

			if (item.Changed && Warning != null) {
				Size size = TopLevelControl.ClientSize;
				int sizeX = size.Width / 90;
				int sizeY = size.Height / 90;
				if (sizeX > sizeY) { sizeX = sizeY; }
				sizeY = bounds.Width / 4;
				if (sizeX > sizeY) { sizeX = sizeY; }
				sizeY = bounds.Height / 4;
				if (sizeX > sizeY) { sizeX = sizeY; }
				g.DrawImage(Warning, new Rectangle(bounds.X - 1, bounds.Y - 1, sizeX, sizeX), 0, 0, Warning.Width, Warning.Height, GraphicsUnit.Pixel);
			}

			if (selected) {
				g.DrawRectangle(Pens.DarkRed, bounds.X - 2, bounds.Y - 2, bounds.Width + 3, bounds.Height + 3);
				g.DrawRectangle(Pens.Red, bounds.X - 1, bounds.Y - 1, bounds.Width + 1, bounds.Height + 1);
			}
		}
	}
	public class ListItem : IComparable {
		public ListPanel Parent { get; set; }
		public string Text { get; set; }
		public Bitmap Image { get; set; }
		public object Value { get; set; }
		public bool Changed { get; set; }
		public bool Visible { get; set; }
		public Color BackColor { get; set; }
		private int itemWidth;
		public int Width {
			get { return itemWidth; }
			set {
				if (value != itemWidth) {
					itemWidth = value;
					if (Image != null) { Image.Dispose(); }
					Image = null;
				}
			}
		}
		private int itemHeight;
		public int Height {
			get { return itemHeight; }
			set {
				if (value != itemHeight) {
					itemHeight = value;
					if (Image != null) { Image.Dispose(); }
					Image = null;
				}
			}
		}

		public ListItem(object value, string text, int width, int height) {
			BackColor = Color.Transparent;
			Text = text;
			Value = value;
			Visible = true;
			itemWidth = width;
			itemHeight = height;
		}
		public ListItem(object value, string text, Bitmap image) {
			BackColor = Color.Transparent;
			Text = text;
			Value = value;
			Visible = true;
			Image = image;
			itemWidth = image.Width;
			itemHeight = image.Height;
		}
		public int CompareTo(object obj) {
			if (obj is ListItem item) {
				if (Parent == null || Parent.SortByText) {
					return Text.CompareTo(item.Text);
				} else {
					return (Value == null ? string.Empty : Value.ToString()).CompareTo(item.Value == null ? string.Empty : item.Value.ToString());
				}
			}
			return -1;
		}
		public override string ToString() {
			return $"{Parent.Name} {Text}";
		}
	}
}