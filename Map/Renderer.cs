using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Reflection;
namespace BabaIsYou.Map {
	public class Renderer {
		private static Bitmap Selector;
		static Renderer() {
			Selector = (Bitmap)Bitmap.FromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("BabaIsYou.Images.grid.png"));
		}
		public static Rectangle GetBounds(Grid grid, int totalWidth, int totalHeight) {
			int tileWidth = totalWidth / grid.Width;
			int tileHeight = totalHeight / grid.Height;

			if (tileWidth > tileHeight) {
				tileWidth = tileHeight;
			} else if (tileWidth < tileHeight) {
				tileHeight = tileWidth;
			}

			int widthPadding = totalWidth - tileWidth * grid.Width;
			int heightPadding = totalHeight - tileHeight * grid.Height;
			int x = widthPadding / 2;
			int y = heightPadding / 2;
			return new Rectangle(x, y, tileWidth, tileHeight);
		}
		public static void Render(Grid grid, Graphics g, int totalWidth, int totalHeight, bool showStacked = false, bool showDirections = false) {
			Rectangle mapBounds = GetBounds(grid, totalWidth, totalHeight);
			int rowEnd = mapBounds.X + mapBounds.Width * grid.Width;
			Palette palette = Reader.Palettes[grid.Palette];
			g.InterpolationMode = InterpolationMode.NearestNeighbor;
			g.PixelOffsetMode = PixelOffsetMode.Half;

			using (SolidBrush brush = new SolidBrush(palette.Edge)) {
				g.FillRectangle(brush, 0, 0, totalWidth, totalHeight);
			}
			using (SolidBrush brush = new SolidBrush(palette.Background)) {
				g.FillRectangle(brush, mapBounds.X, mapBounds.Y, mapBounds.Width * grid.Width, mapBounds.Height * grid.Height);
			}
			for (int i = 0; i < grid.Images.Count; i++) {
				string image = grid.Images[i];
				Bitmap img = Reader.Sprites[image][0, 1];
				g.DrawImage(img, new Rectangle(mapBounds.X + mapBounds.Width, mapBounds.Y + mapBounds.Height, mapBounds.Width * (grid.Width - 2), mapBounds.Height * (grid.Height - 2)), 0, 0, img.Width, img.Height, GraphicsUnit.Pixel);
			}

			int xOrig = mapBounds.X;
			int yOrig = mapBounds.Y;
			int size = grid.Cells.Count;
			for (int i = 0; i < size; i++) {
				Cell cell = grid.Cells[i];
				int items = cell.Objects.Count;
				cell.Objects.Sort();

				for (int j = 0; j < items; j++) {
					Item item = cell.Objects[j];
					DrawSprite(grid, g, mapBounds, item, palette, showStacked && items > 1 && j + 1 == items ? items : 0);
				}

				mapBounds.X += mapBounds.Width;
				if (mapBounds.X >= rowEnd) {
					mapBounds.Y += mapBounds.Height;
					mapBounds.X = xOrig;
				}
			}

			if (showDirections) {
				mapBounds.X = xOrig;
				mapBounds.Y = yOrig;

				DrawDirections(g, grid, mapBounds, xOrig, rowEnd);
			}

			int selectorX;
			if (!int.TryParse(grid.Info["general", "selectorX"], out selectorX)) {
				selectorX = -1;
			}
			int selectorY;
			if (!int.TryParse(grid.Info["general", "selectorY"], out selectorY)) {
				selectorY = -1;
			}
			if (selectorX >= 0 && selectorY >= 0 && selectorX < grid.Width && selectorY < grid.Height) {
				mapBounds.X = xOrig + mapBounds.Width * selectorX;
				mapBounds.Y = yOrig + mapBounds.Height * selectorY;
				DrawSprite(grid, g, mapBounds, Item.SELECTOR, palette);
			}
		}
		private static void DrawDirections(Graphics g, Grid grid, Rectangle bounds, int xOrig, int rowEnd) {
			int penWidth = bounds.Width / 16;
			penWidth = penWidth > 0 ? penWidth : 1;
			int arrowSize = bounds.Width / 9;
			arrowSize = arrowSize > 0 ? arrowSize : 1;
			AdjustableArrowCap endCap = new AdjustableArrowCap(arrowSize, arrowSize, true);
			endCap.WidthScale = 0;
			int size = grid.Cells.Count;
			for (int i = 0; i < size; i++) {
				Cell cell = grid.Cells[i];
				int items = cell.Objects.Count;

				for (int j = 0; j < items; j++) {
					Item item = cell.Objects[j];
					if (item.ID == 0) { continue; }

					Color penColor;
					if (items - j == 1) {
						penColor = Color.Red;
					} else if (items - j == 2) {
						penColor = Color.Orange;
					} else if (items - j == 3) {
						penColor = Color.Blue;
					} else if (items - j == 4) {
						penColor = Color.Gray;
					} else {
						penColor = Color.Teal;
					}
					using (Pen pen = new Pen(penColor, penWidth)) {
						int endX = 0;
						int endY = 0;

						switch ((Direction)item.Direction) {
							case Direction.Right:
								endX = bounds.X + bounds.Width - 1;
								endY = bounds.Y + bounds.Height / 2;
								break;
							case Direction.Up:
								endX = bounds.X + bounds.Width / 2;
								endY = bounds.Y;
								break;
							case Direction.Left:
								endX = bounds.X;
								endY = bounds.Y + bounds.Height / 2;
								break;
							case Direction.Down:
								endX = bounds.X + bounds.Width / 2;
								endY = bounds.Y + bounds.Height - 1;
								break;
						}

						pen.CustomEndCap = endCap;
						g.PageUnit = GraphicsUnit.Pixel;
						g.DrawLine(pen, bounds.X + bounds.Width / 2, bounds.Y + bounds.Height / 2, endX, endY);
					}
				}

				bounds.X += bounds.Width;
				if (bounds.X >= rowEnd) {
					bounds.Y += bounds.Height;
					bounds.X = xOrig;
				}
			}
		}
		public static void DrawSprite(Graphics g, Rectangle destination, Sprite sprite, Color color) {
			Bitmap image = sprite[0, 1];
			DrawImage(g, image, destination, color);
		}
		public static Bitmap DrawSprite(Item item, int imgSize, Palette palette) {
			Bitmap img = new Bitmap(imgSize, imgSize);
			Rectangle rect = new Rectangle(0, 0, img.Width, img.Height);
			using (Graphics g = Graphics.FromImage(img)) {
				using (SolidBrush brush = new SolidBrush(palette.Background)) {
					g.FillRectangle(brush, rect);
				}
				DrawSprite(null, g, rect, item, palette);
			}
			return img;
		}
		public static void DrawSprite(Grid grid, Graphics g, Rectangle destination, Item item, Palette palette, int stackCount = 0) {
			Color color = palette.Colors[item.Active && item.ActiveColor != -1 ? item.ActiveColor : item.Color];

			if (item.ID == 0 || string.IsNullOrEmpty(item.Sprite)) {
				using (SolidBrush brush = new SolidBrush(item.ID == 0 ? palette.Edge : color)) {
					g.FillRectangle(brush, destination);
				}
				return;
			}

			Sprite sprite = null;
			if (item.ID != short.MaxValue && !Reader.Sprites.TryGetValue(item.Sprite, out sprite)) {
				throw new Exception($"Failed to find Sprite {item.Sprite}");
			}
			Bitmap image = item.ID == short.MaxValue ? Selector : sprite[0, 1];
			switch ((Tiling)item.Tiling) {
				case Tiling.Directional:
				case Tiling.Animated:
					switch ((Direction)item.Direction) {
						case Direction.Up:
							image = sprite[8, 1];
							break;
						case Direction.Left:
							image = sprite[16, 1];
							break;
						case Direction.Down:
							image = sprite[24, 1];
							break;
					}
					break;
				case Tiling.Tiled:
					if (grid != null) {
						bool isLevelLine = item is Level || item is Line;
						int position = item.Position + grid.Width;
						int value = 0;
						Cell cell;
						if (position < grid.Cells.Count) {
							cell = grid.Cells[position];
							value = cell.ContainsObjectType(item) || IsEdge(grid, position) || (isLevelLine && cell.HasLevelPath()) ? 8 : 0;
						}

						position = item.Position - 1;
						if (position >= 0) {
							cell = grid.Cells[position];
							value |= cell.ContainsObjectType(item) || IsEdge(grid, position) || (isLevelLine && cell.HasLevelPath()) ? 4 : 0;
						}

						position = item.Position - grid.Width;
						if (position >= 0) {
							cell = grid.Cells[position];
							value |= cell.ContainsObjectType(item) || IsEdge(grid, position) || (isLevelLine && cell.HasLevelPath()) ? 2 : 0;
						}

						position = item.Position + 1;
						if (position < grid.Cells.Count) {
							cell = grid.Cells[position];
							value |= cell.ContainsObjectType(item) || IsEdge(grid, position) || (isLevelLine && cell.HasLevelPath()) ? 1 : 0;
						}

						image = sprite[value, 1];
					}
					break;
				case Tiling.Character:
					switch ((Direction)item.Direction) {
						case Direction.Up:
							image = sprite[8, 1];
							break;
						case Direction.Left:
							image = sprite[16, 1];
							break;
						case Direction.Down:
							image = sprite[24, 1];
							break;
					}
					if (item.Sleeping) {
						image = sprite[31, 1];
					}
					break;
			}

			if (item is Level level) {
				int inc = 40;
				Color colorText = palette.Colors[item.Color];
				Color colorInc = ColorUtil.TransformBrightness(colorText, ColorUtil.ColorTransformMode.Hsb, 0.6);
				if (colorInc.R == 0 && colorInc.G == 0 && colorInc.B == 0) {
					colorInc = Color.FromArgb(color.R + inc, color.G + inc, color.B + inc);
				}

				using (GraphicsPath path = RoundedRect(destination, destination.Width / 3)) {
					using (SolidBrush brush = new SolidBrush(colorInc)) {
						g.FillPath(brush, path);
					}

					if (level.Style == (byte)LevelStyle.Icon) {
						DrawImage(g, image, destination, color);
					}

					using (Pen pen = new Pen(colorText, destination.Width / 10)) {
						g.DrawPath(pen, path);
					}
				}

				if (level.Style == (byte)LevelStyle.Number || level.Style == (byte)LevelStyle.Letter) {
					using (SolidBrush brush = new SolidBrush(color)) {
						int fontWidth = destination.Width * 2 / 3;
						if (fontWidth <= 0) { fontWidth = 1; }
						using (Font font = new Font(FontFamily.GenericSansSerif, fontWidth, FontStyle.Bold, GraphicsUnit.Pixel)) {
							int number = level.Number;
							if (level.Style == (byte)LevelStyle.Number) {
								number = number > 99 ? 99 : number;
							} else {
								number = number > 25 ? 25 : number;
							}
							string text = level.Style == (byte)LevelStyle.Number ? number.ToString("00") : ((char)(number + 0x41)).ToString();
							SizeF textSize = g.MeasureString(text, font, 9999999, StringFormat.GenericTypographic);
							g.DrawString(text, font, brush, new Point(destination.X + (int)(destination.Width / 2 - textSize.Width / 2), destination.Y + (int)(destination.Height / 2 - textSize.Height / 2)), StringFormat.GenericTypographic);
						}
					}
				} else if (level.Style == (byte)LevelStyle.Dot) {
					DrawDots(g, destination, level.Number, color);
				}
			} else {
				DrawImage(g, image, destination, color);
			}

			if (stackCount > 0) {
				using (SolidBrush brush = new SolidBrush(Color.Red)) {
					int fontWidth = destination.Width / 3;
					if (fontWidth <= 0) { fontWidth = 1; }
					using (Font font = new Font(FontFamily.GenericSansSerif, fontWidth, FontStyle.Bold, GraphicsUnit.Pixel)) {
						string text = stackCount.ToString();
						SizeF textSize = g.MeasureString(text, font, 9999999, StringFormat.GenericTypographic);
						g.DrawString(text, font, brush, new Point(destination.X + 1, destination.Y - 2), StringFormat.GenericTypographic);
					}
				}
			}
		}
		private static void DrawDots(Graphics g, Rectangle destination, int number, Color inside) {
			int diameter = destination.Width / 4 - 1;
			diameter = diameter < 1 ? 1 : diameter;
			int offset = diameter * 2 / 3;
			switch (number) {
				case 0:
					DrawDot(g, (destination.Width - diameter) / 2 + destination.X, (destination.Height - diameter) / 2 + destination.Y, diameter, diameter, inside);
					break;
				case 1:
					DrawDot(g, (destination.Width - diameter) / 2 + destination.X, destination.Height / 2 - diameter - offset + destination.Y, diameter, diameter, inside);
					DrawDot(g, (destination.Width - diameter) / 2 + destination.X, destination.Height / 2 + offset + destination.Y, diameter, diameter, inside);
					break;
				case 2:
					DrawDot(g, (destination.Width - diameter) / 2 + destination.X, destination.Height / 2 - diameter - offset + destination.Y, diameter, diameter, inside);
					DrawDot(g, destination.Width / 2 - diameter - offset + destination.X, destination.Height / 2 + offset + destination.Y, diameter, diameter, inside);
					DrawDot(g, destination.Width / 2 + offset + destination.X, destination.Height / 2 + offset + destination.Y, diameter, diameter, inside);
					break;
				case 3:
					DrawDot(g, destination.Width / 2 - diameter - offset + destination.X, destination.Height / 2 - diameter - offset + destination.Y, diameter, diameter, inside);
					DrawDot(g, destination.Width / 2 + offset + destination.X, destination.Height / 2 - diameter - offset + destination.Y, diameter, diameter, inside);
					DrawDot(g, destination.Width / 2 - diameter - offset + destination.X, destination.Height / 2 + offset + destination.Y, diameter, diameter, inside);
					DrawDot(g, destination.Width / 2 + offset + destination.X, destination.Height / 2 + offset + destination.Y, diameter, diameter, inside);
					break;
				case 4:
					DrawDot(g, destination.Width / 2 - diameter - offset + destination.X, destination.Height / 2 - diameter - offset + destination.Y, diameter, diameter, inside);
					DrawDot(g, destination.Width / 2 + offset + destination.X, destination.Height / 2 - diameter - offset + destination.Y, diameter, diameter, inside);
					DrawDot(g, destination.Width / 2 - diameter - offset + destination.X, destination.Height / 2 + offset + destination.Y, diameter, diameter, inside);
					DrawDot(g, destination.Width / 2 + offset + destination.X, destination.Height / 2 + offset + destination.Y, diameter, diameter, inside);
					DrawDot(g, (destination.Width - diameter) / 2 + destination.X, (destination.Height - diameter) / 2 + destination.Y, diameter, diameter, inside);
					break;
				case 5:
					DrawDot(g, destination.Width / 2 - diameter - offset + destination.X, destination.Height / 2 - diameter - offset + destination.Y, diameter, diameter, inside);
					DrawDot(g, destination.Width / 2 + offset + destination.X, destination.Height / 2 - diameter - offset + destination.Y, diameter, diameter, inside);
					DrawDot(g, destination.Width / 2 - diameter - offset + destination.X, (destination.Height - diameter) / 2 + destination.Y, diameter, diameter, inside);
					DrawDot(g, destination.Width / 2 + offset + destination.X, (destination.Height - diameter) / 2 + destination.Y, diameter, diameter, inside);
					DrawDot(g, destination.Width / 2 - diameter - offset + destination.X, destination.Height / 2 + offset + destination.Y, diameter, diameter, inside);
					DrawDot(g, destination.Width / 2 + offset + destination.X, destination.Height / 2 + offset + destination.Y, diameter, diameter, inside);
					break;
				case 6:
					DrawDot(g, destination.Width / 2 - diameter - offset + destination.X, destination.Height / 2 - diameter - offset + destination.Y, diameter, diameter, inside);
					DrawDot(g, destination.Width / 2 + offset + destination.X, destination.Height / 2 - diameter - offset + destination.Y, diameter, diameter, inside);
					DrawDot(g, destination.Width / 2 - diameter - offset + destination.X, destination.Height / 2 + offset + destination.Y, diameter, diameter, inside);
					DrawDot(g, destination.Width / 2 + offset + destination.X, destination.Height / 2 + offset + destination.Y, diameter, diameter, inside);
					DrawDot(g, destination.Width / 2 - diameter - offset + destination.X, (destination.Height - diameter) / 2 + destination.Y, diameter, diameter, inside);
					DrawDot(g, destination.Width / 2 + offset + destination.X, (destination.Height - diameter) / 2 + destination.Y, diameter, diameter, inside);
					DrawDot(g, (destination.Width - diameter) / 2 + destination.X, (destination.Height - diameter) / 2 + destination.Y, diameter, diameter, inside);
					break;
				case 7:
					DrawDot(g, destination.Width / 2 - diameter - offset + destination.X, destination.Height / 2 - diameter - offset + destination.Y, diameter, diameter, inside);
					DrawDot(g, destination.Width / 2 + offset + destination.X, destination.Height / 2 - diameter - offset + destination.Y, diameter, diameter, inside);
					DrawDot(g, (destination.Width - diameter) / 2 + destination.X, destination.Height / 2 - diameter - offset + destination.Y, diameter, diameter, inside);
					DrawDot(g, (destination.Width - diameter) / 2 + destination.X, destination.Height / 2 + offset + destination.Y, diameter, diameter, inside);
					DrawDot(g, destination.Width / 2 - diameter - offset + destination.X, destination.Height / 2 + offset + destination.Y, diameter, diameter, inside);
					DrawDot(g, destination.Width / 2 + offset + destination.X, destination.Height / 2 + offset + destination.Y, diameter, diameter, inside);
					DrawDot(g, destination.Width / 2 - diameter - offset + destination.X, (destination.Height - diameter) / 2 + destination.Y, diameter, diameter, inside);
					DrawDot(g, destination.Width / 2 + offset + destination.X, (destination.Height - diameter) / 2 + destination.Y, diameter, diameter, inside);
					break;
				case 8:
					DrawDot(g, destination.Width / 2 - diameter - offset + destination.X, destination.Height / 2 - diameter - offset + destination.Y, diameter, diameter, inside);
					DrawDot(g, destination.Width / 2 + offset + destination.X, destination.Height / 2 - diameter - offset + destination.Y, diameter, diameter, inside);
					DrawDot(g, (destination.Width - diameter) / 2 + destination.X, destination.Height / 2 - diameter - offset + destination.Y, diameter, diameter, inside);
					DrawDot(g, (destination.Width - diameter) / 2 + destination.X, destination.Height / 2 + offset + destination.Y, diameter, diameter, inside);
					DrawDot(g, destination.Width / 2 - diameter - offset + destination.X, destination.Height / 2 + offset + destination.Y, diameter, diameter, inside);
					DrawDot(g, destination.Width / 2 + offset + destination.X, destination.Height / 2 + offset + destination.Y, diameter, diameter, inside);
					DrawDot(g, destination.Width / 2 - diameter - offset + destination.X, (destination.Height - diameter) / 2 + destination.Y, diameter, diameter, inside);
					DrawDot(g, destination.Width / 2 + offset + destination.X, (destination.Height - diameter) / 2 + destination.Y, diameter, diameter, inside);
					DrawDot(g, (destination.Width - diameter) / 2 + destination.X, (destination.Height - diameter) / 2 + destination.Y, diameter, diameter, inside);
					break;
				default:
					DrawDot(g, destination.Width / 2 - diameter + destination.X, destination.Height / 2 - diameter + destination.Y, diameter * 2, diameter * 2, inside);
					break;
			}
		}
		private static void DrawDot(Graphics g, int x, int y, int w, int h, Color inside) {
			using (SolidBrush brush = new SolidBrush(inside)) {
				g.FillEllipse(brush, x, y, w, h);
			}
		}
		public static GraphicsPath RoundedRect(Rectangle bounds, int radius) {
			int diameter = radius * 2;
			Size size = new Size(diameter, diameter);
			Rectangle arc = new Rectangle(bounds.Location, size);
			GraphicsPath path = new GraphicsPath();

			if (radius == 0) {
				path.AddRectangle(bounds);
				return path;
			}

			path.AddArc(arc, 180, 90);
			arc.X = bounds.Right - diameter;
			path.AddArc(arc, 270, 90);
			arc.Y = bounds.Bottom - diameter;
			path.AddArc(arc, 0, 90);
			arc.X = bounds.Left;
			path.AddArc(arc, 90, 90);
			path.CloseFigure();
			return path;
		}
		private static void DrawImage(Graphics g, Image image, Rectangle destination, Color color) {
			ColorMatrix matrix = new ColorMatrix();
			matrix.Matrix00 = color.R / 255f;
			matrix.Matrix11 = color.G / 255f;
			matrix.Matrix22 = color.B / 255f;
			ImageAttributes attributes = new ImageAttributes();
			attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
			g.InterpolationMode = InterpolationMode.NearestNeighbor;
			g.PixelOffsetMode = PixelOffsetMode.Half;
			g.DrawImage(image, destination, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, attributes);
		}
		private static bool IsEdge(Grid grid, int position) {
			int mod = position % grid.Width;
			return position < grid.Width || mod == 0 || mod == grid.Width - 1 || position >= grid.Width * (grid.Height - 1);
		}
	}
}