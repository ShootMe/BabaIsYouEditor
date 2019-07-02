using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Windows.Forms;
namespace BabaIsYou.Map {
	public class Renderer {
		public static PrivateFontCollection CustomFont;
		private static Font GlobalFont;
		private static Bitmap Selector, Petal;
		private static Bitmap SpecialIcon, DownIcon, IdleIcon, LeftIcon, PauseIcon, RightIcon, UndoIcon, UpIcon;
		public static Bitmap Error;
		private static Sprite Levels = new Sprite("Level", true);

		static Renderer() {
			Selector = GetBitmapFromAssembly("BabaIsYou.Images.grid.png");
			Petal = GetBitmapFromAssembly("BabaIsYou.Images.petal.png");
			SpecialIcon = GetBitmapFromAssembly("BabaIsYou.Images.special_icon.png");
			DownIcon = GetBitmapFromAssembly("BabaIsYou.Images.down.png");
			IdleIcon = GetBitmapFromAssembly("BabaIsYou.Images.idle.png");
			LeftIcon = GetBitmapFromAssembly("BabaIsYou.Images.left.png");
			PauseIcon = GetBitmapFromAssembly("BabaIsYou.Images.pause.png");
			RightIcon = GetBitmapFromAssembly("BabaIsYou.Images.right.png");
			UndoIcon = GetBitmapFromAssembly("BabaIsYou.Images.undo_icon.png");
			UpIcon = GetBitmapFromAssembly("BabaIsYou.Images.up.png");
			Error = GetBitmapFromAssembly("BabaIsYou.Images.error.png");
			Levels[0, 1] = new SpriteImage(GetBitmapFromAssembly("BabaIsYou.Images.level1.png"), "Level_1");
			Levels[0, 2] = new SpriteImage(GetBitmapFromAssembly("BabaIsYou.Images.level2.png"), "Level_2");
			Levels[0, 3] = new SpriteImage(GetBitmapFromAssembly("BabaIsYou.Images.level3.png"), "Level_3");
			using (Stream fontStream = typeof(Renderer).Assembly.GetManifestResourceStream("BabaIsYou.Images.Consolas.ttf")) {
				byte[] fontdata = new byte[fontStream.Length];
				fontStream.Read(fontdata, 0, (int)fontStream.Length);
				CustomFont = new PrivateFontCollection();
				unsafe {
					fixed (byte* pFontData = fontdata) {
						CustomFont.AddMemoryFont((IntPtr)pFontData, fontdata.Length);
					}
				}
			}
			GlobalFont = new Font(CustomFont.Families[0], 8, FontStyle.Regular, GraphicsUnit.Point);
		}
		public static Bitmap GetBitmapFromAssembly(string name) {
			using (Stream stream = typeof(Renderer).Assembly.GetManifestResourceStream(name)) {
				return (Bitmap)Bitmap.FromStream(stream);
			}
		}
		public static void SetFonts(Control control, float customSize = -1, Font font = null) {
			if (font == null) {
				font = customSize <= 0 ? GlobalFont : new Font(CustomFont.Families[0], customSize, FontStyle.Regular, GraphicsUnit.Point);
			}
			control.Font = font;
			foreach (Control ctr in control.Controls) {
				ctr.Font = font;
				if (ctr.HasChildren) {
					SetFonts(ctr, customSize, font);
				}
			}
		}
		public static Size GetSizeForCount(int count, int maxX = int.MaxValue, int maxY = int.MaxValue) {
			int sizeX = (int)Math.Sqrt(count);
			int sizeY = sizeX;
			if (sizeX > maxX) { sizeX = maxX; }
			if (sizeY > maxY) { sizeY = maxY; }

			bool isY = true;
			while ((sizeX < maxX || sizeY < maxY) && sizeX * sizeY < count) {
				if (isY) {
					sizeY++;
					isY = false;
				} else {
					sizeX++;
					isY = true;
				}
			}
			return new Size(sizeX, sizeY);
		}
		public static Size GetFillSizeForCount(int count, int xDim, int yDim) {
			int area = xDim * yDim;
			int size = (int)Math.Sqrt(area / count);

			while ((xDim / size) * (yDim / size) < count) {
				size--;
			}
			return new Size(xDim / size, yDim / size);
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
		public static void Render(Grid grid, Graphics g, int totalWidth, int totalHeight, int frameNumber = 0, bool showStacked = false, bool showDirections = false) {
			Rectangle mapBounds = GetBounds(grid, totalWidth, totalHeight);
			int rowEnd = mapBounds.X + mapBounds.Width * grid.Width;

			Palette palette;
			if (!Reader.Palettes.TryGetValue(grid.Palette, out palette)) {
				throw new Exception($"Failed to find palette {grid.Palette} for map {grid.FileName}");
			}
			g.InterpolationMode = InterpolationMode.NearestNeighbor;
			g.PixelOffsetMode = PixelOffsetMode.Half;

			using (SolidBrush brush = new SolidBrush(palette.Edge)) {
				g.FillRectangle(brush, 0, 0, totalWidth, totalHeight);
			}
			using (SolidBrush brush = new SolidBrush(palette.Background)) {
				g.FillRectangle(brush, mapBounds.X, mapBounds.Y, mapBounds.Width * grid.Width, mapBounds.Height * grid.Height);
			}

			g.Clip = new Region(new Rectangle(mapBounds.X - 1, mapBounds.Y - 1, mapBounds.Width * grid.Width + 1, mapBounds.Height * grid.Height + 1));
			int frame = (frameNumber % 3) + 1;
			for (int i = 0; i < grid.Images.Count; i++) {
				string image = grid.Images[i];
				Bitmap img = Reader.Sprites[image][0, frame].Image;
				int width = img.Width * mapBounds.Width / 24;
				if (width < 1) { width = 1; }
				int height = img.Height * mapBounds.Height / 24;
				if (height < 1) { height = 1; }
				g.DrawImage(img, new Rectangle(mapBounds.X + mapBounds.Width, mapBounds.Y + mapBounds.Height, width, height), 0, 0, img.Width, img.Height, GraphicsUnit.Pixel);
			}

			int xOrig = mapBounds.X;
			int yOrig = mapBounds.Y;
			DrawSpecials(g, grid, mapBounds, rowEnd, palette, true, frameNumber);

			mapBounds.X = xOrig;
			mapBounds.Y = yOrig;
			int size = grid.Cells.Count;
			for (int i = 0; i < size; i++) {
				Cell cell = grid.Cells[i];
				int items = cell.Objects.Count;
				cell.Objects.Sort();

				int itemCount = 0;
				Special special = cell.GetExtraObject<Special>();
				bool hasSpecialLevel = special != null && special.Type == (byte)SpecialType.Level;
				for (int j = 0; j < items; j++) {
					Item item = cell.Objects[j];
					if (!(item is Level) && !(item is LevelPath) && !(item is Special)) {
						itemCount++;
					}
					DrawSprite(grid, g, mapBounds, item, palette, frameNumber, showStacked && itemCount > 1 && j + 1 == items ? itemCount : 0, hasSpecialLevel);
				}

				mapBounds.X += mapBounds.Width;
				if (mapBounds.X >= rowEnd) {
					mapBounds.Y += mapBounds.Height;
					mapBounds.X = xOrig;
				}
			}

			mapBounds.X = xOrig;
			mapBounds.Y = yOrig;
			DrawSpecials(g, grid, mapBounds, rowEnd, palette, false, frameNumber);

			if (showDirections) {
				mapBounds.X = xOrig;
				mapBounds.Y = yOrig;

				DrawDirections(g, grid, mapBounds, rowEnd);
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
				Item cursor = null;
				if (!Reader.DefaultsByName.TryGetValue("cursor", out cursor)) {
					cursor = Item.SELECTOR;
				}
				DrawSprite(grid, g, mapBounds, cursor, palette);
			}
		}
		private static void DrawSpecials(Graphics g, Grid grid, Rectangle bounds, int rowEnd, Palette palette, bool drawLevels, int frameNumber) {
			int xOrig = bounds.X;
			int size = grid.Cells.Count;
			for (int i = 0; i < size; i++) {
				Cell cell = grid.Cells[i];
				int items = cell.Objects.Count;

				for (int j = 0; j < items; j++) {
					Item item = cell.Objects[j];
					if (item is Special specialItem) {
						if (!drawLevels && specialItem.Type == (byte)SpecialType.Flower) {
							Flower flower = specialItem.GetFlower();
							DrawFlower(g, bounds, flower.Radius, palette.Colors[flower.Color], palette.Colors[flower.InnerColor], frameNumber);
						} else if (!drawLevels && specialItem.Type == (byte)SpecialType.Controls) {
							GameControl gameControl = specialItem.GetGameControl();
							switch (gameControl.Type) {
								case ControlType.Down: g.DrawImage(DownIcon, bounds); break;
								case ControlType.Idle: g.DrawImage(IdleIcon, bounds); break;
								case ControlType.Left: g.DrawImage(LeftIcon, bounds); break;
								case ControlType.Pause: g.DrawImage(PauseIcon, bounds); break;
								case ControlType.Right: g.DrawImage(RightIcon, bounds); break;
								case ControlType.Undo: g.DrawImage(UndoIcon, bounds); break;
								case ControlType.Up: g.DrawImage(UpIcon, bounds); break;
								case ControlType.Unknown: g.DrawImage(SpecialIcon, bounds); break;
							}
						} else if (drawLevels && specialItem.Type == (byte)SpecialType.Level) {
							Level level = specialItem.GetLevel();
							if (level.Style == (byte)LevelStyle.Icon) {
								level.Style = (byte)LevelStyle.Dot;
							}
							DrawLevel(g, bounds, level, null, palette.Colors[level.ActiveColor], palette.Colors[level.Color], frameNumber);
						}
						break;
					}
				}

				bounds.X += bounds.Width;
				if (bounds.X >= rowEnd) {
					bounds.Y += bounds.Height;
					bounds.X = xOrig;
				}
			}
		}
		private static void DrawDirections(Graphics g, Grid grid, Rectangle bounds, int rowEnd) {
			int xOrig = bounds.X;
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
			SpriteImage image = sprite[0, 1];
			DrawImage(g, image?.Image, destination, color, false);
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
		public static void DrawSprite(Grid grid, Graphics g, Rectangle destination, Item item, Palette palette, int frameNumber = 0, int stackCount = 0, bool hasSpecialLevel = false) {
			Color color = palette.Colors[item.Active && item.ActiveColor != -1 && !(item is Level) ? item.ActiveColor : item.Color];
			if (item is Special specialItem) {
				if (specialItem.Type == (byte)SpecialType.Art || specialItem.Type == (byte)SpecialType.Unknown) {
					g.DrawImage(SpecialIcon, destination);
				}
				return;
			} else if (item.ID == 0 || string.IsNullOrEmpty(item.Sprite)) {
				using (SolidBrush brush = new SolidBrush(item.ID == 0 ? palette.Edge : color)) {
					g.FillRectangle(brush, destination);
				}
				return;
			}

			Sprite sprite = null;
			if (item.ID != short.MaxValue && !Reader.Sprites.TryGetValue(item.Sprite, out sprite)) {
				throw new Exception($"Failed to find Sprite {item.Sprite}");
			}
			Bitmap image = null;
			int frame = ((frameNumber + item.Position) % 3) + 1;
			if (item.ID == short.MaxValue) {
				image = Selector;
			} else {
				image = sprite[0, frame]?.Image;
			}
			switch ((Tiling)item.Tiling) {
				case Tiling.Directional:
				case Tiling.Animated:
					switch ((Direction)item.Direction) {
						case Direction.Up: image = sprite[8, frame]?.Image; break;
						case Direction.Left: image = sprite[16, frame]?.Image; break;
						case Direction.Down: image = sprite[24, frame]?.Image; break;
					}
					break;
				case Tiling.Tiled:
					if (grid != null) {
						bool isLevelPath = item is Level || item is LevelPath;
						int position = item.Position + grid.Width;
						int value = 0;
						Cell cell;
						if (position < grid.Cells.Count) {
							cell = grid.Cells[position];
							value = cell.ContainsObject(item) || IsEdge(grid, position) || (isLevelPath && cell.HasLevelPath()) ? 8 : 0;
						}

						position = item.Position - 1;
						if (position >= 0) {
							cell = grid.Cells[position];
							value |= cell.ContainsObject(item) || IsEdge(grid, position) || (isLevelPath && cell.HasLevelPath()) ? 4 : 0;
						}

						position = item.Position - grid.Width;
						if (position >= 0) {
							cell = grid.Cells[position];
							value |= cell.ContainsObject(item) || IsEdge(grid, position) || (isLevelPath && cell.HasLevelPath()) ? 2 : 0;
						}

						position = item.Position + 1;
						if (position < grid.Cells.Count) {
							cell = grid.Cells[position];
							value |= cell.ContainsObject(item) || IsEdge(grid, position) || (isLevelPath && cell.HasLevelPath()) ? 1 : 0;
						}

						image = sprite[value, frame]?.Image;
					}
					break;
				case Tiling.Character:
					switch ((Direction)item.Direction) {
						case Direction.Up: image = sprite[8, frame]?.Image; break;
						case Direction.Left: image = sprite[16, frame]?.Image; break;
						case Direction.Down: image = sprite[24, frame]?.Image; break;
					}
					if (item.Sleeping) {
						image = sprite[31, frame]?.Image;
					}
					break;
			}
			if (image == null) {
				image = sprite[0, frame]?.Image;
				if (image == null) {
					image = Error;
				}
			}

			if (item is Level level) {
				if (level.Style == (byte)LevelStyle.Icon) {
					Bitmap img = sprite[level.SpriteNum, level.SpriteNumExtra]?.Image;
					if (img != null) {
						image = img;
					}
				}
				DrawLevel(g, destination, level, image, color, palette.Colors[level.Color], frameNumber);
			} else {
				DrawImage(g, image, destination, color, hasSpecialLevel);
			}

			if (stackCount > 0) {
				int fontWidth = destination.Width / 3;
				if (fontWidth <= 0) { fontWidth = 1; }
				using (Font font = new Font(CustomFont.Families[0], fontWidth, FontStyle.Bold, GraphicsUnit.Pixel)) {
					string text = stackCount.ToString();
					SizeF textSize = g.MeasureString(text, font, 9999999, StringFormat.GenericTypographic);
					g.DrawString(text, font, Brushes.Red, new Point(destination.X + 1, destination.Y - 2), StringFormat.GenericTypographic);
				}
			}
		}
		private static void DrawLevel(Graphics g, Rectangle bounds, Level level, Bitmap image, Color color, Color colorText, int frameNumber) {
			int frame = ((frameNumber + level.Position) % 3) + 1;
			DrawImage(g, Levels[0, frame].Image, bounds, colorText, false);

			if (level.Style == (byte)LevelStyle.Icon) {
				DrawImage(g, image, bounds, color, false);
			}

			if (level.Style == (byte)LevelStyle.Number || level.Style == (byte)LevelStyle.Letter) {
				using (SolidBrush brush = new SolidBrush(color)) {
					int fontWidth = bounds.Width * 2 / 3;
					if (fontWidth <= 0) { fontWidth = 1; }
					using (Font font = new Font(CustomFont.Families[0], fontWidth, FontStyle.Bold, GraphicsUnit.Pixel)) {
						int number = level.Number;
						if (level.Style == (byte)LevelStyle.Number) {
							number = number > 99 ? 99 : number;
						} else {
							number = number > 25 ? 25 : number;
						}
						string text = level.Style == (byte)LevelStyle.Number ? number.ToString("00") : ((char)(number + 0x41)).ToString();
						SizeF textSize = g.MeasureString(text, font, 9999999, StringFormat.GenericTypographic);
						g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
						g.DrawString(text, font, brush, new Point(bounds.X + (int)((bounds.Width - textSize.Width) / 2f) + 1, bounds.Y + (int)((bounds.Height - textSize.Height) / 2f)), StringFormat.GenericTypographic);
					}
				}
			} else if (level.Style == (byte)LevelStyle.Dot) {
				DrawDots(g, bounds, level.Number, color);
			}
		}
		private static void DrawFlower(Graphics g, Rectangle bounds, int radius, Color color, Color center, int frameNumber) {
			GraphicsState state = g.Save();
			Matrix matrix = g.Transform;
			for (int i = radius; i > 0; i--) {
				int startingPoint = (frameNumber % 360) * ((i & 1) == 0 ? -4 : 4);
				float increase = (360f / (i * 8f));

				float offset = bounds.Height * 2f / 3f;
				for (int j = 1; j < i; j++) {
					offset += (float)bounds.Height * j / (j + 1) + offset / 8;
				}

				float totalRotation = startingPoint;
				matrix.RotateAt(totalRotation, new Point(bounds.X + bounds.Width / 2, bounds.Y + bounds.Height / 2));

				for (int j = i * 8; j > 0; j--) {
					totalRotation += increase;
					matrix.RotateAt(increase, new Point(bounds.X + bounds.Width / 2, bounds.Y + bounds.Height / 2));
					matrix.Translate(0, -offset);
					g.Transform = matrix;
					DrawImage(g, Petal, bounds, color, false);
					matrix.Translate(0, offset);
				}

				matrix.RotateAt(-totalRotation, new Point(bounds.X + bounds.Width / 2, bounds.Y + bounds.Height / 2));
			}

			g.Restore(state);
			using (SolidBrush brush = new SolidBrush(center)) {
				g.FillEllipse(brush, bounds.X + bounds.Width / 2 - bounds.Width / 4, bounds.Y + bounds.Height / 2 - bounds.Height / 4, bounds.Width / 2, bounds.Height / 2);
			}
		}
		private static void DrawDots(Graphics g, Rectangle bounds, int number, Color inside) {
			int diameter = bounds.Width / 4 - 1;
			diameter = diameter < 1 ? 1 : diameter;
			int offset = diameter / 2;
			switch (number) {
				case 0:
					DrawDot(g, (bounds.Width - diameter) / 2 + bounds.X, (bounds.Height - diameter) / 2 + bounds.Y, diameter, diameter, inside);
					break;
				case 1:
					DrawDot(g, (bounds.Width - diameter) / 2 + bounds.X, bounds.Height / 2 - diameter - offset + bounds.Y, diameter, diameter, inside);
					DrawDot(g, (bounds.Width - diameter) / 2 + bounds.X, bounds.Height / 2 + offset + bounds.Y, diameter, diameter, inside);
					break;
				case 2:
					DrawDot(g, (bounds.Width - diameter) / 2 + bounds.X, bounds.Height / 2 - diameter - offset + bounds.Y, diameter, diameter, inside);
					DrawDot(g, bounds.Width / 2 - diameter - offset + bounds.X, bounds.Height / 2 + offset + bounds.Y, diameter, diameter, inside);
					DrawDot(g, bounds.Width / 2 + offset + bounds.X, bounds.Height / 2 + offset + bounds.Y, diameter, diameter, inside);
					break;
				case 3:
					DrawDot(g, bounds.Width / 2 - diameter - offset + bounds.X, bounds.Height / 2 - diameter - offset + bounds.Y, diameter, diameter, inside);
					DrawDot(g, bounds.Width / 2 + offset + bounds.X, bounds.Height / 2 - diameter - offset + bounds.Y, diameter, diameter, inside);
					DrawDot(g, bounds.Width / 2 - diameter - offset + bounds.X, bounds.Height / 2 + offset + bounds.Y, diameter, diameter, inside);
					DrawDot(g, bounds.Width / 2 + offset + bounds.X, bounds.Height / 2 + offset + bounds.Y, diameter, diameter, inside);
					break;
				case 4:
					DrawDot(g, bounds.Width / 2 - diameter - offset + bounds.X, bounds.Height / 2 - diameter - offset + bounds.Y, diameter, diameter, inside);
					DrawDot(g, bounds.Width / 2 + offset + bounds.X, bounds.Height / 2 - diameter - offset + bounds.Y, diameter, diameter, inside);
					DrawDot(g, bounds.Width / 2 - diameter - offset + bounds.X, bounds.Height / 2 + offset + bounds.Y, diameter, diameter, inside);
					DrawDot(g, bounds.Width / 2 + offset + bounds.X, bounds.Height / 2 + offset + bounds.Y, diameter, diameter, inside);
					DrawDot(g, (bounds.Width - diameter) / 2 + bounds.X, (bounds.Height - diameter) / 2 + bounds.Y, diameter, diameter, inside);
					break;
				case 5:
					DrawDot(g, bounds.Width / 2 - diameter - offset + bounds.X, bounds.Height / 2 - diameter - offset + bounds.Y, diameter, diameter, inside);
					DrawDot(g, bounds.Width / 2 + offset + bounds.X, bounds.Height / 2 - diameter - offset + bounds.Y, diameter, diameter, inside);
					DrawDot(g, bounds.Width / 2 - diameter - offset + bounds.X, (bounds.Height - diameter) / 2 + bounds.Y, diameter, diameter, inside);
					DrawDot(g, bounds.Width / 2 + offset + bounds.X, (bounds.Height - diameter) / 2 + bounds.Y, diameter, diameter, inside);
					DrawDot(g, bounds.Width / 2 - diameter - offset + bounds.X, bounds.Height / 2 + offset + bounds.Y, diameter, diameter, inside);
					DrawDot(g, bounds.Width / 2 + offset + bounds.X, bounds.Height / 2 + offset + bounds.Y, diameter, diameter, inside);
					break;
				case 6:
					DrawDot(g, bounds.Width / 2 - diameter - offset + bounds.X, bounds.Height / 2 - diameter - offset + bounds.Y, diameter, diameter, inside);
					DrawDot(g, bounds.Width / 2 + offset + bounds.X, bounds.Height / 2 - diameter - offset + bounds.Y, diameter, diameter, inside);
					DrawDot(g, bounds.Width / 2 - diameter - offset + bounds.X, bounds.Height / 2 + offset + bounds.Y, diameter, diameter, inside);
					DrawDot(g, bounds.Width / 2 + offset + bounds.X, bounds.Height / 2 + offset + bounds.Y, diameter, diameter, inside);
					DrawDot(g, bounds.Width / 2 - diameter - offset + bounds.X, (bounds.Height - diameter) / 2 + bounds.Y, diameter, diameter, inside);
					DrawDot(g, bounds.Width / 2 + offset + bounds.X, (bounds.Height - diameter) / 2 + bounds.Y, diameter, diameter, inside);
					DrawDot(g, (bounds.Width - diameter) / 2 + bounds.X, (bounds.Height - diameter) / 2 + bounds.Y, diameter, diameter, inside);
					break;
				case 7:
					DrawDot(g, bounds.Width / 2 - diameter - offset + bounds.X, bounds.Height / 2 - diameter - offset + bounds.Y, diameter, diameter, inside);
					DrawDot(g, bounds.Width / 2 + offset + bounds.X, bounds.Height / 2 - diameter - offset + bounds.Y, diameter, diameter, inside);
					DrawDot(g, (bounds.Width - diameter) / 2 + bounds.X, bounds.Height / 2 - diameter - offset + bounds.Y, diameter, diameter, inside);
					DrawDot(g, (bounds.Width - diameter) / 2 + bounds.X, bounds.Height / 2 + offset + bounds.Y, diameter, diameter, inside);
					DrawDot(g, bounds.Width / 2 - diameter - offset + bounds.X, bounds.Height / 2 + offset + bounds.Y, diameter, diameter, inside);
					DrawDot(g, bounds.Width / 2 + offset + bounds.X, bounds.Height / 2 + offset + bounds.Y, diameter, diameter, inside);
					DrawDot(g, bounds.Width / 2 - diameter - offset + bounds.X, (bounds.Height - diameter) / 2 + bounds.Y, diameter, diameter, inside);
					DrawDot(g, bounds.Width / 2 + offset + bounds.X, (bounds.Height - diameter) / 2 + bounds.Y, diameter, diameter, inside);
					break;
				case 8:
					DrawDot(g, bounds.Width / 2 - diameter - offset + bounds.X, bounds.Height / 2 - diameter - offset + bounds.Y, diameter, diameter, inside);
					DrawDot(g, bounds.Width / 2 + offset + bounds.X, bounds.Height / 2 - diameter - offset + bounds.Y, diameter, diameter, inside);
					DrawDot(g, (bounds.Width - diameter) / 2 + bounds.X, bounds.Height / 2 - diameter - offset + bounds.Y, diameter, diameter, inside);
					DrawDot(g, (bounds.Width - diameter) / 2 + bounds.X, bounds.Height / 2 + offset + bounds.Y, diameter, diameter, inside);
					DrawDot(g, bounds.Width / 2 - diameter - offset + bounds.X, bounds.Height / 2 + offset + bounds.Y, diameter, diameter, inside);
					DrawDot(g, bounds.Width / 2 + offset + bounds.X, bounds.Height / 2 + offset + bounds.Y, diameter, diameter, inside);
					DrawDot(g, bounds.Width / 2 - diameter - offset + bounds.X, (bounds.Height - diameter) / 2 + bounds.Y, diameter, diameter, inside);
					DrawDot(g, bounds.Width / 2 + offset + bounds.X, (bounds.Height - diameter) / 2 + bounds.Y, diameter, diameter, inside);
					DrawDot(g, (bounds.Width - diameter) / 2 + bounds.X, (bounds.Height - diameter) / 2 + bounds.Y, diameter, diameter, inside);
					break;
				default:
					DrawDot(g, bounds.Width / 2 - diameter + bounds.X, bounds.Height / 2 - diameter + bounds.Y, diameter * 2, diameter * 2, inside);
					break;
			}
		}
		private static void DrawDot(Graphics g, int x, int y, int w, int h, Color inside) {
			using (SolidBrush brush = new SolidBrush(inside)) {
				g.FillEllipse(brush, x, y, w, h);
			}
		}
		private static void DrawImage(Graphics g, Image image, Rectangle bounds, Color color, bool drawTransparent) {
			ColorMatrix matrix = new ColorMatrix();
			matrix.Matrix00 = color.R / 255f;
			matrix.Matrix11 = color.G / 255f;
			matrix.Matrix22 = color.B / 255f;
			if (drawTransparent) {
				matrix.Matrix33 = 0.6f;
			}
			ImageAttributes attributes = new ImageAttributes();
			attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
			g.InterpolationMode = InterpolationMode.NearestNeighbor;
			g.PixelOffsetMode = PixelOffsetMode.Half;
			g.DrawImage(image, bounds, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, attributes);
		}
		private static bool IsEdge(Grid grid, int position) {
			int mod = position % grid.Width;
			return position < grid.Width || mod == 0 || mod == grid.Width - 1 || position >= grid.Width * (grid.Height - 1);
		}
	}
}