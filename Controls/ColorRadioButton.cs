using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
namespace BabaIsYou.Controls {
	public class ColorRadioButton : RadioButton {
		private Color onColor;
		private Color offColor;
		private Rectangle circle;

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

			SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			BackColor = Color.Transparent;
		}
		protected override void OnPaint(PaintEventArgs e) {
			base.OnPaint(e);

			Graphics g = e.Graphics;
			g.SmoothingMode = SmoothingMode.AntiAlias;

			if (this.Checked) {
				if (OnColor != Color.Empty) {
					using (SolidBrush brush = new SolidBrush(OnColor)) {
						g.FillEllipse(brush, circle);
					}
					g.DrawEllipse(Pens.Black, circle);
				}
			} else if (OffColour != Color.Empty) {
				using (SolidBrush brush = new SolidBrush(OffColour)) {
					g.FillEllipse(brush, circle);
				}
				g.DrawEllipse(Pens.Black, circle);
			}
		}
	}
}