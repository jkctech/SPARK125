using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace SPARK125
{
	public class DualButton : Button
	{
		public string TopText { get; set; }
		public string BottomText { get; set; }
		public SolidBrush TopBrush { get; set; }
		public SolidBrush BottomBrush { get; set; }
		public int LineSpacing { get; set; }
		
		private StringFormat _sf;

		public DualButton(string TopText = "", string BottomText = "")
		{
			this.TopText = TopText;
			this.BottomText = BottomText;

			TopBrush = new SolidBrush(Color.Black);
			BottomBrush = new SolidBrush(Color.Red);

			LineSpacing = -4;

			_sf = new StringFormat();
			_sf.LineAlignment = StringAlignment.Center;
			_sf.Alignment = StringAlignment.Center;

			Font = new Font(Font, FontStyle.Bold);
		}

		public DualButton(string TopText, string BottomText, SolidBrush TopBrush, SolidBrush BottomBrush)
		{
			this.TopText = TopText;
			this.BottomText = BottomText;

			this.TopBrush = TopBrush;
			this.BottomBrush = BottomBrush;

			LineSpacing = -4;

			_sf = new StringFormat();
			_sf.LineAlignment = StringAlignment.Center;
			_sf.Alignment = StringAlignment.Center;

			Font = new Font(Font, FontStyle.Bold);
		}

		// Custom painting override.
		// Allows for multi-color buttons
		protected override void OnPaint(PaintEventArgs e)
		{
			// Paint base button
			base.OnPaint(e);

			// Top text
			e.Graphics.DrawString(
				TopText, 
				Font, 
				TopBrush, 
				new RectangleF(
					new Point(0, LineSpacing / 2 * -1 - LineSpacing / 2), 
					new Size(Size.Width, Size.Height / 2)
				),
				_sf
			);

			// Bottom Text
			e.Graphics.DrawString(
				BottomText,
				Font,
				BottomBrush,
				new RectangleF(
					new Point(0, Height / 2 + LineSpacing / 2),
					new Size(Size.Width, Size.Height / 2)
				),
				_sf
			);
		}
	}
}
