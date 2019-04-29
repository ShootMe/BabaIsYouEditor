using System;
using System.Windows.Forms;
namespace BabaIsYou.Controls {
	public class NumericBox : NumericUpDown {
		protected override void OnMouseWheel(MouseEventArgs e) {
			HandledMouseEventArgs hme = e as HandledMouseEventArgs;
			if (hme != null) {
				hme.Handled = true;
			}

			if (e.Delta > 0 && this.Value < this.Maximum) {
				this.Value += this.Increment;
			} else if (e.Delta < 0 && this.Value > this.Minimum) {
				this.Value -= this.Increment;
			}
		}
		protected override void OnEnter(EventArgs e) {
			base.OnEnter(e);
			if (Text.Length > 0) {
				Select(0, Text.Length);
			}
		}
	}
}