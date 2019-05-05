using BabaIsYou.Map;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
namespace BabaIsYou.Controls {
	public class MapViewer : MessageFilter {
		public delegate void CellMouseEvent(Grid map, Cell cell, MouseEventArgs e);
		public event CellMouseEvent CellMouseOver;
		public event CellMouseEvent CellMouseDown;
		public event CellMouseEvent CellMouseWheel;
		public delegate void DrawCurrentCellEvent(Graphics g, Grid map, Cell cell, Rectangle bounds);
		public event DrawCurrentCellEvent DrawCurrentCellStart;
		public event DrawCurrentCellEvent DrawCurrentCellFinish;
		public delegate void PaintFinishedEvent(Graphics g, Grid map);
		public event PaintFinishedEvent PaintFinished;

		private Grid currentMap;
		[EditorBrowsable(EditorBrowsableState.Never), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), ReadOnly(true)]
		public Grid Map {
			get { return currentMap; }
			set {
				if (currentMap != null) {
					currentMap.Resized -= MapResized;
				}
				currentMap = value;
				if (currentMap != null) {
					currentMap.Resized += MapResized;
				}
				currentCell = null;
				CellMouseOver?.Invoke(currentMap, null, null);
				Invalidate();
			}
		}
		private Cell currentCell;
		[EditorBrowsable(EditorBrowsableState.Never), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), ReadOnly(true)]
		public Cell CurrentCell {
			get { return currentCell; }
		}
		[EditorBrowsable(EditorBrowsableState.Never), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), ReadOnly(true)]
		public bool ShowStacked { get; set; }
		[EditorBrowsable(EditorBrowsableState.Never), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), ReadOnly(true)]
		public bool ShowDirections { get; set; }
		[EditorBrowsable(EditorBrowsableState.Never), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), ReadOnly(true)]
		public bool AllowEdgePlacement { get; set; }
		[EditorBrowsable(EditorBrowsableState.Never), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), ReadOnly(true)]
		public bool ShowAnimations { get; set; }
		private Thread redrawThread;
		private int frameCount = 0;
		public MapViewer() : base() {
			DoubleBuffered = true;
			ShowStacked = false;
			ShowDirections = false;
			ShowAnimations = true;
			AllowEdgePlacement = false;
			ResizeRedraw = true;
			redrawThread = new Thread(RedrawTimer);
			redrawThread.IsBackground = true;
			redrawThread.Start();
		}

		private void RedrawTimer() {
			while (!IsDisposed) {
				try {
					frameCount++;
					if (currentMap != null && ShowAnimations) {
						this.Invalidate();
					}
				} catch {
				}
				Thread.Sleep(170);
			}
		}
		private void MapResized(Grid map) {
			ClearCurrentCell();
		}
		public void ClearCurrentCell() {
			currentCell = null;
			CellMouseOver?.Invoke(currentMap, null, null);
			Invalidate();
		}
		protected override void OnMouseDown(MouseEventArgs e) {
			if (currentMap != null && currentCell != null) {
				CellMouseDown?.Invoke(currentMap, currentCell, e);
			}
		}
		protected override void OnMouseMove(MouseEventArgs e) {
			if (currentMap == null) { return; }

			Rectangle rectangle = Renderer.GetBounds(currentMap, Width, Height);
			int xpos = rectangle.X + (AllowEdgePlacement ? 0 : rectangle.Width);
			int ypos = rectangle.Y + (AllowEdgePlacement ? 0 : rectangle.Height);
			int edge = AllowEdgePlacement ? 0 : 2;
			if (CheckBounds(e.X, e.Y, xpos, ypos, rectangle.Width * (currentMap.Width - edge), rectangle.Height * (currentMap.Height - edge))) {
				int xOrig = rectangle.X;
				int size = currentMap.Cells.Count;
				int rowEnd = rectangle.X + rectangle.Width * currentMap.Width;
				for (int i = 0; i < size; i++) {
					Cell cell = currentMap.Cells[i];

					if (CheckBounds(e.X, e.Y, rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height)) {
						if (currentCell != cell) {
							currentCell = cell;
							CellMouseOver?.Invoke(currentMap, cell, e);
							if (e.Button != MouseButtons.None) {
								CellMouseDown?.Invoke(currentMap, currentCell, e);
							}
						}
						return;
					}

					rectangle.X += rectangle.Width;
					if (rectangle.X >= rowEnd) {
						rectangle.Y += rectangle.Height;
						rectangle.X = xOrig;
					}
				}
			} else if (currentCell != null) {
				currentCell = null;
				CellMouseOver?.Invoke(currentMap, null, e);
			}
		}
		protected override void OnMouseLeave(EventArgs e) {
			if (currentCell != null) {
				currentCell = null;
				CellMouseOver?.Invoke(currentMap, null, null);
			}
		}
		protected override void OnMouseWheel(MouseEventArgs e) {
			if (currentCell != null) {
				CellMouseWheel?.Invoke(currentMap, currentCell, e);
			}
		}
		private bool CheckBounds(int x1, int y1, int x2, int y2, int w, int h) {
			return x1 >= x2 && x1 < x2 + w && y1 >= y2 && y1 < y2 + h;
		}
		protected override void OnPaint(PaintEventArgs e) {
			if (currentMap == null) { return; }

			Rectangle rectangle = Renderer.GetBounds(currentMap, Width, Height);
			Rectangle saved = new Rectangle(-1, -1, -1, -1);

			int rowEnd = rectangle.X + rectangle.Width * currentMap.Width;
			int xOrig = rectangle.X;
			int size = currentMap.Cells.Count;
			for (int i = 0; i < size; i++) {
				Cell cell = currentMap.Cells[i];

				if (currentCell == cell) {
					saved = rectangle;
					break;
				}

				rectangle.X += rectangle.Width;
				if (rectangle.X >= rowEnd) {
					rectangle.Y += rectangle.Height;
					rectangle.X = xOrig;
				}
			}

			if (saved.X >= 0) {
				DrawCurrentCellStart?.Invoke(e.Graphics, currentMap, currentCell, saved);
			}

			Renderer.Render(currentMap, e.Graphics, Width, Height, ShowAnimations ? frameCount : 0, ShowStacked, ShowDirections);

			if (saved.X >= 0) {
				DrawCurrentCellFinish?.Invoke(e.Graphics, currentMap, currentCell, saved);
			}

			PaintFinished?.Invoke(e.Graphics, currentMap);
		}
	}
}