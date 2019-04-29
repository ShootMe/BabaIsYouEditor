using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
namespace BabaIsYou.Controls {
	public class ColorRadioButton : RadioButton {
		private Color onColor;
		private Color offColor;
		private Rectangle circle;
		private Pen outline;

		public Color OnColor {
			get { return onColor; }
			set {
				if (value == Color.White || value == Color.Transparent) {
					onColor = Color.Empty;
				} else {
					onColor = value;
				}
			}
		}
		public Color OffColour {
			get { return offColor; }
			set {
				if (value == Color.White || value == Color.Transparent) {
					offColor = Color.Empty;
				} else {
					offColor = value;
				}
			}
		}

		public ColorRadioButton() {
			circle = new Rectangle(2, 4, 8, 8);
			outline = new Pen(new SolidBrush(Color.Black), 1F);

			SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			BackColor = Color.Transparent;
		}
		protected override void OnPaint(PaintEventArgs e) {
			base.OnPaint(e);

			Graphics g = e.Graphics;
			g.SmoothingMode = SmoothingMode.AntiAlias;

			if (this.Checked) {
				if (OnColor != Color.Empty) {
					g.FillEllipse(new SolidBrush(OnColor), circle);
					g.DrawEllipse(outline, circle);
				}
			} else if (OffColour != Color.Empty) {
				g.FillEllipse(new SolidBrush(OffColour), circle);
				g.DrawEllipse(outline, circle);
			}
		}
	}
}