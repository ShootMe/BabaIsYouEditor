using System.Drawing;
using System.Windows.Forms;
namespace BabaIsYou.Controls {
	public class MessageFilter : Control, IMessageFilter {
		private const int WM_MOUSEWHEEL = 0x020A;

		public MessageFilter() {
			Application.AddMessageFilter(this);
		}
		~MessageFilter() {
			Application.RemoveMessageFilter(this);
		}

		public bool PreFilterMessage(ref Message m) {
			switch (m.Msg) {
				case WM_MOUSEWHEEL:
					Point point = new Point((int)(long)m.LParam);
					Point controlPoint = this.PointToClient(point);
					if (this.ClientRectangle.Contains(controlPoint)) {
						WndProc(ref m);
						return true;
					}
					break;
			}
			return false;
		}
	}
}